using System;
using System.Collections.Generic;
using System.Data.SQLite;

using CSharpDevConnect.TPL.Exercises.Model;

namespace CSharpDevConnect.TPL.Exercises.Repository
{
    public class EnrollmentRepository : RepositoryBase
    {
        private readonly UserRepository _userRepository;

        private readonly CourseRepository _courseRepository;

        public EnrollmentRepository(SQLiteConnection sqLiteConnection, UserRepository userRepository, CourseRepository courseRepository)
            : base(sqLiteConnection)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
        }

        public IEnumerable<Enrollment> GetEnrollments()
        {
            const string QUERY = "SELECT course_info_id, user_info_id FROM course_users";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Guid courseId = reader.GetGuid(reader.GetOrdinal("course_info_id"));
                    Course course = _courseRepository.GetCourse(courseId);

                    Guid userId = reader.GetGuid(reader.GetOrdinal("user_info_id"));
                    User user = _userRepository.GetUser(userId);

                    yield return new Enrollment
                                     {
                                         Course = course,
                                         User = user
                                     };
                }
            }
        }

        public void SaveEnrollment(Enrollment enrollment)
        {
            const string QUERY = "INSERT INTO course_users (course_info_id, user_info_id) VALUES (@courseId, @userId)";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);
            command.Parameters.Add(CreateParameter(command, "@courseId", enrollment.Course.CourseId.ToString("N")));
            command.Parameters.Add(CreateParameter(command, "@userId", enrollment.User.UserId.ToString("N")));

            command.ExecuteNonQuery();
        }

    }
}