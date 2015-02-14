using System;
using System.Collections.Generic;
using System.Data.SQLite;

using CSharpDevConnect.TPL.Exercises.Model;

namespace CSharpDevConnect.TPL.Exercises.Repository
{
    public class UserRepository : RepositoryBase
    {
        public UserRepository(SQLiteConnection sqLiteConnection)
            : base(sqLiteConnection)
        {
        }

        public IEnumerable<User> GetUsers()
        {
            const string QUERY = "SELECT user_info_id, user_name, first_name, last_name FROM user_info";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return ReadUser(reader);
                }
            }
        }

        public void SaveUser(User user)
        {
            const string QUERY = "INSERT INTO user_info (user_info_id, user_name, first_name, last_name) VALUES (@userId, @userName, @firstName, @lastName)";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);
            command.Parameters.Add(CreateParameter(command, "@userId", user.UserId.ToString("N")));
            command.Parameters.Add(CreateParameter(command, "@userName", user.UserName));
            command.Parameters.Add(CreateParameter(command, "@firstName", user.FirstName));
            command.Parameters.Add(CreateParameter(command, "@lastName", user.LastName));

            command.ExecuteNonQuery();
        }

        public User GetUser(Guid userId)
        {
            const string QUERY = "SELECT user_info_id, user_name, first_name, last_name FROM user_info WHERE user_info_id = @userId";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);
            command.Parameters.Add(CreateParameter(command, "@userId", userId.ToString("N")));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return ReadUser(reader);
                }
            }

            return null;
        }

        private static User ReadUser(SQLiteDataReader reader)
        {
            return new User
            {
                UserId = reader.GetGuid(reader.GetOrdinal("user_info_id")),
                UserName = reader.GetString(reader.GetOrdinal("user_name")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
            };
        }

    }
}