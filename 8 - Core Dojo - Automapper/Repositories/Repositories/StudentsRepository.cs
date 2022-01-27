using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.RequestModels;
using Common.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly RepositoryContext _context;
        private readonly IMapper _mapper;

        public StudentsRepository(RepositoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddStudent(StudentAddRequest request)
        {
            var student = new Student
            {
                CreatedDate = DateTime.UtcNow,
                DateOfBirth = request.DateOfBirth,
                FirstName = request.FirstName,
                MiddleNames = request.MiddleNames,
                Surname = request.Surname
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StudentResponse> GetStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(student => student.Id == id);
            if (student == null) { return null; }

            var studentResponse = _mapper.Map<StudentResponse>(student);
            return studentResponse;
        }

        public async Task<List<StudentResponse>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            var studentsResponse = _mapper.Map<List<StudentResponse>>(students);

            return studentsResponse;
        }
    }
}
