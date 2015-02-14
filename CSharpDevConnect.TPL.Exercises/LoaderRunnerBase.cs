using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;

using Newtonsoft.Json;

using Xunit;

namespace CSharpDevConnect.TPL.Exercises
{
    public abstract class LoaderRunnerBase
    {
        private readonly string _filePath;

        protected LoaderRunnerBase()
        {
            string loadUserDbPath = string.Format("{0}\\CSharpDevConnect\\Exercises\\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            if (!Directory.Exists(loadUserDbPath))
            {
                Directory.CreateDirectory(loadUserDbPath);
            }

            _filePath = loadUserDbPath + "Loader.db";
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        protected static IEnumerable<Enrollment> GetEnrollmentsFromJson()
        {
            string enrollmentsJson = ReadJsonFileIntoString("enrollments.json");
            IEnumerable<Enrollment> enrollments = JsonConvert.DeserializeObject<IEnumerable<Enrollment>>(enrollmentsJson).OrderBy(e => e.User.UserName).ThenBy(e => e.Course.CourseName);
            return enrollments;
        }

        protected static IEnumerable<User> GetOrderedUsersFromJson()
        {
            IEnumerable<User> users = GetEnrollmentsFromJson().Select(e => e.User).OrderBy(u => u.UserName);
            return users;
        }

        protected void AssertEnrollmentLoad(IEnumerable<Enrollment> enrollments, SQLiteDataStore dataStore)
        {
            Enrollment[] storedEnrollments = dataStore.EnrollmentRepository.GetEnrollments().OrderBy(e => e.User.UserName).ThenBy(e => e.Course.CourseName).ToArray();

            int currentIndex = 0;
            foreach (Enrollment originalEnrollment in enrollments)
            {
                Enrollment storedEnrollment = null;
                Assert.DoesNotThrow(() => storedEnrollment = storedEnrollments[currentIndex]);

                Assert.Equal(originalEnrollment.User.UserId, storedEnrollment.User.UserId);
                Assert.Equal(originalEnrollment.Course.CourseId, storedEnrollment.Course.CourseId);

                currentIndex++;
            }

            Assert.Equal(currentIndex, storedEnrollments.Length);

            Console.WriteLine();
            Console.WriteLine("Wrote out {0} enrollments.", currentIndex);
        }

        protected void AssertUserLoad(IEnumerable<User> users, SQLiteDataStore dataStore)
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

        protected void AssertCourseLoad(IEnumerable<Course> courses, SQLiteDataStore sqLiteDataStore)
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