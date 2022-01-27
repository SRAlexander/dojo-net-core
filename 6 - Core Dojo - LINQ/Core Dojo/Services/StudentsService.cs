using Common.RequestModels;
using Common.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Dojo.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentsService(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task<bool> AddStudent(StudentAddRequest request)
        {
            return await _studentsRepository.AddStudent(request);
        }

        public async Task<bool> DeleteStudent(int id)
        {
            await _studentsRepository.DeleteStudent(id);
            return true;
        }

        public async Task<StudentResponse> GetStudent(int id)
        {
            return await _studentsRepository.GetStudent(id);
        }

        public async Task<List<StudentResponse>> GetStudents()
        {
            return await _studentsRepository.GetStudents();
        }
    }
}
