using System.Collections.Generic;
using System.Data.SQLite;

namespace CSharpDevConnect.TPL.Exercises
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
                    yield return new Course
                                     {
                                         CourseId = reader.GetGuid(reader.GetOrdinal("course_info_id")),
                                         CourseName = reader.GetString(reader.GetOrdinal("course_name"))
                                     };
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

    }
}