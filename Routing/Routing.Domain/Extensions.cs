using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class SystemExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static Queue<T> To_Queue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }

        public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
        {
            for (int i = 0; i < count; i++)
                if (queue.Count == 0)
                    break;
                else
                    yield return queue.Dequeue();
        }
    }
}
