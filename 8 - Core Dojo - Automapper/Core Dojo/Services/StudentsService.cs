using AutoMapper;
using Common.RequestModels;
using Common.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Repositories;

namespace Core_Dojo.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly IMapper _mapper;

        public StudentsService(IStudentsRepository studentsRepository, IMapper mapper)
        {
            _studentsRepository = studentsRepository;
            _mapper = mapper;
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

        public async Task<SimpleStudentResponse> GetSimpleStudent(int id)
        {
           var student = await GetStudent(id);
           var simpleStudent = _mapper.Map<SimpleStudentResponse>(student);

           return simpleStudent;
        }

        public async Task<List<StudentResponse>> GetStudents()
        {
            return await _studentsRepository.GetStudents();
        }
    }
}
