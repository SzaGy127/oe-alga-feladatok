using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        void Vegrehajtas()
        { }
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int size) : base(size)
        {
        }

        public override void MindentVegrehajt()
        {
            foreach(T elem in this)
            {
                if (elem.FuggosegTeljesul)
                {
                    elem.Vegrehajtas();
                }
            }
        }
    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        protected int n = 0;
        protected T[] tarolo;

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public T Current => tarolo[n];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (n + 1 <= tarolo.Length)
            {
                n++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            n = 0;
        }
    }

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        T[] tarolo;
        int n = 0;

        public FeladatTarolo(int size)
        {
            this.tarolo = new T[size];
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
            for (int i = 0; i < tarolo.Length; i++)
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

    [Serializable]
    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel()
        {
        }

        public TaroloMegteltKivetel(string message) : base(message)
        {
        }

        public TaroloMegteltKivetel(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TaroloMegteltKivetel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
