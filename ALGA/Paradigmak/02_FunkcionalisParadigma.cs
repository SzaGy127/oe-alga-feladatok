using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // --- osztályok ---

    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel { get; set; }

        public FeltetelesFeladatTarolo(int size) : base(size)
        {
        }

        public void FeltetelesVegrehajtas(Func<T, bool> cond)
        {
            for(int i = 0; i < n; i++)
            {
                if (cond(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            T[] output = new T[tarolo.Length];
            int temp = 0;
            if(BejaroFeltetel != null)
            {
                for (int i = 0; i < n; i++)
                {
                    if (BejaroFeltetel(tarolo[i]))
                    {
                        output[temp] = tarolo[i];
                        temp++;
                    }
                }
                return new FeltetelesFeladatTaroloBejaro<T>(output, temp, BejaroFeltetel);
            }
            return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
        }
    }

    // --- Bejarok ---

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        protected int n;
        protected T[] tarolo;
        protected int pointer = -1;
        protected Func<T, bool> cond; // ha null, akkor minden instance-nél true output-ként kezelendő

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> cond)
        {
            this.tarolo = tarolo;
            this.n = n;
            this.cond = cond;
            if(this.cond == null)
            {
                cond = Replacement;
            }
        }

        public bool Replacement(T none)
        {
            return true;
        }

        public T Current => tarolo[pointer];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if(pointer+1 < n)
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
}
