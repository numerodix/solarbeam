using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Methods to manipulate arrays.
    /// </summary>
    public static class ArrayUtilities
    {
        /// <summary>
        /// Gets the list from enumerable.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static List<T> GetListFromEnumerable<T>(IEnumerable<T> enumerable)
        {
            List<T> result = new List<T>();
            foreach (T t in enumerable)
            {
                result.Add(t);
            }
            return result;
        }

        /// <summary>
        /// Clones the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static List<T> CloneList<T>(List<T> list) where T : ICloneable
        {
            return list.ConvertAll<T>(delegate(T t)
            {
                return (T)t.Clone();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[] ConvertToArray<T>(ICollection<T> list)
        {
            if (list == null)
            {
                return null;
            }
            T[] result = new T[list.Count];
            list.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Appends the contents of <paramref name="list"/> to the end of
        /// <paramref name="destination"/> and returns <paramref name="destination"/>
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="list">The list.</param>
        public static IList<T> AppendList<T>(IList<T> destination, IList<T> list)
        {
            List<T> shortcut = destination as List<T>;
            if (shortcut != null)
            {
                shortcut.AddRange(list);
            }
            else
            {
                foreach (T t in list)
                {
                    destination.Add(t);
                }
            }

            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <param name="insert">The insert.</param>
        /// <returns></returns>
        public static T[] InsertReplace<T>(T[] array, int index, T[] insert)
        {
            int newLength = array.Length + insert.Length - 1;
            Array.Resize<T>(ref array, newLength);
            // Shift all the elements from the index up to the end
            int offset = (insert.Length - 1);
            for (int i = array.Length - 1; i >= index; i--)
            {
                if (i - offset < 0)
                {
                    break;
                }
                array[i] = array[i - offset];
            }

            // Now put all the insert elements starting at index
            for (int j = 0; j < insert.Length; j++)
            {
                array[index++] = insert[j];
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static T[] Remove<T>(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (index < 0 || index >= array.Length)
            {
                throw new IndexOutOfRangeException("Cannot remove element at index " + index);
            }

            // Crush the elemnts from the end down to the index
            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }

            Array.Resize<T>(ref array, array.Length - 1);
            return array;
        }
    }
}
