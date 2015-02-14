
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;

namespace CSharpDevConnect.TPL.Exercises.UseParallel.ExampleSolution
{
    internal sealed class AnswerUserAndCourseLoader : ILoader<Enrollment>
    {
        private readonly SQLiteDataStore _dataStore;
        private readonly ConcurrentDictionary<Guid, Course> _savedCourses = new ConcurrentDictionary<Guid, Course>();

        public AnswerUserAndCourseLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public ParallelLoopResult Load(IEnumerable<Enrollment> enrollments)
        {
            ParallelLoopResult parallelLoopResult = Parallel.ForEach(enrollments, LoadUsersAndCourses);
            return parallelLoopResult;
        }

        private void LoadUsersAndCourses(Enrollment enrollment)
        {
            _dataStore.UserRepository.SaveUser(enrollment.User);

            // Only save unique courses
            if (_savedCourses.TryAdd(enrollment.Course.CourseId, enrollment.Course))
            {
                _dataStore.CourseRepository.SaveCourse(enrollment.Course);
            }
        }
    }
}