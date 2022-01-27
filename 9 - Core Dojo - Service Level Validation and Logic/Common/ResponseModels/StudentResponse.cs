using System;

namespace Common.ResponseModels
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth {get;set;}
    }
}
