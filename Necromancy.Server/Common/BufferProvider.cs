using Arrowgene.Buffers;

namespace Necromancy.Server.Common
{
    public class BufferProvider
    {
        private static readonly IBufferProvider _Provider = new StreamBuffer();

        public static IBuffer Provide()
        {
            return _Provider.Provide();
        }

        public static IBuffer Provide(byte[] data)
        {
            return _Provider.Provide(data);
        }
    }
}
