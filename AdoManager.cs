using Labb_4.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Labb_4
{
    internal class AdoManager
    {
        private static readonly string _connectionString = @"Server = localhost;Database = Labb_4;Integrated Security = true;Trust Server Certificate = true;";

        internal static void PrintTeachers()
        {
            Console.Clear();
            string query = @"SELECT s.FirstName, s.LastName, r.RoleName, 
                             DATEDIFF(YEAR, s.HireDate, GETDATE()) as YearsWorked 
                             FROM Staff s JOIN Roles r ON s.RoleID = r.RoleID";

            ExecQuery(query);
        }

        internal static void ExecQuery(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine($"{reader.GetName(i)} {reader.GetValue(i)}");
                            }
                            Console.WriteLine("-------------------------------------");
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            Console.WriteLine("\nPress AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void NewStaff(string FName, string LName, int rId, int dId, Decimal Salary)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Staff(FirstName, LastName, RoleID, DepartmentID, Salary, HireDate) VALUES (@FirstName,@LastName,@RoleID,@DepartmentID,@Salary,GETDATE())";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FName);
                    command.Parameters.AddWithValue("@LastName", LName);
                    command.Parameters.AddWithValue("@RoleID", rId);
                    command.Parameters.AddWithValue("@DepartmentID", dId);
                    command.Parameters.AddWithValue("@Salary", Salary);
                    try
                    {
                        connection.Open();

                        Console.WriteLine($"Changes {command.ExecuteNonQuery()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    connection.Close();

                    Console.WriteLine("Press AnyKey to Continue");
                    Console.ReadKey();
                }
            }

        }

        internal static void SaveStaffPrompt()
        {
            Console.Write("First Name: "); string fName = Console.ReadLine();
            Console.Write("Last Name: "); string lName = Console.ReadLine();
            Console.Write("Role ID (1:Teacher, 2:Principal, 3:Admin): "); int rId = int.Parse(Console.ReadLine());
            Console.Write("Department ID (1-4): "); int dId = int.Parse(Console.ReadLine());
            if (dId > 4 && dId > 0)
            {
                Console.WriteLine("invalid");
                return;

            }
            Console.Write("Salary: "); decimal salary = decimal.Parse(Console.ReadLine());
            if (salary <= 0 && salary >= 100000)
            {
                Console.WriteLine("invalid");
                salary = 0;
            }
            NewStaff(fName, lName, rId, dId, salary);
        }


        internal static void GetStudentGrades(int studentId)
        {
            Console.Clear();
            Console.WriteLine("----Student Grade----");

            string query = @" SELECT st.FirstName + ' ' + st.LastName as StudentName,
                              sub.SubjectName,
                              g.Grade,
                              g.GradeDate,
                              staff.FirstName +' '+ staff.Lastname AS TeacherName
                              FROM Grades g
                              JOIN Students st on g.StudentID = st.StudentID
                              JOIN Subjects sub ON g.SubjectID = sub.SubjectID
                              JOIN Staff staff ON g.TeacherID = staff.StaffID
                              WHERE g.StudentID = @studentId";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("NAME: " + reader["StudentName"]);
                                Console.WriteLine("COURSE: " + reader["SubjectName"]);
                                Console.WriteLine("GRADE: " + reader["Grade"]);
                                Console.WriteLine("TEACHER " + reader["TeacherName"]);
                                Console.WriteLine("GRADE DATE " + reader["GradeDate"]);
                                Console.WriteLine("---------------------------");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); 
                }
                Console.WriteLine("Press AnyKey to Continue");
                Console.ReadKey();
            }
    
        }
    }

}
