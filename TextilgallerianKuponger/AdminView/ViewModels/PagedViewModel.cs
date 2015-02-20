using System.Linq;

namespace AdminView.ViewModel
{
    public class PagedViewModel<T>
    {
        public IQueryable<T> PagedObjects { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}