using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;

using Xunit;

namespace CSharpDevConnect.TPL.Exercises.UseTask
{
    public class TaskLoaderRunner : LoaderRunnerBase
    {
        [Fact]
        public void LoadEnrollments()
        {
            using (SQLiteDataStore sqLiteDataStore = new SQLiteDataStore(FilePath))
            {
                sqLiteDataStore.InitializeDb();
                IEnumerable<Enrollment> enrollments = GetEnrollmentsFromJson();

                ITaskLoader<Enrollment> taskLoader = new EnrollmentLoader(sqLiteDataStore);
                Task[] tasks = taskLoader.Load(enrollments);

                Assert.NotEmpty(tasks);
                Assert.True(tasks.Length > 1);

                foreach (Task task in tasks)
                {
                    Assert.True(task.IsCompleted);
                    Assert.Null(task.Exception);
                }

                IEnumerable<Course> coursesByName = enrollments.GroupBy(e => e.Course.CourseId).Select(e => e.First().Course).OrderBy(c => c.CourseName);
                AssertCourseLoad(coursesByName, sqLiteDataStore);

                IEnumerable<User> expectedUsers = GetOrderedUsersFromJson();
                AssertUserLoad(expectedUsers, sqLiteDataStore);

                AssertEnrollmentLoad(enrollments, sqLiteDataStore);
            }
        }
    }
}