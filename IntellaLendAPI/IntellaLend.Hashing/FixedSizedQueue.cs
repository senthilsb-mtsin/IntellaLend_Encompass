using System.Collections.Generic;

namespace IntellaLend.Hashing
{

    public class FixedSizedQueue<T> : Queue<T>
        {
            private int Size = int.MaxValue;

            public FixedSizedQueue(int size)
            {
                Size = size;
            }

            public FixedSizedQueue(IEnumerable<T> collection, int size) : base(collection)
            {
                Size = size;
                while (Count > size)
                {
                    Dequeue();
                }
            }

            public new void Enqueue(T obj)
            {
                base.Enqueue(obj);

                while (base.Count > Size)
                {
                    base.Dequeue();
                }
            }
        }

    }

