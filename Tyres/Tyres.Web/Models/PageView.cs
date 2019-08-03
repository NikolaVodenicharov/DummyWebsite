using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tyres.Web.Models
{
    public class PageView<T>
    {
        public int Page { get; set; }
        public int PagesCount { get; set; }

        public bool HasPrevious => this.Page > 1;
        public int PreviousPage => this.Page - 1;

        public bool HasNext => this.Page < PagesCount;
        public int NextPage => this.Page + 1;

        public IEnumerable<T> Elements { get; set; }
    }
}
