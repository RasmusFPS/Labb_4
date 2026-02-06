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
                        AdoManager.SaveStaffPrompt();
                        break;
                    case "6":
                        GetId();
                        break;
                    case "7":
                        break;
                    case "8":
                        break;
                    case "9":
                        break;
                    case "10":
                        break;
                    case "0":
                        return;
                }
            }
        }

        internal static void GetId()
        {
            Console.Clear();

            EFManager.StudentInfo();

            Console.Write("\nEnter Id:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                AdoManager.GetStudentGrades(id);
            }
            else
            {
                Console.WriteLine("Invalid Id. Press Anykey to Continue");
                Console.ReadKey();
            }
        }


    }
}
