using System;
using System.Data.Common;
using Arrowgene.Logging;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Database.Sql.Core
{
    /// <summary>
    ///     Implementation of Necromancy database operations.
    /// </summary>
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(NecSqlDb<TCon, TCom>));

        protected override void Exception(Exception ex)
        {
            _Logger.Exception(ex);
        }
    }
}
