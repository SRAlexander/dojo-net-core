using Common.RequestModels;
using Common.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public interface IStudentsRepository
    {
        public Task<StudentResponse> GetStudent(int id);
        public Task<List<StudentResponse>> GetStudents();
        public Task<bool> AddStudent(StudentAddRequest request);
        public Task<bool> DeleteStudent(int id);
    }
}
