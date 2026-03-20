using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashFeeder.Models
{
    public class PaginatedList<T>
    {
        public IReadOnlyList<T> Items { get; }

        public int PageIndex { get; }
        public int TotalPages { get; }

        // Indicates if there is a previous page available
        public bool HasPreviousPage => PageIndex > 1;

        // Indicates if there is a next page available
        public bool HasNextPage => PageIndex < TotalPages;

        // Constructor to initialize the PaginatedList with items, page index, and total pages
        public PaginatedList(List<T> items, int pageIndex, int totalPages)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "PageIndex must be greater than 0.");
            }

            if (totalPages < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(totalPages), "TotalPages must be greater than 0.");
            }

            Items = new ReadOnlyCollection<T>(items ?? throw new ArgumentNullException(nameof(items)));
            PageIndex = pageIndex;
            TotalPages = totalPages;

        }
    }
}
