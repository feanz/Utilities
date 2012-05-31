using System;

namespace Utilities._Samples
{
    /// <summary>
    ///   Example for the ToDo class. This example can be dropped into a console application to see the Validation functionality.
    /// </summary>
    public class Example_ToDo
    {
        private static void Main(string[] args)
        {
            // The ToDo logs all the information
            // 1. priority
            // 2. author
            // 3. description
            // 4. STACK TRACE to the log.
            // 
            // NOTE: This is very useful alternative to using comments in the code.
            //       Plus this logs the stack trace(Class and Methodname) where the ToDo.MethodXXX was called from 
            //       so you know from the logs exactly where some work needs done.

            // Example 1: Log code review.
            ToDo.CodeReview(ToDo.Priority.Normal, "John Smith", "Get john to review this code",
                            () => { Console.WriteLine("My code needs review"); });

            // Example 2: Log some thing that needs to be implemented.
            ToDo.Implement(ToDo.Priority.High, "John Smith",
                           "Have to finish up the Repository.Find to use dynamic objects.",
                           () => { Console.WriteLine("DynamicObject find code is incomplete."); });

            // Example 3: Log refactoring needed
            ToDo.Refactor(ToDo.Priority.Low, "John Smith", "What a mess in the Reflection utils.",
                          () => { Console.WriteLine("This code is a mess, needs some refactoring."); });

            // Example 4: Log optimzation needed.
            ToDo.Optimize(ToDo.Priority.Critical, "John Smith",
                          "Multi-tenant code needs to queried from database only once.",
                          () => { Console.WriteLine("Too many queries here. Get all data in one call."); });

            // Example 5: Log bug found in code.
            ToDo.BugFix(ToDo.Priority.High, "John Smith", "This is a bug related to configuration data. ",
                        () => { Console.WriteLine("Fix this code"); });
        }
    }
}