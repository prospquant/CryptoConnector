using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Any_Collection
{
    public class SyncList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> _inner = new List<T>();
        private readonly object _syncRoot = new object();
        public object SyncRoot
        {
            get
            {
                return this._syncRoot;
            }
        }

        public delegate void AddItemNotification<T>(T item);

        public event AddItemNotification<T> OnItemAdded;

        public delegate void RemoveItemNotification<T>(T item);

        public event RemoveItemNotification<T> OnItemRemoved;

        public int Count
        {
            get
            {
                int count;
                lock (this.SyncRoot)
                {
                    count = this._inner.Count;
                }
                return count;
            }
        }
        bool System.Collections.Generic.ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(T value)
        {
            lock (this.SyncRoot)
            {
                this._inner.Add(value);
                if (OnItemAdded != null) OnItemAdded(value);
            }
        }

        public void AddRange(IEnumerable<T> values)
        {
            lock (this.SyncRoot)
            {
                this._inner.AddRange(values);
            }
        }

        public bool Remove(T item)
        {
            lock (this.SyncRoot)
            {
                var result =  this._inner.Remove(item);
                if (result && OnItemRemoved != null)
                OnItemRemoved(item);

                return result;
            }
        }

        public int RemoveAll(Predicate<T> predicate)
        {
            lock (this.SyncRoot)
            {
                return this._inner.RemoveAll(predicate);
            }
        }

        public void RemoveRange(IEnumerable<T> values)
        {
            lock (this.SyncRoot)
            {
                foreach (var item in values)
                {
                    this._inner.Remove(item);
                }
               
            }
        }


        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this._inner.Clear();
            }
        }

        public bool Contains(T item)
        {
            bool result;
            lock (this.SyncRoot)
            {
                result = this._inner.Contains(item);
            }
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this.SyncRoot)
            {
                this._inner.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            System.Collections.Generic.IEnumerator<T> result;
            lock (this.SyncRoot)
            {
                result = this._inner.GetEnumerator();
            }
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    
    }
}
