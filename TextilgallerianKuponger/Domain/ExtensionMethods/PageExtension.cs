using System.Linq;

namespace Domain.ExtensionMethods
{
    public static class PageExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> self, int page, int pageSize)
        {
            return self.Skip(page*pageSize).Take(pageSize);
        }
    }
}
