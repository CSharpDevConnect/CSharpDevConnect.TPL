using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;


using Xunit;

namespace CSharpDevConnect.TPL.Exercises.UseParallel
{
    public class ParallelLoaderRunner : LoaderRunnerBase
    {
        [Fact]
        public void LoadCoursesAndUsersFromEnrollmentsParallel()
        {
            using (SQLiteDataStore sqLiteDataStore = new SQLiteDataStore(FilePath))
            {
                sqLiteDataStore.InitializeDb();
                IEnumerable<Enrollment> enrollments = GetEnrollmentsFromJson();

                ILoader<Enrollment> loader = new UserAndCourseLoader(sqLiteDataStore);
                ParallelLoopResult parallelLoopResult = loader.Load(enrollments);

                Assert.True(parallelLoopResult.IsCompleted);

                IEnumerable<Course> coursesByName = enrollments.GroupBy(e => e.Course.CourseId).Select(e => e.First().Course).OrderBy(c => c.CourseName);
                AssertCourseLoad(coursesByName, sqLiteDataStore);

                IEnumerable<User> expectedUsers = GetOrderedUsersFromJson();
                AssertUserLoad(expectedUsers, sqLiteDataStore);
            }
        }

        [Fact]
        public void LoadUsersParallel()
        {
            using (SQLiteDataStore sqLiteDataStore = new SQLiteDataStore(FilePath))
            {
                sqLiteDataStore.InitializeDb();
                IEnumerable<User> users = GetOrderedUsersFromJson();

                ILoader<User> loader = new UserLoader(sqLiteDataStore);
                ParallelLoopResult parallelLoopResult = loader.Load(users);

                Assert.True(parallelLoopResult.IsCompleted);

                AssertUserLoad(users, sqLiteDataStore);
            }
        }
    }
}