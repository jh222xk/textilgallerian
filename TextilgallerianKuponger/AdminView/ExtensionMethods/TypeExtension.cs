using System.Linq;

namespace AdminView.ExtensionMethods
{
    public static class TypeExtension
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> self, int page, int pageSize)
        {
            return self.Skip(page * pageSize).Take(pageSize);
        }
    }
}