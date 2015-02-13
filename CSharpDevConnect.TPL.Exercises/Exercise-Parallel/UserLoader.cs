
namespace CSharpDevConnect.TPL.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal sealed class UserLoader : IUserLoader
    {
        private readonly SQLiteDataStore _dataStore;

        public UserLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public ParallelLoopResult Load(IEnumerable<User> users)
        {
            // Load the JSON file here
            // Use _dataStore.AddUser() to store users in the database.

            throw new NotImplementedException("You must implement UserLoader.Load() as part of this exercise.");
        }
    }
}