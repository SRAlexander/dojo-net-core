using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Queries
{
    public interface IPaginationQuery
    {
        int PageNumber { get; set; }
        int DisplayedPerPage { get; set; }
        string SortBy { get; set; }
        bool SortByDecending { get; set; }
        int TotalRecords { get; set; }
        int NumberOfPages { get; set; }
    }
}
