using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionExtensions
{
    public static class CollectionPartitionExtension
    {

        /// <summary>
        /// Get the array slice between the two indexes; If overflow or inderflow occurs, 
        /// will return an empty array of type T. Slice is inclusive for start, exclusive
        /// for end
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="arr">Array to be sliced</param>
        /// <param name="start">Index at which the slice starts; cannot be negative.</param>
        /// <param name="end">Endpoint for the slice of the array. Supports negative values</param>
        /// <returns>Array slice adhereing to the start and ending points, or a strongly
        /// typed emtpy array if an underflow or overflow situation is encountered</returns>
        public static T[] Slice<T>(this T[] arr, int start, int end)
        {
            int segmentEnd = end;
            //check for and handle underflow/overflow scenarios
            if (start < 0 || start > arr.Length)
            {
                return new T[0];
            }
            if (end > 0 && start > end || end == 0)
            {
                return new T[0];
            }

            if (end < 0)
            {
                //end is negative value, so adding will result in subtraction
                segmentEnd = arr.Length + end;

                //handle scenario where negative end index causes underflow
                if (segmentEnd < start)
                {
                    return new T[0];
                }
            }

            int len = segmentEnd - start;

            T[] newArry = new T[len];
            for (int i = 0; i < len; i++)
            {
                newArry[i] = arr[i + start];
            }

            return newArry;
        }


    }
}
