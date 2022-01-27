using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.ResponseModels;

namespace Repositories.QueryExtensions
{
    public static class PaginationExtensions { 

        public static PaginationResponse GetPaginationResponse(int numberPerPage, int pageNumber, int totalRecords, bool sortByDecending = true, string sortBy = "")
        {
            var response = new PaginationResponse
            {
                NumberOfPages = ((totalRecords + numberPerPage - 1) / numberPerPage),
                TotalRecords = totalRecords,
                DisplayedPerPage = numberPerPage,
                SortBy = sortBy,
                SortByDecending = sortByDecending
            };

            if (pageNumber == 0) {
                response.PageNumber = 1;
            } 
            else
            {
                if(pageNumber > response.NumberOfPages)
                {
                    response.PageNumber = response.NumberOfPages == 0 ? 1 : response.NumberOfPages;
                } else
                {
                    response.PageNumber = pageNumber;
                }
            }

            return response;
        }

        public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int displayedPerPage, int pageNumber)
        {
            var offset = displayedPerPage * (pageNumber - 1);
            var takeAmount = displayedPerPage;

            query = query.Skip(offset);
            query = query.Take(takeAmount);

            return query;
        }

        public static IList<T> ApplyPagination<T>(IEnumerable<T> query, int displayedPerPage, int pageNumber)
        {
            var offset = displayedPerPage * (pageNumber - 1);
            var takeAmount = displayedPerPage;

            query = query.Skip(offset);
            query = query.Take(takeAmount);

            return query.ToList();
        }

        public static IQueryable<IGrouping<T,S>> ApplyPagination<T,S>(IQueryable<IGrouping<T, S>> query, int displayedPerPage, int pageNumber)
        {
            var offset = displayedPerPage * (pageNumber - 1);
            var takeAmount = displayedPerPage;

            query = query.Skip(offset);
            query = query.Take(takeAmount);

            return query;
        }
    }
}
