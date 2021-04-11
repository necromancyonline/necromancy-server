using System;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Logging;

namespace Necromancy.Server.Tasks.Core
{
    public abstract class PeriodicTask
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(PeriodicTask));

        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;

        public abstract string taskName { get; }
        public abstract TimeSpan taskTimeSpan { get; }

        protected abstract void Execute();
        protected abstract bool taskRunAtStart { get; }

        public void Start()
        {
            if (_task != null)
            {
                _Logger.Error($"Task {taskName} already started");
                return;
            }


            _cancellationTokenSource = new CancellationTokenSource();
            _task = new Task(Run, _cancellationTokenSource.Token);
            _task.Start();
        }

        public void Stop()
        {
            if (_task == null)
            {
                _Logger.Error($"Task {taskName} already stopped");
                return;
            }

            _cancellationTokenSource.Cancel();
            _task = null;
        }

        private async void Run()
        {
            _Logger.Debug($"Task {taskName} started");
            if (taskRunAtStart)
            {
                _Logger.Trace($"Task {taskName} run");
                ExecuteUserCode();
                _Logger.Trace($"Task {taskName} completed");
            }

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(taskTimeSpan, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    _Logger.Debug($"Task {taskName} canceled");
                }

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _Logger.Trace($"Task {taskName} run");
                    ExecuteUserCode();
                    _Logger.Trace($"Task {taskName} completed");
                }
            }

            _Logger.Debug($"Task {taskName} ended");
        }

        private void ExecuteUserCode()
        {
            try
            {
                Execute();
            }
            catch (Exception ex)
            {
                _Logger.Error($"Task {taskName} crashed.  Stopping Task");
                _Logger.Exception(ex);
                Stop();
            }
        }
    }
}
