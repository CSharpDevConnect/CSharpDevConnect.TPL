using System;

namespace CSharpDevConnect.TPL.Exercises
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CSharpDevConnect.TPL.Exercises.ExampleSolution;

    using Newtonsoft.Json;

    using Xunit;

    public class UserLoaderRunner
    {
        private readonly SQLiteDataStore _dataStore;

        public UserLoaderRunner()
        {
            string loadUserDbPath = string.Format("{0}\\CSharpDevConnect\\Exercises\\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            if (!Directory.Exists(loadUserDbPath))
            {
                Directory.CreateDirectory(loadUserDbPath);
            }

            _dataStore = new SQLiteDataStore(loadUserDbPath + "UserLoader.db");

            _dataStore.InitializeDb();
        }

        [Fact]
        public void LoadParallel()
        {
            string usersJson = ReadJsonFileIntoString("users.json");
            IEnumerable<User> users = JsonConvert.DeserializeObject<IEnumerable<User>>(usersJson).OrderBy(u => u.UserName);

            IUserLoader loader = new AnswerUserLoader(_dataStore);
            ParallelLoopResult parallelLoopResult = loader.Load(users);

            Assert.True(parallelLoopResult.IsCompleted);

            AssertUserLoad(users);
        }

        [Fact]
        public void LoadSerial()
        {
            string usersJson = ReadJsonFileIntoString("users.json");
            IEnumerable<User> users = JsonConvert.DeserializeObject<IEnumerable<User>>(usersJson).OrderBy(u => u.UserName);

            foreach (User user in users)
            {
                _dataStore.UserRepository.SaveUser(user);
            }

            AssertUserLoad(users);
        }

        private void AssertUserLoad(IEnumerable<User> users)
        {
            User[] storedUsers = _dataStore.UserRepository.GetUsers().OrderBy(u => u.UserName).ToArray();

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