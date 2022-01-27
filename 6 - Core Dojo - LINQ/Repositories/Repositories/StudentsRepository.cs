using Common.RequestModels;
using Common.ResponseModels;
using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core_Dojo.Services
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly RepositoryContext _context;

        public StudentsRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<bool> AddStudent(StudentAddRequest request)
        {
            var student = new Student();
            student.CreatedDate = DateTime.UtcNow;
            student.DateOfBirth = request.DateOfBirth;
            student.FirstName = request.FirstName;
            student.MiddleNames = request.MiddleNames;
            student.Surname = request.Surname;
            
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
            var student= await _context.Students.FirstOrDefaultAsync(student => student.Id == id);
            if (student == null) { return null; }

            var studentResponse = new StudentResponse();
            studentResponse.Id = student.Id;
            studentResponse.FirstName = student.FirstName;
            studentResponse.MiddleNames = student.MiddleNames;
            studentResponse.Surname = student.Surname;
            studentResponse.DateOfBirth = student.DateOfBirth;
                

            return studentResponse;
        }

        public async Task<List<StudentResponse>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            var studentsResponse = new List<StudentResponse>();

            foreach (var student in students) {
                var studentResponse = new StudentResponse();
                studentResponse.Id = student.Id;
                studentResponse.FirstName = student.FirstName;
                studentResponse.MiddleNames = student.MiddleNames;
                studentResponse.Surname = student.Surname;
                studentResponse.DateOfBirth = student.DateOfBirth;
            }

            return studentsResponse;
        }
    }
}
