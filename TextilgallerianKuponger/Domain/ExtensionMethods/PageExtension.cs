using System.Collections;
using System.Collections.Generic;
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
        public static IEnumerable<T> Page<T>(this IEnumerable<T> self, int page, int pageSize)
        {
            return self.Skip(page*pageSize).Take(pageSize);
        }
    }
}
