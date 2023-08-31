using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Any_Collection
{
    public class OrderCollection<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCollection"/> class.
        /// </summary>
        public OrderCollection()
        : base()
        {
            this.IsSorted = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public OrderCollection(IEnumerable<T> collection)
        : base(collection)
        {
            this.IsSorted = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCollection"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public OrderCollection(int capacity)
        : base(capacity)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the collection is sorted.
        /// </summary>
        public bool IsSorted { get; private set; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                this.IsSorted = false;
            }
        }

        /// <summary>
        /// Adds an item to the end of the collection.
        /// </summary>
        /// <param name="item">The item to be added to the end of the collection.</param>
        public new void Add(T item)
        {
            base.Add(item);
            this.IsSorted = false;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the collection.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the collection.</param>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            this.IsSorted = false;
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            this.IsSorted = true;
        }

        /// <summary>
        /// Trims the collection to <paramref name="maxCount"/>.
        /// </summary>
        /// <param name="maxCount">Maximal count to leave.</param>
        /// <returns>True, if trim happened; false otherwise.</returns>
        public bool TrimExcess(int maxCount)
        {
            if (maxCount < 0)
            {
                return false;
            }

            int tail = this.Count - maxCount;
            if (tail > 0)
            {
                this.RemoveRange(maxCount, tail);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inserts an element into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            this.IsSorted = false;
        }

        /// <summary>
        /// Inserts the elements of a collection into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the collection.</param>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            this.IsSorted = false;
        }

        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            this.IsSorted = false;
        }

        /// <summary>
        /// Reverses the order of the elements in the entire collection.
        /// </summary>
        public new void Reverse()
        {
            base.Reverse();
            this.IsSorted = false;
        }

        /// <summary>
        /// Sorts the elements in the entire collection using the specified <see cref="Comparison"/>.
        /// </summary>
        /// <param name="comparison">The <see cref="Comparison"/> to use when comparing elements.</param>
        public new void Sort(Comparison<T> comparison)
        {
            base.Sort(comparison);
            this.IsSorted = true;
        }

        /// <summary>
        /// Not supported - sorting must be applied on the entire collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The <see cref="IComparer"/> implementation to use when comparing elements, or null to use the default comparer.</param>
        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            throw new InvalidOperationException("Sorting must be applied on the entire collection.");
        }

        /// <summary>
        /// Not supported - sorting comparer must be provided explicitly.
        /// </summary>
        public new void Sort()
        {
            throw new InvalidOperationException("Sorting comparer must be provided explicitly.");
        }

        /// <summary>
        /// Sorts the elements in the entire collection using the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer"/> implementation to use when comparing elements, or null to use the default comparer.</param>
        public new void Sort(IComparer<T> comparer)
        {
            base.Sort(comparer);
            this.IsSorted = true;
        }
    }
}
