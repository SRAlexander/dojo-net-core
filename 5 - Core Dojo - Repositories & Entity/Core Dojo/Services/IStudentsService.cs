using Common.RequestModels;
using Common.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Dojo.Services
{
    public interface IStudentsService
    {
        public Task<StudentResponse> GetStudent(int id);
        public Task<List<StudentResponse>> GetStudents();
        public Task<Boolean> AddStudent(StudentAddRequest request);
        public Task<Boolean> DeleteStudent(int id);
    }
}
