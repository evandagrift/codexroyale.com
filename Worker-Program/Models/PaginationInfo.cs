using System;
using System.Collections.Generic;
using System.Text;

namespace ClashFeeder.Models
{
    public class PaginationInfo
    {
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginationInfo(int pageIndex, int itemsPerPage, int totalPages)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "PageIndex must be greater than 0.");
            }

            if (totalPages < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(totalPages), "TotalPages must be greater than 0.");
            }

            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }
}
