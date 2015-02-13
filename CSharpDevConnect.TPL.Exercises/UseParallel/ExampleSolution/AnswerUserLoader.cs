
namespace CSharpDevConnect.TPL.Exercises.UseParallel.ExampleSolution
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal sealed class AnswerUserLoader : ILoader<User>
    {
        private readonly SQLiteDataStore _dataStore;

        public AnswerUserLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public ParallelLoopResult Load(IEnumerable<User> enrollments)
        {
            ParallelLoopResult parallelLoopResult = Parallel.ForEach(enrollments, _dataStore.UserRepository.SaveUser);
            return parallelLoopResult;
        }
    }
}