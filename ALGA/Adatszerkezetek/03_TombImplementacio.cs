using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Microsoft.VisualBasic;

namespace OE.ALGA.Adatszerkezetek
{
    // ---osztályok---

    public class TombVerem<T> : Verem<T>
    {
        public T[] E;
        public int n = 0;

        public TombVerem(int size)
        {
            E = new T[size];
        }

        public bool Ures => (n == 0);

        public T Felso()
        {
            if (!Ures)
            {
                return E[n - 1];
            }
            throw new NincsElemKivetel();

        }

        public void Verembe(T ertek)
        {
            if (n < E.Length)
            {
                E[n] = ertek;
                n++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Verembol()
        {
            if (!Ures)
            {
                n--;
                return E[n];
            }
            throw new NincsElemKivetel();
        }
    }

    public class TombSor<T> : Sor<T>
    {
        public int n = 0; // elemek száma lesz, ig
        public T[] E;
        public int e = 0; // "első" (kivett elem poz.), "elso szabad pozicio"
        public int u = 0; // utoljára berakott elem, "current index"

        public TombSor(int size)
        {
            E = new T[size];
        }

        public bool Ures => UresCheck();

        public bool UresCheck()
        {
            if (n > 0)
            {
                return false;
            }
            return true;
        }

        public T Elso()
        {
            if (!Ures)
            {
                return E[e];
            }
            throw new NincsElemKivetel();
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                n++;
                u = u % E.Length;
                E[u] = ertek;
                u++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if (!Ures)
            {
                n--;
                e = e % E.Length+1;
                return E[e-1];
            }
            throw new NincsElemKivetel();
        }
    }

    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        public T[] E = new T[1];
        public int n = 0;
        public int count = 0;

        public TombLista()
        {
        }

        public int Elemszam => count;

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (index <= n + 1)
            {
                if (n == E.Length)
                {
                    Array.Resize(ref E, E.Length * 2);
                }
                n++;
                if (n == E.Length)
                {
                    Array.Resize(ref E, E.Length * 2);
                }
                for (int i = E.Length - 1; i > index; i--)
                {
                    E[i] = E[i - 1];
                }
                E[index] = ertek;
                count++;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index < n)
            {
                return E[index];
            }
            throw new HibasIndexKivetel();
        }

        public void Modosit(int index, T ertek)
        {
            if (E[index] == null || index >= n)
            {
                throw new HibasIndexKivetel();
            }
            E[index] = ertek;
        }

        public void Torol(T ertek)
        {
            if (!EqualityComparer<T>.Default.Equals(ertek, default(T)) && E.Contains<T>(ertek))
            {
                for (int i = 0; i < E.Length; i++)
                {
                    if (EqualityComparer<T>.Default.Equals(E[i], ertek))
                    {
                        E[i] = default(T);
                        count--;
                    }
                }
                T[] temp = new T[E.Length];
                int pointer = 0;
                for (int i = 0; i < temp.Length; i++)
                {

                    if (!EqualityComparer<T>.Default.Equals(E[i], default(T)))
                    {
                        temp[pointer] = E[i];
                        pointer++;
                    }
                }
                E = temp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        public T[] E;
        public int pointer = -1;
        public int size;

        public TombListaBejaro(T[] tarolo, int size)
        {
            E = tarolo;
            this.size = size;
        }

        public T Current => E[pointer];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (pointer + 1 == E.Length)
            {
                return false;
            }
            pointer++;
            return true;
        }

        public void Reset()
        {
            pointer = -1;
        }
    }
}