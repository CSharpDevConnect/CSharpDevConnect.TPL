﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CSharpDevConnect.TPL.Exercises.Model;
using CSharpDevConnect.TPL.Exercises.Repository;

namespace CSharpDevConnect.TPL.Exercises.UseParallel
{
    internal sealed class UserAndCourseLoader : ILoader<Enrollment>
    {
        private readonly SQLiteDataStore _dataStore;

        public UserAndCourseLoader(SQLiteDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public ParallelLoopResult Load(IEnumerable<Enrollment> enrollments)
        {
            // Use _dataStore.UserRepository.SaveUser() to store users in the database.
            // Use _dataStore.CourseRepository.SaveCourse() to store courses in the database.

            throw new NotImplementedException("You must implement UserAndCourseLoader.Load() as part of this exercise.");
        }
    }
}