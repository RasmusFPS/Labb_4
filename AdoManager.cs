using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Labb_4
{
    internal class AdoManager
    {
        private static readonly string _connectionString = "Server=localhost;Database=Labb_4;Integrated Security=true;TrustServerCertificate=true;";

        internal static void PrintTeachers()
        {
            Console.Clear();
            Console.WriteLine("---- Staff Overview ----");
            string query = @"SELECT 
                            s.FirstName + ' ' + s.LastName AS Name, 
                            r.RoleName AS Position, 
                            DATEDIFF(YEAR, s.HireDate, GETDATE()) AS YearsWorked
                            FROM Staff s 
                            JOIN Roles r ON s.RoleID = r.RoleID";

            ExecuteQuery(query);
        }


        internal static void NewStaff(string FName, string LName, int rId, int dId, decimal Salary)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Staff (FirstName, LastName, RoleID, DepartmentID, Salary, HireDate)
                                 VALUES (@FirstName, @LastName, @RoleID, @DepartmentID, @Salary, GETDATE())";

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
                        command.ExecuteNonQuery();
                        Console.WriteLine("\nStaff successfully added.");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            Console.WriteLine("Press AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void GetStudentGrades(int studentId)
        {
            Console.Clear();
            Console.WriteLine($"---- Grades for Student ID {studentId} ----");

            string query = @"SELECT 
                            sub.SubjectName AS Subject,
                            g.Grade,
                            g.GradeDate,
                            staff.FirstName + ' ' + staff.LastName AS Teacher
                            FROM Grades g
                            JOIN Students st ON g.StudentID = st.StudentID
                            JOIN Subjects sub ON g.SubjectID = sub.SubjectID
                            JOIN Staff staff ON g.TeacherID = staff.StaffID
                            WHERE g.StudentID = @studentId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No grades found.");
                            }
                      
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetName(i) + "\t");
                            }
                            Console.WriteLine();

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write(reader.GetValue(i) + "\t");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            Console.WriteLine("\nPress AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void GetSalary()
        {
            Console.Clear();
            string query = @"SELECT d.DepartmentName, SUM(s.Salary) AS TotalSalary
                            FROM Staff s JOIN Departments d ON s.DepartmentID = d.DepartmentID
                            GROUP BY d.DepartmentName";
            ExecuteQuery(query);
        }

        internal static void AvgSalary()
        {
            Console.Clear();
            string query = @"SELECT d.DepartmentName, AVG(s.Salary) AS AverageSalary
                            FROM Staff s JOIN Departments d ON s.DepartmentID = d.DepartmentID
                            GROUP BY d.DepartmentName";
            ExecuteQuery(query);
        }

        internal static void StudentDetails(int studentid)
        {
            Console.Clear();
            using (var connection = new SqlConnection(_connectionString))
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
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Student not found.");
                            }

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                    Console.WriteLine($"{reader.GetName(i)}: {reader.GetValue(i)}");
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            Console.WriteLine("\nPress AnyKey to Continue");
            Console.ReadKey();
        }

        internal static void SetGrade(int sid, int tid, int sub, string grade)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    string query = @"INSERT INTO Grades (StudentID, TeacherID, SubjectID, Grade, GradeDate) 
                                     VALUES (@sid, @tid, @sub, @grade, GETDATE())";

                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@sid", sid);
                        command.Parameters.AddWithValue("@tid", tid);
                        command.Parameters.AddWithValue("@sub", sub);
                        command.Parameters.AddWithValue("@grade", grade);

                        try
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            Console.WriteLine("\nsuccess.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine("\nTransaction Failed: " + ex.Message);
                        }
                    }
                }
            }
            Console.WriteLine("Press AnyKey to Continue");
            Console.ReadKey();
        }

        public static void ExecuteQuery(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i),-20}");
                        }
                        Console.WriteLine();


                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetValue(i),-20}");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            Console.WriteLine("\nPress AnyKey to Continue");
            Console.ReadKey();
        }
    }
}