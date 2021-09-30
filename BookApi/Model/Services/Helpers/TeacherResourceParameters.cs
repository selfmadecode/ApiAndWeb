using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Services.Helpers
{
    public class TeacherResourceParameters
    {
        const int maxPageSize = 20;
        public string SearchParam { get; set; } 
        public string OrderBy { get; set; }

        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 5;

        public int PageSize { 
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize ? maxPageSize: value);
        }

    }
}
