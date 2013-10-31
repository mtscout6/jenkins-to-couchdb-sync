using System.Collections.Generic;
using System.Linq;

namespace sabatoast_puller.Utils
{
    public static class ListExtensions
    {
         public static bool ListEquals<T>(this IList<T> src, IList<T> other)
         {
             if (ReferenceEquals(src, other)) return true;
             if (ReferenceEquals(src, null)) return false;
             if (ReferenceEquals(other, null)) return false;
             if (src.Count != other.Count) return false;

             var cnt = new Dictionary<T, int>();

             foreach (var item in src)
             {
                 if (cnt.ContainsKey(item))
                 {
                     cnt[item]++;
                 }
                 else
                 {
                     cnt.Add(item, 1);
                 }
             }

             foreach (var item in other)
             {
                 if (cnt.ContainsKey(item))
                 {
                     cnt[item]--;
                 }
                 else
                 {
                     return false;
                 }
             }

             return cnt.Values.All(c => c == 0);
         }
    }
}