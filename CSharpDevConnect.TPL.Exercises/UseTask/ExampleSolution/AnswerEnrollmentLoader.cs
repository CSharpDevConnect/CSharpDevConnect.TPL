using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;

namespace CSharpDevConnect.TPL.Exercises.UseTask.ExampleSolution
{
    internal class AnswerEnrollmentLoader : ITaskLoader<Enrollment>
    {
        private readonly SQLiteDataStore _dataStore;
        private readonly ConcurrentDictionary<Guid, Course> _savedCourses = new ConcurrentDictionary<Guid, Course>();

        public AnswerEnrollmentLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Task[] Load(IEnumerable<Enrollment> enrollments)
        {
            CancellationToken cancellationToken = new CancellationToken();

            IEnumerable<User> users = enrollments.Select(e => e.User);
            Task userTask = new Task(() => LoadUsers(users, cancellationToken), cancellationToken);

            IEnumerable<Course> courses = enrollments.Select(e => e.Course).Distinct();
            Task courseTask = new Task(() => LoadCourses(courses, cancellationToken), cancellationToken);

            // Schedule the enrollmentsTask to run after the user and course tasks
            Task[] tasksToWaitFor = new[] { userTask, courseTask };
            Task enrollmentsTask = Task.Factory.ContinueWhenAll(tasksToWaitFor, (tasks) => LoadEnrollments(enrollments, tasks, cancellationToken), cancellationToken);

            userTask.Start();
            courseTask.Start();

            enrollmentsTask.Wait(cancellationToken);

            return new[] { userTask, courseTask, enrollmentsTask };
        }

        private void LoadCourses(IEnumerable<Course> courses, CancellationToken cancellationToken)
        {
            ParallelOptions options = new ParallelOptions { CancellationToken = cancellationToken };

            ParallelLoopResult parallelLoopResult = Parallel.ForEach(courses, (course, loopState) =>
                {
                    // Check for cancellation before doing any work
                    options.CancellationToken.ThrowIfCancellationRequested();

                    // Only save unique courses
                    if (_savedCourses.TryAdd(course.CourseId, course))
                    {
                        options.CancellationToken.ThrowIfCancellationRequested();
                        _dataStore.CourseRepository.SaveCourse(course);
                    }
                });

            if (!parallelLoopResult.IsCompleted)
            {
                throw new Exception("Course load failed to finish.");
            }
        }

        private void LoadUsers(IEnumerable<User> users, CancellationToken cancellationToken)
        {
            ParallelOptions options = new ParallelOptions { CancellationToken = cancellationToken };
            ParallelLoopResult parallelLoopResult = Parallel.ForEach(users, (user) =>
                {
                    // Check for cancellation before doing any work
                    options.CancellationToken.ThrowIfCancellationRequested();

                    _dataStore.UserRepository.SaveUser(user);
                });
            if (!parallelLoopResult.IsCompleted)
            {
                throw new Exception("User load failed to finish.");
            }
        }

        private Task[] LoadEnrollments(IEnumerable<Enrollment> enrollments, Task[] tasks, CancellationToken cancellationToken)
        {
            ParallelOptions options = new ParallelOptions { CancellationToken = cancellationToken };

            // Only run the enrollments if all of the preceding tasks succeeded
            if (tasks.All(t => t.IsCompleted && !t.IsFaulted))
            {
                ParallelLoopResult parallelLoopResult = Parallel.ForEach(enrollments, (enrollment) =>
                    {
                        // Check for cancellation before doing any work
                        options.CancellationToken.ThrowIfCancellationRequested();
                        _dataStore.EnrollmentRepository.SaveEnrollment(enrollment);
                    });

                if (!parallelLoopResult.IsCompleted)
                {
                    throw new Exception("Enrollment load failed to finish.");
                }
            }
            else
            {
                // Error handling: in this case, we're just dumping to the console
                foreach (Task task in tasks)
                {
                    if (task.IsFaulted && task.Exception != null)
                    {
                        Console.WriteLine(task.Exception);
                    }
                }
            }

            return tasks;
        }
    }
}