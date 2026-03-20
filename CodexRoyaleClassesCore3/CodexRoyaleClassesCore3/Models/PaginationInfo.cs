using System;
using System.Collections.Generic;
using System.Text;

namespace CodexRoyaleClassesCore3.Models
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
            ItemsPerPage = itemsPerPage;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }
}
