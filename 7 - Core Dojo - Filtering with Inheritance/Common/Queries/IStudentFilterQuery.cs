using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Queries
{
    public interface IStudentFilterQuery : IPaginationQuery
    {
        string Surname { get; set; }
        DateTime? DateOfBirth { get; set; }
        DateTime? CreatedFrom { get; set; }
        DateTime? CreatedTo { get; set; }
    }
}
