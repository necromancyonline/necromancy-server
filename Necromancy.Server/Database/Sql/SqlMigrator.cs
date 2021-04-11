using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Arrowgene.Logging;

namespace Necromancy.Server.Database.Sql
{
    /// <summary>
    /// Apply SQL migrations to a database with a set of SQL migration scripts.
    /// </summary>
    public class SqlMigrator
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(SqlMigrator));

        /// <summary>The SQL database to apply migrations to.</summary>
        private readonly IDatabase _database;

        /// <param name="database">The SQL database to apply migrations to.</param>
        public SqlMigrator(IDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Apply all new migrations, by version number, in the given database migrations folder.
        /// </summary>
        /// <param name="migrationsFolder">Database migrations folder.</param>
        /// <remarks>
        /// SQL migration file names must start with a version number, and end with the .sql extension.
        /// There is only one migration file per version number.
        /// Throws ArgumentException if two migration files have the same version.
        /// Throws ArgumentException if the version=>file dictionary does not give us the keys in order.
        /// Throws FormatException on file name format error.
        /// </remarks>
        public void Migrate(string migrationsFolder)
        {
            long initialVersion = _database.version, versionNumber = 0;
            var versionFileDict = new SortedList<long, string>();
            /* Prepare migration files, ordered by version number. */
            foreach (var f in Directory.GetFiles(migrationsFolder, "*.sql"))
            {
                /* Migration naming scheme: [0-9]+-<description>.sql */
                if (!long.TryParse(Regex.Match(Path.GetFileName(f), @"\d+").Value, NumberStyles.None,
                    NumberFormatInfo.InvariantInfo, out versionNumber))
                {
                    LogException(new FormatException(String.Format(
                        "Failed to detect DB version for migration file: {0}. File name should begin with a database version number.",
                        f)));
                }

                /* Ignore 0-version, this is an example. Adding the same version twice will throw
                   an ArgumentException. */
                if (versionNumber > initialVersion)
                    versionFileDict.Add(versionNumber, f);
            }

            ApplyMigrations(versionFileDict);
            if ((versionNumber = _database.version) > initialVersion)
            {
                _Logger.Debug($"Database db.sqlite version is updated to {versionNumber}.");
            }
        }

        /// <summary>
        /// Apply all migrations in-order per sql file, and update the DB version.
        /// </summary>
        /// <param name="versionFileDict">Map of version number to migration file path.
        /// The keys are required to be sorted to apply in order.</param>
        /// <remarks>
        /// Throws ArgumentException if the version=>file dictionary does not give us the keys in order.
        /// </remarks>
        private void ApplyMigrations(IDictionary<long, string> versionFileDict)
        {
            long initialVersion = _database.version, versionNumber = 0;
            ScriptRunner scriptRunner = new ScriptRunner(_database);
            /* Assert: All entries' version is above current DB version. */
            foreach (KeyValuePair<long, string> kvp in versionFileDict)
            {
                /* Assert: Keys are in-order to apply migrations in-order. */
                if (kvp.Key < versionNumber)
                {
                    LogException(new ArgumentException(
                        "A sorted list's keys are out of order, and expected to be in-order."));
                }

                scriptRunner.Run(kvp.Value);
                _database.version = versionNumber = kvp.Key;
            }
        }

        /// <summary>
        /// Log the given exception and throw as a runtime error.
        /// </summary>
        private void LogException(Exception exception)
        {
            _Logger.Error(exception.Message);
            _Logger.Exception(exception);
            throw exception;
        }
    }
}
