using System;
using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    ///   Paged list to represent the page index, size, total records, and total pages in addition to the items.
    /// </summary>
    /// <typeparam name="T"> Type of items to hold in the paged list. </typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        ///   Empty/ null object.
        /// </summary>
        public static readonly PagedList<T> Empty = new PagedList<T>(1, 1, 0, null);

        /// <summary>
        ///   Initialize w/ items, page index, size and total records.
        /// </summary>
        /// <param name="items"> The items representing the list. </param>
        /// <param name="pageIndex"> Page index to start at. </param>
        /// <param name="pageSize"> Page size to start with. </param>
        /// <param name="totalRecords"> Total number of records to start with. </param>
        public PagedList(int pageIndex, int pageSize, int totalRecords, ICollection<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalRecords;
            TotalPages = (int) Math.Ceiling(TotalCount/(double) PageSize);
            if (items != null && items.Count > 0)
            {
                AddRange(items);
            }
        }

        /// <summary>
        ///   Get/set the size of a page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///   Get/set the page index.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        ///   Get/set the total number of items in the list.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        ///   Get/set the total number of pages.
        /// </summary>
        public int TotalPages { get; private set; }
    }
}