using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpDevConnect.TPL.Exercises.ExampleSolution
{
    internal sealed class AnswerUserLoader : IUserLoader
    {
        private readonly SQLiteDataStore _dataStore;

        public AnswerUserLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public ParallelLoopResult Load(IEnumerable<User> users)
        {
            ParallelLoopResult parallelLoopResult = Parallel.ForEach(users, _dataStore.UserRepository.SaveUser);
            return parallelLoopResult;
        }
    }
}