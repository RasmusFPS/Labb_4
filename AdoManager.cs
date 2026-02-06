using Labb_4.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
                connection.Close();

                Console.WriteLine("Press AnyKey to Continue");
                Console.ReadKey();
            }
    
        }

        internal static void GetSalary()
        {
            Console.Clear();

            Console.WriteLine("----Salary----");

            string query = "SELECT DepartmentName,SUM(Salary) as Total FROM Staff s JOIN Departments d on s.DepartmentID = d.DepartmentID GROUP BY DepartmentName";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read()) Console.WriteLine($"Department {reader["DepartmentName"]}: {reader["Total"]:c} /month");
                    }

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            Console.Write("\nPress AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void AvgSalary()
        {
            Console.Clear();

            Console.WriteLine("----Salary----");

            string query = "SELECT DepartmentName,AVG(Salary) as Avgsal FROM Staff s JOIN Departments d on s.DepartmentID = d.DepartmentID GROUP BY DepartmentName";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read()) Console.WriteLine($"Department {reader["DepartmentName"]}: {reader["Avgsal"]:c} Avg");
                    }

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            Console.Write("\nPress AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void StudentDetails(int studentid)
        {
            Console.Clear();
            Console.WriteLine("----Student Info----");

            using(var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetStudentDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", studentid);

                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine($"NAME {reader["StudentName"]}");
                                Console.WriteLine($"SSN {reader["SSN"]}");
                                Console.WriteLine($"CLASS {reader["ClassName"]}");
                                Console.WriteLine($"MENTOR {reader["TeacherName"]}");
                            }
                            else
                            {
                                Console.WriteLine("No one with this ID");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            Console.WriteLine("\nPress AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void SetGrade(int studentId, int teacherId, int subjectId, string grade)
        {
            Console.Clear();
            Console.WriteLine("----Set Grade----");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    string query = @"INSERT INTO Grades (StudentID, TeacherID, SubjectID, Grade, GradeDate) 
                             VALUES (@sid, @tid, @sub, @grade, GETDATE())";

                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@sid", studentId);
                        command.Parameters.AddWithValue("@tid", teacherId);
                        command.Parameters.AddWithValue("@sub", subjectId);
                        command.Parameters.AddWithValue("@grade", grade);

                        try
                        {
                            command.ExecuteNonQuery();

                            transaction.Commit();
                            Console.WriteLine("Success");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine("Transaction Failed No data was saved.");
                            Console.WriteLine("Error Message: " + ex.Message);
                        }
                    }
                }
            }

            Console.WriteLine("\nPress AnyKey to Continue");
            Console.ReadKey();
        }
    }

}
