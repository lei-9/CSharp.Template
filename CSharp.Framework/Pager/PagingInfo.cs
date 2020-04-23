﻿using System.ComponentModel;

namespace CSharp.Framework.Pager
{
    public class PagingInfo
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 15;

        public int TotalCount { get; set; } 

        public string SearchParameters { get; set; }

        public string Order { get; set; }
        
        public bool IsAsc { get; set; }

        public int SkipSize => PageIndex * PageSize;

    }
}