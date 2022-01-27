using System;

namespace Common.RequestModels
{
    public class StudentAddRequest
    {
        public string FirstName {get;set;}
        public string MiddleNames { get; set; }
        public string Surname{ get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
