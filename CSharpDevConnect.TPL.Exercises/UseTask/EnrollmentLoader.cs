using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;

namespace CSharpDevConnect.TPL.Exercises.UseTask
{
    internal class EnrollmentLoader : ITaskLoader<Enrollment>
    {
        private readonly SQLiteDataStore _dataStore;

        public EnrollmentLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Task[] Load(IEnumerable<Enrollment> enrollments)
        {
            // Use _dataStore.UserRepository.SaveUser() to store users in the database.
            // Use _dataStore.CourseRepository.SaveCourse() to store courses in the database.
            // Use _dataStore.EnrollmentRepository.SaveEnrollment() to store enrollments in the database.

            throw new NotImplementedException("You must implement EnrollmentLoader.Load() as part of this exercise.");
        }
    }
}