using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobb;
        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = E;
            this.n = n;
            nagyobb = nagyobbPrioritas;
            KupacotEpit();
        }

        public static int Bal(int i)
        {
            return 2 * i;
        }
        public static int Jobb(int i)
        {
            return 2 * i + 1;
        }
        public static int Szulo(int i)
        {
            return i / 2;
        }

        protected void Kupacol(int i)
        {
            int b = Bal(i);
            int j = Jobb(i);
            int max = 0;

            if (b < n && nagyobb(E[b], E[i]))
            {
                max = b;
            }
            else
            {
                max = i;
            }

            if (j < n && nagyobb(E[j], E[max]))
            {
                max = j;
            }

            if (max != i)
            {
                T tmp = E[i];
                E[i] = E[max];
                E[max] = tmp;

                Kupacol(max);
            }
        }

        protected void KupacotEpit()
        {
            for (int i = n / 2; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable
    {
        public KupacRendezes(T[] E) : base(E, E.Length, (x, y) => x.CompareTo(y) > 0)
        {
        }

        public void Rendezes()
        {
            KupacotEpit();
            for (int i = n - 1; i >= 1; i--)
            {
                T tmp = E[0];
                E[0] = E[i];
                E[i] = tmp;
                n--;
                Kupacol(0);
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int n, Func<T, T, bool> nagyobbPrioritas) : base(new T[n], 0, nagyobbPrioritas)
        {
        }

        public bool Ures => n == 0;

        private void KulcsotFelvesz(int i)
        {
            int sz = Szulo(i);

            if (sz >= 0 && nagyobb(E[i], E[sz]))
            {

                T tmp = E[sz];
                E[sz] = E[i];
                E[i] = tmp;

                KulcsotFelvesz(sz);
            }
        }

        public T Elso()
        {
            if (Ures)
            {
                throw new NincsElemKivetel();
            }
            return E[0];
        }

        public void Frissit(T elem)
        {
            int i = 0;
            while (i < n && !E[i].Equals(elem))
            {
                i++;
            }

            if (i > n)
            {
                throw new NincsElemKivetel();
            }
            KulcsotFelvesz(i);
            Kupacol(i);
        }

        public void Sorba(T ertek)
        {
            if (n - 1 >= E.Length - 1)
            {
                throw new NincsHelyKivetel();
            }
            n++;
            E[n - 1] = ertek;
            KulcsotFelvesz(n - 1);
        }

        public T Sorbol()
        {
            if (Ures) 
            {
                throw new NincsElemKivetel();
            }
            T max = E[0];
            E[0] = E[n - 1];
            n--;
            Kupacol(0);
            return max;
        }
    }
}
