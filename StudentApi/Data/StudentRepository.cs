using System.Collections.Generic;
using System.Linq;
using StudentApi.Models;

namespace StudentApi.Data
{
    public class StudentRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>
        {
            new Student { Id = 1, Name = "Zeinab Brown", Email = "zeinab@example.com" },
            new Student { Id = 2, Name = "Dina Smith", Email = "dina@example.com" },
            new Student { Id = 3, Name = "Carlos Brown", Email = "carlos@example.com" },
            new Student { Id = 4, Name = "Jawad Lee", Email = "jawad@example.com" },
            new Student { Id = 5, Name = "Ethan Matar", Email = "ethan@example.com" }

        };
        public static bool DeleteStudent(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return false;

            Students.Remove(student);
            return true;
        }
    }
}