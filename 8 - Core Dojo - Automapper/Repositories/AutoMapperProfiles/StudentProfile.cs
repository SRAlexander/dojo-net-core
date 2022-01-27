using AutoMapper;
using Common.ResponseModels;
using Repositories.Entities;

namespace Repositories.AutoMapperProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentResponse>();
            CreateMap<StudentResponse, SimpleStudentResponse>().ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.Surname}"));
        }
    }
}
