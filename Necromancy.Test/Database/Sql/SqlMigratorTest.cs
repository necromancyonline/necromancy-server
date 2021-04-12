using System;
using System.Collections.Generic;
using System.IO;
using Necromancy.Server.Database;
using Necromancy.Server.Database.Sql;
using Xunit;
using Xunit.Abstractions;

namespace Necromancy.Test.Database.Sql
{
    public class SqlMigratorTest
    {
        private const string _SqliteFile = "TestMigrations/db.sqlite";
        private const string _MigrationDir = "TestMigrations/Script/Migrations/";
        private const string _First = _MigrationDir + "1-first.sql";

        private const string _Second = _MigrationDir + "2.sql";

        /* 0 is a sample and will be silently ignored. */
        private const string _IgnoreZero = _MigrationDir + "0.sql";

        /* Non-sql files should be ignored. */
        private const string _IgnoreText = _MigrationDir + "notsql.txt";

        /* Same version, will throw ArgumentException. */
        private const string _FailDuplicate = _MigrationDir + "1-dup.sql";

        /* No version, will throw FormatException. */
        private const string _FailNoversion = _MigrationDir + "noversion.sql";

        private readonly ITestOutputHelper _output;

        public SqlMigratorTest(ITestOutputHelper output)
        {
            _output = output;

            /* Create an artificial migrations environment to test specific requirements. */
            PrepFiles();
        }

        [Fact]
        public void TestSqlLite()
        {
            IDatabase db = new NecSqLiteDb(_SqliteFile);
            db.CreateDatabase();
            Assert.Equal(0, db.version);

            /* Valid case, no errors. */
            TestValid(db);

            /* Test sync */
            IDatabase db2 = new NecSqLiteDb(_SqliteFile);
            db.CreateDatabase();
            db.version = 11;
            Assert.Equal(11, db.version);
            Assert.Equal(11, db2.version);
            db2.version = 12;
            Assert.Equal(12, db.version);
            Assert.Equal(12, db2.version);

            /* Reset */
            db.version = 0;
            Assert.Equal(0, db.version);

            /* Invalid case, errors are thrown. */
            TestInvalid(db);
        }

        [Fact]
        public void TestSortedKeys()
        {
            /* Prep data random */
            Random random = new Random();
            int length = random.Next(5, 100);
            int current;
            int[] versionList = new int[length];
            SortedList<int, string> list = new SortedList<int, string>();
            for (int i = 0; i < length; i++)
            {
                current = random.Next(1, 100);
                versionList[i] = current;
                if (!list.ContainsKey(current))
                    list[current] = "NONAME";
            }

            /* Finally assert list iterates in the sorted order. */
            current = 0;
            foreach (KeyValuePair<int, string> kvp in list)
            {
                Assert.True(kvp.Key > current);
                Assert.Equal("NONAME", kvp.Value);
                current = kvp.Key;
            }

            _output.WriteLine("SortedList iterates successfully in sorted order for {0} integers, and a total of {1} inserted values.", length, list.Count);
        }

        private void TestValid(IDatabase db)
        {
            SqlMigrator migrator = new SqlMigrator(db);
            migrator.Migrate(_MigrationDir);
            /* Highest migration version = 2. */
            Assert.Equal(2, db.version);
            db.version = 0;
            Assert.Equal(0, db.version);
        }

        private void TestInvalid(IDatabase db)
        {
            SqlMigrator migrator = new SqlMigrator(db);

            /* FAIL_DUPLICATE migration file is a version duplicate. */
            Create(_FailDuplicate);
            Assert.Throws<ArgumentException>(() => { migrator.Migrate(_MigrationDir); });
            File.Delete(_FailDuplicate);

            /* FAIL_NOVERSION migration file has an invalid name, with no version. */
            Create(_FailNoversion);
            Assert.Throws<FormatException>(() => { migrator.Migrate(_MigrationDir); });
            File.Delete(_FailNoversion);
        }

        private void PrepFiles()
        {
            if (!Directory.Exists(_MigrationDir))
                Directory.CreateDirectory(_MigrationDir);
            Create(_First);
            Create(_Second);
            Create(_IgnoreZero);
            Create(_IgnoreText);

            /* Clean the runs with errors. */
            if (File.Exists(_FailDuplicate))
                File.Delete(_FailDuplicate);
            if (File.Exists(_FailNoversion))
                File.Delete(_FailNoversion);
        }

        private void Create(string fileName)
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
        }
    }
}
