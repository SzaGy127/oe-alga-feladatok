using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // --- interface-ek ---

    public interface IVegrehajthato
    {
        public void Vegrehajtas() { }
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    // --- osztályok ---

    public class FeladatTarolo<T> : IEnumerable<T> where T: IVegrehajthato
    {
        public T[] tarolo;
        public int n;

        public FeladatTarolo(int size)
        {
            tarolo = new T[size];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n < tarolo.Length)
            {
                tarolo[n] = elem;
                n++;
            }
            else
            {
                throw new TaroloMegteltKivetel();
            }
        }

        public virtual void MindentVegrehajt()
        {
            for(int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new FeladatTaroloBejaro<T>(tarolo, n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int size) : base(size)
        {
        }

        public override void MindentVegrehajt()
        {
            if (n>0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (tarolo[i].FuggosegTeljesul)
                    {
                        tarolo[i].Vegrehajtas();
                    }
                }
            }
        }
    }

    // --- bejárók/bejárhatók ---

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        protected T[] tarolo;
        protected int pointer = -1;
        protected int size;

        public FeladatTaroloBejaro(T[] tarolo, int size)
        {
            this.tarolo = tarolo;
            this.size = size;
        }

        public T Current => tarolo[pointer];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (pointer < size-1)
            {
                pointer++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            pointer = -1;
        }
    }

    // --- kivételek ---

    [Serializable]
    public class TaroloMegteltKivetel : Exception
    {
    }
}
