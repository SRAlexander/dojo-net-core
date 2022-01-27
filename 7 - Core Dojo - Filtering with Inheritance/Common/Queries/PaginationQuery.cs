using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Queries
{
    public class PaginationQuery : IPaginationQuery
    {
        public int PageNumber { get; set; }
        public int DisplayedPerPage { get; set; }
        public string SortBy { get; set; }
        public bool SortByDecending { get; set; }
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
    }
}
