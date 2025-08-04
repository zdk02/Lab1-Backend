using StudentApi.Models;
using StudentApi.Data;

namespace StudentApi.Services
{
    public class StudentService : IStudentService
    {
        public IEnumerable<Student> GetAll()
        {
            return StudentRepository.Students;
        }

        public Student? GetById(int id)
        {
            return StudentRepository.Students.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Student> Search(string name)
        {
            return StudentRepository.Students
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public bool Delete(int id)
        {
            return StudentRepository.DeleteStudent(id);
        }

        public Student? Update(UpdateStudentDto dto)
        {
            var student = StudentRepository.Students.FirstOrDefault(s => s.Id == dto.Id);
            if (student != null)
            {
                student.Name = dto.Name;
                student.Email = dto.Email;
            }
            return student;
        }
    }
}