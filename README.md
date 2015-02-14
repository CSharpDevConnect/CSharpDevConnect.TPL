# CSharpDevConnect.TPL
Sample code and presentation slides for the TPL session given February 17, 2015.

Exercises
---------
  The [CSharpDevConnect.TPL.Exercises](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/tree/master/CSharpDevConnect.TPL.Exercises) project contains exercises to try.  All of the exercises involve loading data from a
  provided JSON file into the provided SQLite database.

**Exercise One:  Loading JSON records into a database using Parallel.ForEach()**

  The entry point for you work is via xUnit test methods in 
  [CSharpDevConnect.TPL.Exercises.UseParallel.ParallelLoaderRunner.cs](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseParallel/ParallelLoaderRunner.cs)

* LoadUsersParallel() - Running this test will call *UserLoader.Load()* to load user information from the 
                             JSON into the database.  The assignment is to make this test pass by providing the 
                             implementation for *UserLoader.Load()*.

* LoadCoursesAndUsersFromEnrollmentsParallel() - Running this test will call *UserAndCourseLoader.Load()*
                            to load user and course information from the JSON into the database.  The assignment 
                            is to make this test pass by providing the implementation for 
                            *UserAndCourseLoader.Load()*.


**Exercise Two:  Loading JSON records into a database using Task**

  The entry point for you work is via xUnit test methods in 
  [CSharpDevConnect.TPL.Exercises.UseTask.TaskLoaderRunner.cs](https://github.com/CSharpDevConnect/CSharpDevConnect.TPL/blob/master/CSharpDevConnect.TPL.Exercises/UseTask/EnrollmentLoader.cs)

  * LoadEnrollments() - Running this test will call *EnrollmentLoader.Load()* to load user information from the 
                             JSON into the database.  The assignment is to make this test pass by providing the 
                             implementation for *UserLoader.Load()*.
