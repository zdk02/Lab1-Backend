using System.Collections.Generic;
using StudentApi.Models;

namespace StudentApi.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll();
        Student? GetById(int id);
        IEnumerable<Student> Search(string name);
        bool Delete(int id);
        Student? Update(UpdateStudentDto dto);
    }
}