using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class PagerDto<T> {
        public int RecordsTotal { get; private set; }
        public int RecordsFiltered => RecordsTotal;

        public int StartPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }

        public int Start { get; private set; }
        public int EndPage { get; private set; }
        public IEnumerable<T> Data { get; private set; }

        public PagerDto(IEnumerable<T> list, int totalItems, int? start, int length = 20) {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)length);
            var currentPage = start ?? 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if(startPage <= 0) {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if(endPage > totalPages) {
                endPage = totalPages;
                if(endPage > 10) {
                    startPage = endPage - 9;
                }
            }

            RecordsTotal = totalItems;
            Start = currentPage;
            PageSize = length;
            TotalPages = totalPages;
            StartPages = startPage;
            EndPage = endPage;
            Data = list;
        }
    }
}
