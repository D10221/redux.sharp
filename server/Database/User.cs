namespace server.Database
{
    class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }

        public class Scripts
        {
            public const string Create = @"
                CREATE TABLE IF NOT EXISTS User (
                ID int PRIMARY KEY,
                Name TEXT NOT null,
                Password TEXT NOT Null,
                Roles TEXT Not Null
            )
            ";
            public const string Insert = @"
                INSERT INTO USER (
                    Name, Password, Roles
                ) VALUES (
                    @Name, @Password, @Roles
                )
                        ";
            public const string All = @"select * from User";
            public const string Find = @"select * from User where id = @ID";
            public const string Drop = "DROP TABLE IF EXISTS User";
            public const string Update = @"
                    UPDATE User 
                        SET Name = @Name,
                            Password = @Password,
                            Roles = @Roles
                    where id = @id
                ";
        }
    }
}