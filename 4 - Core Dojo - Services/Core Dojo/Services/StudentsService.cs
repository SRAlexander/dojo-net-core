using Core_Dojo.RequestModels;
using Core_Dojo.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Dojo.Services
{
    public class StudentsService : IStudentsService
    {
        public Task<bool> AddStudent(StudentAddRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public Task<StudentResponse> GetStudent(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentResponse>> GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
