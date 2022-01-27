using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Queries
{
    public class StudentFilterQuery : PaginationQuery, IStudentFilterQuery
    {
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
