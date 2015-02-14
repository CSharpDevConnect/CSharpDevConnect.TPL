# CSharpDevConnect.TPL
Sample code and presentation slides for the TPL session given February 17, 2015.

Exercises
---------
  The [CSharpDevConnect.TPL.Exercises](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/tree/master/CSharpDevConnect.TPL.Exercises) project contains exercises to try.  All of the exercises involve loading data from a
  provided JSON file into the provided SQLite database (by default the code is hardcoded to write this file to $DESKTOP/CSharpDevConnect/Exercises - this can be changed in the constructor of [SQLiteDataStore](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/Repository/SQLiteDataStore.cs)).

**Exercise One:  Loading JSON records into a database using Parallel.ForEach()**

  The entry point for you work is via xUnit test methods in 
  [CSharpDevConnect.TPL.Exercises.UseParallel.ParallelLoaderRunner.cs](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseParallel/ParallelLoaderRunner.cs)

* LoadUsersParallel() - Running this test will call [UserLoader](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseParallel/UserLoader.cs).Load() to load user information from the 
                             JSON into the database.  The assignment is to make this test pass by providing the 
                             implementation for [UserLoader](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseParallel/UserLoader.cs).Load() which simply loads all of the user records in [enrollments.json](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/data/enrollments.json) into the database.

* LoadCoursesAndUsersFromEnrollmentsParallel() - Running this test will call [UserAndCourseLoader](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseParallel/UserAndCourseLoader.cs).Load()
                            to load user and course information from the JSON into the database.  The assignment 
                            is to make this test pass by providing the implementation for 
                            [UserAndCourseLoader](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseParallel/UserAndCourseLoader.cs).Load() to load all of the User and Course records from [enrollments.json](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/data/enrollments.json) into the database..


**Exercise Two:  Loading JSON records into a database using Task**

  The entry point for you work is via xUnit test methods in 
  [CSharpDevConnect.TPL.Exercises.UseTask.TaskLoaderRunner.cs](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseTask/EnrollmentLoader.cs)

  * LoadEnrollments() - Running this test will call [EnrollmentLoader](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseTask/EnrollmentLoader.cs).Load() to load user information from the 
                             JSON into the database.  The assignment is to make this test pass by providing the 
                             implementation for [EnrollmentLoader](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseTask/EnrollmentLoader.cs).Load() to load all of the Enrollment records from
                             [enrollments.json](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/data/enrollments.json) into the database.  **Note:** in this case the Enrollments are dependent on the User and Course records being loaded first so tasks need to be scheduled accordingly.
