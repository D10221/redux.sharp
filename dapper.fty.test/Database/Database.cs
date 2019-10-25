using System.Data;
using System.Data.SQLite;

namespace dapper.fty.test
{
    using static System.IO.Path;
    using static System.IO.Directory;
    using System.IO;

    public class Database
    {
        static string DBPath => Combine(Base, "database.db");

        static string Base => Combine(GetCurrentDirectory());

        public static void Drop()
        {
            if (File.Exists(DBPath))
            {
                File.Delete(DBPath);
            }
        }

        public static IDbConnection Connect()
        {
            return new SQLiteConnection("Data Source=" + DBPath);
        }
    }
}