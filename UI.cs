using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_4
{
    internal class UI
    {
        internal static void start()
        {
            bool running = true;
            while (running)
            {
                int id = 0;
                Console.Clear();
                Console.WriteLine("SCHOOL MANAGEMENT SYSTEM");
                Console.WriteLine("1.  Teachers per Department");
                Console.WriteLine("2.  Student info");
                Console.WriteLine("3.  List Active Courses");
                Console.WriteLine("4.  Staff Overview");
                Console.WriteLine("5.  Add New Staff Member");
                Console.WriteLine("6.  Student Grades Overview");
                Console.WriteLine("7.  Monthly Salary per Department");
                Console.WriteLine("8.  Average Salary per Department");
                Console.WriteLine("9.  Get Student Info by ID");
                Console.WriteLine("10. Set Grade with Transaction");
                Console.WriteLine("0.  Exit");
                Console.Write("\nSelect an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EFManager.DepartmentCount();
                        break;
                    case "2":
                        EFManager.StudentInfo();
                        Console.WriteLine("\nPress AnyKey to continue");
                        Console.ReadKey();
                        break;
                    case "3":
                        EFManager.ActiveCourses();
                        break;
                    case "4":
                        AdoManager.PrintTeachers();
                        break;
                    case "5":
                        AdoManager.SaveStaff();
                        break;
                    case "6":
                        EFManager.StudentInfo();
                        id = GetId("\nEnter Student ID:");
                        AdoManager.GetStudentGrades(id);
                        break;
                    case "7":
                        AdoManager.GetSalary();
                        break;
                    case "8":
                        AdoManager.AvgSalary();
                        break;
                    case "9":
                        EFManager.StudentInfo();
                        id = GetId("\nEnter Student ID:");
                        AdoManager.StudentDetails(id);
                        break;
                    case "10":
                        SetGradeMenu();
                        break;
                    case "0":
                        return;
                }
            }
        }


        internal static int GetId(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    return id;
                }

                Console.WriteLine("Invalid input. Please enter a numeric ID.");
            }
        }

        internal static void SetGradeMenu()
        {
            Console.Clear();
            Console.WriteLine("----- Assign New Grade -----");

            EFManager.StudentInfo();

            int sid = GetId("\nEnter Student ID: ");
            int tid = GetId("Enter Teacher ID (Your ID): ");
            int sub = GetId("Enter Subject ID: ");

            Console.Write("Enter Grade (A-F): ");
            string grade = Console.ReadLine().ToUpper();

            AdoManager.SetGrade(sid, tid, sub, grade);
        }


    }
}
