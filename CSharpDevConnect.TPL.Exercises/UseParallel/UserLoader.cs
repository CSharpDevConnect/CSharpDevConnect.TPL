using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpDevConnect.TPL.Exercises.UseParallel
{
    internal sealed class UserLoader : ILoader<User>
    {
        private readonly SQLiteDataStore _dataStore;

        public UserLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public ParallelLoopResult Load(IEnumerable<User> enrollments)
        {
            // Use _dataStore.UserRepository.SaveUser() to store users in the database.

            throw new NotImplementedException("You must implement UserLoader.Load() as part of this exercise.");
        }
    }
}