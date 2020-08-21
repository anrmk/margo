using System.Collections.Generic;
using System.Linq;

using Core.Data.Entities;

namespace Core.Extension {
    public static class CompareExtension {
        public static IEnumerable<T> Intersect<T, K>(IEnumerable<T> originalList, IEnumerable<T> comparableList)
                where T : IEntityBase<K> {
            return IntersectOperation<T, K>(originalList, comparableList, false);
        }

        public static IEnumerable<T> Exclude<T, K>(IEnumerable<T> originalList, IEnumerable<T> comparableList)
                where T : IEntityBase<K> {
            return IntersectOperation<T, K>(originalList, comparableList, true);
        }

        private static IEnumerable<T> IntersectOperation<T, K>(IEnumerable<T> originalList, IEnumerable<T> comparableList, bool exclude)
                where T : IEntityBase<K> {
            var comparebleIds = comparableList.Select(x => x.Id).ToHashSet();

            var result = new List<T>();
            foreach(var item in originalList) {
                if(comparebleIds.Contains(item.Id) ^ exclude) {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
