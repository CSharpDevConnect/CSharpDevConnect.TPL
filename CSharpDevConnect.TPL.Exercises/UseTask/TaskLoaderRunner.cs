using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xunit;

namespace CSharpDevConnect.TPL.Exercises.UseTask
{
    public class TaskLoaderRunner
    {
        private readonly string _filePath;

        public TaskLoaderRunner()
        {
            string loadUserDbPath = string.Format("{0}\\CSharpDevConnect\\Exercises\\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            if (!Directory.Exists(loadUserDbPath))
            {
                Directory.CreateDirectory(loadUserDbPath);
            }

            _filePath = loadUserDbPath + "Loader.db";
        }

        [Fact]
        public void LoadEnrollments()
        {
            using (SQLiteDataStore sqLiteDataStore = new SQLiteDataStore(_filePath))
            {
                sqLiteDataStore.InitializeDb();
                IEnumerable<Enrollment> enrollments = GetEnrollmentsFromJson();

                ITaskLoader<Enrollment> taskLoader = new EnrollmentLoader(sqLiteDataStore);
                Task[] tasks = taskLoader.Load(enrollments);

                Assert.NotEmpty(tasks);
                Assert.True(tasks.Length > 1);

                IEnumerable<Course> coursesByName = enrollments.GroupBy(e => e.Course.CourseId).Select(e => e.First().Course).OrderBy(c => c.CourseName);
                AssertCourseLoad(coursesByName, sqLiteDataStore);

                IEnumerable<User> expectedUsers = GetOrderedUsersFromJson();
                AssertUserLoad(expectedUsers, sqLiteDataStore);
            }
        }

        private static IEnumerable<Enrollment> GetEnrollmentsFromJson()
        {
            string enrollmentsJson = ReadJsonFileIntoString("enrollments.json");
            IEnumerable<Enrollment> enrollments = JsonConvert.DeserializeObject<IEnumerable<Enrollment>>(enrollmentsJson);
            return enrollments;
        }

        private static IEnumerable<User> GetOrderedUsersFromJson()
        {
            string enrollmentsJson = ReadJsonFileIntoString("enrollments.json");
            IEnumerable<User> users = JsonConvert.DeserializeObject<IEnumerable<Enrollment>>(enrollmentsJson).Select(e => e.User).OrderBy(u => u.UserName);
            return users;
        }

        private void AssertUserLoad(IEnumerable<User> users, SQLiteDataStore dataStore)
        {
            User[] storedUsers = dataStore.UserRepository.GetUsers().OrderBy(u => u.UserName).ToArray();

            int currentIndex = 0;
            foreach (User originalUser in users)
            {
                User storedUser = null;
                Assert.DoesNotThrow(() => storedUser = storedUsers[currentIndex]);

                Assert.Equal(originalUser.UserName, storedUser.UserName);
                Assert.Equal(originalUser.UserId, storedUser.UserId);
                Assert.Equal(originalUser.FirstName, storedUser.FirstName);
                Assert.Equal(originalUser.LastName, storedUser.LastName);

                currentIndex++;
            }

            Assert.Equal(currentIndex, storedUsers.Length);

            Console.WriteLine();
            Console.WriteLine("Wrote out {0} users.", currentIndex);
        }

        private void AssertCourseLoad(IEnumerable<Course> courses, SQLiteDataStore sqLiteDataStore)
        {
            Course[] storedCourses = sqLiteDataStore.CourseRepository.GetCourses().OrderBy(c => c.CourseName).ToArray();

            int currentIndex = 0;

            foreach (Course course in courses)
            {
                Course storedCourse = null;
                Assert.DoesNotThrow(() => storedCourse = storedCourses[currentIndex]);

                Assert.Equal(course.CourseId, storedCourse.CourseId);
                Assert.Equal(course.CourseName, storedCourse.CourseName);
                currentIndex++;
            }

            Assert.Equal(currentIndex, storedCourses.Length);

            Console.WriteLine();
            Console.WriteLine("Wrote out {0} courses.", currentIndex);
        }

        private static string ReadJsonFileIntoString(string fileName)
        {
            const string JSON_FOLDER = "CSharpDevConnect.TPL.Exercises.data.";

            string result = null;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullFileName = JSON_FOLDER + fileName;

            using (Stream stream = assembly.GetManifestResourceStream(fullFileName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Unable to open load script stream for '{0}'.", fullFileName));
                }
            }

            return result;
        }
    }
}