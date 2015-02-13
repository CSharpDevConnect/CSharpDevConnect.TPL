namespace CSharpDevConnect.TPL.Exercises
{
    using System.Collections.Generic;
    using System.Data.SQLite;

    public class UserRepository
    {
        private readonly SQLiteConnection _sqlConnection;

        public UserRepository(SQLiteConnection sqLiteConnection)
        {
            _sqlConnection = sqLiteConnection;
        }

        public IEnumerable<User> GetUsers()
        {
            const string QUERY = "SELECT user_info_id, user_name, first_name, last_name FROM user_info";

            SQLiteCommand command = new SQLiteCommand(QUERY, _sqlConnection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return new User
                                     {
                                         UserId = reader.GetGuid(reader.GetOrdinal("user_info_id")),
                                         UserName = reader.GetString(reader.GetOrdinal("user_name")),
                                         FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                         LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                     };
                }
            }
        }

        public void SaveUser(User user)
        {
            const string QUERY = "INSERT INTO user_info (user_info_id, user_name, first_name, last_name) VALUES (@userId, @userName, @firstName, @lastName)";

            SQLiteCommand command = new SQLiteCommand(QUERY, _sqlConnection);
            command.Parameters.Add(CreateParameter(command, "@userId", user.UserId.ToString("N")));
            command.Parameters.Add(CreateParameter(command, "@userName", user.UserName));
            command.Parameters.Add(CreateParameter(command, "@firstName", user.FirstName));
            command.Parameters.Add(CreateParameter(command, "@lastName", user.LastName));

            command.ExecuteNonQuery();
        }

        private static SQLiteParameter CreateParameter(SQLiteCommand command, string parameterName, object value)
        {
            SQLiteParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }
    }
}