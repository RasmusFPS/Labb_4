using Labb_4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_4
{
    internal class EFManager
    {
        internal static void DepartmentCount()
        {
            using (var context = new Labb4Context())
            {
                var count = context.Departments.Select(d => new {
                Name = d.DepartmentName,
                Count = d.Staff.Count(s => s.Role.RoleName == "Teacher")
                }).ToList();
                  
                foreach (var d in count)
                {
                    if(d.Count > 1)
                    {
                        Console.WriteLine($"Department {d.Name} || {d.Count} Teachers");
                    }
                    else
                    {
                        Console.WriteLine($"Department {d.Name} || {d.Count} Teacher");
                    }
                }

                Console.WriteLine("\nPress AnyKey to continue");
                Console.ReadKey();
            }
        }

        internal static void StudentInfo()
        {
            using (var context = new Labb4Context())
            {
                var info = context.Students.Select(s => new
                {
                    sID = s.StudentId,
                    FName = s.FirstName,
                    Lname = s.LastName,
                    Class = s.Class.ClassName,

                }).ToList();

                foreach(var i in info)
                {
                    Console.WriteLine($"ID {i.sID} || {i.FName} {i.Lname} {i.Class}");
                }

            }
        }

        internal static void ActiveCourses()
        {
            using (var context = new Labb4Context())
            {
                Console.Clear();
                var activeCourses = context.Subjects.Where(s => s.IsActive == true).ToList();

                Console.WriteLine("--Active Courses--");
                foreach (var c in activeCourses)
                {
                    Console.WriteLine($"{c.SubjectName}");
                }

                Console.WriteLine("\nPress AnyKey to continue");
                Console.ReadKey();
            }
        }
    }
}
