///<summary>
///Copyright 2021 Jussi Mattila a.k.a John Rudy Permission is hereby granted, free of charge, to any person 
///obtaining a copy of this software and associated documentation files (the "Software"), to deal in the 
///Software without restriction, including without limitation the rights to use, copy, modify, merge, 
///publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
///Software is furnished to do so, subject to the following conditions: The above copyright notice and this 
///permission notice shall be included in all copies or substantial portions of the Software. 
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
/// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
/// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
/// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
/// USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>
/// 
/// modified QSArray for lists. QSArray is on my github. 
/// https://github.com/JohnRudy/Generic-Quicksort-Array-for-Unity



using System;
using System.Collections.Generic;

namespace Quicksort {
    public class QSList<T> where T : QSItem<T> {
        List<T> items;
        //Needed for array setting and other functions
        int currentItemCount = 0;

        //Constructor for the array
        public QSList () {
            items = new List<T>();
        }


        ///---------------------------------------------------------///
        /// Basic functionality for the array                       ///
        /// for adding, removing, getting the current count and     ///
        /// does the array contain item and returning an item       ///
        ///---------------------------------------------------------///


        public T RemoveFirst () {
            T item = items [ 0 ];
            Remove(items [ 0 ]);
            return item;
        }

        public T RemoveLast () {
            T item = items [ currentItemCount ];
            Remove(item);
            return item;
        }

        public void Remove ( T item ) {
            currentItemCount--;
            items.Remove(item);
        }

        public void Add ( T item ) {
            items.Insert(currentItemCount, item);
            currentItemCount++;
        }

        public int Count {
            get {
                return currentItemCount;
            }
        }

        public bool Contains ( T item ) {
            bool eq = false;
            foreach(T t in items ) {
                if (t.Equals(item) ) {
                    eq = true;
                }
            }
            return eq;
        }

        public void Swap ( T itemA, T itemB ) {
            T temp = itemA;
            for (int i = 0 ; i < items.Count ;i++ ) {
                if (items[i].Equals(itemA) ) {
                    items [ i ] = itemB;
                }
                else if( items [ i ].Equals(itemB) ) {
                    items [ i ] = temp;
                }
            }
        }

        public void Sort () {
            QuickSort(0, currentItemCount - 1);
        }

        public T ItemAtIndex ( int index ) {
            return items [ index ];
        }


        ///---------------------------------------------------------------///
        /// Quicksort methods, these are called within the QSArray class  ///
        ///---------------------------------------------------------------///


        private void QuickSort ( int left, int right ) {
            //Left    Pivot       Right
            //v         v          v
            //[][][][][][][][][][][]
            if( left >= right ) { return; }

            T pivot = items [ ( left + right ) / 2 ];
            int index = Partition(left, right, pivot);

            //Recursively call the method again in two sections until all values are sorted
            //   Pivot
            //Left   Right 
            //v    v    v          
            //[][][][][][][][][][][]
            QuickSort(left, index - 1);
            QuickSort(index, right);
        }

        private int Partition ( int left, int right, T pivot ) {
            while( left <= right ) {

                // Left  Pivot  Right
                //   v++ -> v <- --v
                //[][][][][][][][][][][]

                while( items [ left ].FCost < pivot.FCost ) {
                    left++;
                }

                while( items [ right ].FCost > pivot.FCost ) {
                    right--;
                }

                if( left <= right ) {
                    Swap(items [ left ], items [ right ]);
                    left++;
                    right--;
                }
            }

            //Left is our new pivot once we hit the end. 
            return left;
        }
    }



    //Interface to access itemIndex. All items must inherit this interface and variables 
    public interface QSItem<T> : IComparable<T> {
        int FCost {
            get;
            set;
        }
    }
}