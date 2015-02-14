using System;
using System.Collections.Generic;
using System.Data.SQLite;

using CSharpDevConnect.TPL.Exercises.Model;

namespace CSharpDevConnect.TPL.Exercises.Repository
{
    public class CourseRepository : RepositoryBase
    {
        public CourseRepository(SQLiteConnection sqLiteConnection)
            : base(sqLiteConnection)
        {
        }

        public IEnumerable<Course> GetCourses()
        {
            const string QUERY = "SELECT course_info_id, course_name FROM course_info";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return ReadCourse(reader);
                }
            }
        }

        public void SaveCourse(Course course)
        {
            const string QUERY = "INSERT INTO course_info (course_info_id, course_name) VALUES (@courseId, @courseName)";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);
            command.Parameters.Add(CreateParameter(command, "@courseId", course.CourseId.ToString("N")));
            command.Parameters.Add(CreateParameter(command, "@courseName", course.CourseName));

            command.ExecuteNonQuery();
        }

        public Course GetCourse(Guid courseId)
        {
            const string QUERY = "SELECT course_info_id, course_name FROM course_info WHERE course_info_id = @courseId";

            SQLiteCommand command = new SQLiteCommand(QUERY, SqlConnection);
            command.Parameters.Add(CreateParameter(command, "@courseId", courseId.ToString("N")));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return ReadCourse(reader);
                }
            }

            return null;
        }

        private static Course ReadCourse(SQLiteDataReader reader)
        {
            return new Course
                       {
                           CourseId = reader.GetGuid(reader.GetOrdinal("course_info_id")),
                           CourseName = reader.GetString(reader.GetOrdinal("course_name"))
                       };
        }

    }
}