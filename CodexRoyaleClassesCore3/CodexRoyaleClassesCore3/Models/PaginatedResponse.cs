using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodexRoyaleClassesCore3.Models
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; }
        public PaginationInfo PaginationInfo { get; }

        public PaginatedResponse(List<T> items, int pageIndex, int itemsPerPage, int totalPages)
        {
            if (pageIndex < 1) { throw new ArgumentOutOfRangeException(nameof(pageIndex), "PageIndex must be greater than 0."); }
            if (pageIndex > totalPages) { throw new ArgumentOutOfRangeException(nameof(pageIndex), "PageIndex must be no greater than the total number of pages."); }
            if (itemsPerPage < 1) { throw new ArgumentOutOfRangeException(nameof(pageIndex), "There must be at least one item per page."); }
            if (totalPages < 1) { throw new ArgumentOutOfRangeException(nameof(totalPages), "TotalPages must be greater than 0."); }
            if (!items.Any()) { throw new ArgumentOutOfRangeException(nameof(totalPages), "No Items Were Fetched"); }

            Items = items;
            PaginationInfo = new PaginationInfo(pageIndex, itemsPerPage, totalPages);
        }
    }
}
