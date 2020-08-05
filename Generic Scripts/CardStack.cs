// Script originally downloaded as Deque.cs from c-sharpcorner.com
// Edited for use in the MechsVsMinions project.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MvM
{
    [Serializable]
    [ComVisible(false)]
    public class CardStack<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        #region Fields

        private List<T> front;
        private List<T> back;
        private int frontDeleted;
        private int backDeleted;
        #endregion

        #region Properties

        public int Capacity
        {
            get { return front.Capacity + back.Capacity; }
        }

        public int Count
        {
            get { return front.Count + back.Count - frontDeleted - backDeleted; }
        }

        public bool IsEmpty
        {
            get { return this.Count == 0; }
        }

        public IEnumerable<T> Reversed
        {
            get
            {
                if (back.Count - backDeleted > 0)
                {
                    for (int i = back.Count - 1; i >= backDeleted; i--) yield return back[i];
                }

                if (front.Count - frontDeleted > 0)
                {
                    for (int i = frontDeleted; i < front.Count; i++) yield return front[i];
                }
            }
        }
        #endregion

        #region Constructors

        public CardStack()
        {
            front = new List<T>();
            back = new List<T>();
        }

        public CardStack(int capacity)
        {
            if (capacity < 0) throw new ArgumentException("Capacity cannot be negative");
            int temp = capacity / 2;
            int temp2 = capacity - temp;
            front = new List<T>(temp);
            back = new List<T>(temp2);
        }

        public CardStack(IEnumerable<T> backCollection) : this(backCollection, null)
        {
        }

        public CardStack(IEnumerable<T> backCollection, IEnumerable<T> frontCollection)
        {
            if (backCollection == null && frontCollection == null) throw new ArgumentException("Collections cannot both be null");
            front = new List<T>();
            back = new List<T>();

            if (backCollection != null)
            {
                foreach (T item in backCollection) back.Add(item);
            }

            if (frontCollection != null)
            {
                foreach (T item in frontCollection) front.Add(item);
            }
        }
        #endregion

        #region Methods

        public void AddTop(T item)
        {
            if (frontDeleted > 0 && front.Count == front.Capacity)
            {
                front.RemoveRange(0, frontDeleted);
                frontDeleted = 0;
            }

            front.Add(item);
        }

        public void AddBottom(T item)
        {
            if (backDeleted > 0 && back.Count == back.Capacity)
            {
                back.RemoveRange(0, backDeleted);
                backDeleted = 0;
            }

            back.Add(item);
        }

        public void AddRangeTop(IEnumerable<T> range)
        {
            if (range != null)
            {
                foreach (T item in range) this.AddTop(item);
            }
        }

        public void AddRangeBottom(IEnumerable<T> range)
        {
            if (range != null)
            {
                foreach (T item in range) this.AddBottom(item);
            }
        }

        public void Clear()
        {
            front.Clear();
            back.Clear();
            frontDeleted = 0;
            backDeleted = 0;
        }

        public bool Contains(T item)
        {
            for (int i = frontDeleted; i < front.Count; i++)
            {
                if (Object.Equals(front[i], item)) return true;
            }

            for (int i = backDeleted; i < back.Count; i++)
            {
                if (Object.Equals(back[i], item)) return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("Array cannot be null");
            if (index < 0) throw new ArgumentOutOfRangeException("Index cannot be negative");
            if (array.Length < index + this.Count) throw new ArgumentException("Index is invalid");
            int i = index;

            foreach (T item in this)
            {
                array[i++] = item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (front.Count - frontDeleted > 0)
            {
                for (int i = front.Count - 1; i >= frontDeleted; i--) yield return front[i];
            }

            if (back.Count - backDeleted > 0)
            {
                for (int i = backDeleted; i < back.Count; i++) yield return back[i];
            }
        }

        public T PeekTop()
        {
            if (front.Count > frontDeleted)
            {
                return front[front.Count - 1];
            }
            else if (back.Count > backDeleted)
            {
                return back[backDeleted];
            }
            else
            {
                throw new InvalidOperationException("Can't peek at empty Deque");
            }
        }

        public T PeekBottom()
        {
            if (back.Count > backDeleted)
            {
                return back[back.Count - 1];
            }
            else if (front.Count > frontDeleted)
            {
                return front[frontDeleted];
            }
            else
            {
                throw new InvalidOperationException("Can't peek at empty Deque");
            }
        }

        public T PopTop()
        {
            T result;

            if (front.Count > frontDeleted)
            {
                result = front[front.Count - 1];
                front.RemoveAt(front.Count - 1);
            }
            else if (back.Count > backDeleted)
            {
                result = back[backDeleted];
                backDeleted++;
            }
            else
            {
                throw new InvalidOperationException("Can't pop empty Deque");
            }

            return result;
        }


        public T PopBottom()
        {
            T result;

            if (back.Count > backDeleted)
            {
                result = back[back.Count - 1];
                back.RemoveAt(back.Count - 1);
            }
            else if (front.Count > frontDeleted)
            {
                result = front[frontDeleted];
                frontDeleted++;
            }
            else
            {
                throw new InvalidOperationException("Can't pop empty Deque");
            }

            return result;
        }

        public void Reverse()
        {
            List<T> temp = front;
            front = back;
            back = temp;
            int temp2 = frontDeleted;
            frontDeleted = backDeleted;
            backDeleted = temp2;
        }

        public T[] ToArray()
        {
            if (this.Count == 0) return new T[0];
            T[] result = new T[this.Count];
            this.CopyTo(result, 0);
            return result;
        }

        public void TrimExcess()
        {
            if (frontDeleted > 0)
            {
                front.RemoveRange(0, frontDeleted);
                frontDeleted = 0;
            }

            if (backDeleted > 0)
            {
                back.RemoveRange(0, backDeleted);
                backDeleted = 0;
            }

            front.TrimExcess();
            back.TrimExcess();
        }

        public bool TryPeekFirst(out T item)
        {
            if (!this.IsEmpty)
            {
                item = this.PeekTop();
                return true;
            }

            item = default(T);
            return false;
        }

        public bool TryPeekLast(out T item)
        {
            if (!this.IsEmpty)
            {
                item = this.PeekBottom();
                return true;
            }

            item = default(T);
            return false;
        }

        public bool TryPopFirst(out T item)
        {
            if (!this.IsEmpty)
            {
                item = this.PopTop();
                return true;
            }

            item = default(T);
            return false;
        }

        public bool TryPopLast(out T item)
        {
            if (!this.IsEmpty)
            {
                item = this.PopBottom();
                return true;
            }

            item = default(T);
            return false;
        }
        #endregion

        #region Explicit property implementations


        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return (ICollection)this; }
        }
        #endregion

        #region Explicit method implementations

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((T[])array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}