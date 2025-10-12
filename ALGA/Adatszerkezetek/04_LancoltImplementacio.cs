using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace OE.ALGA.Adatszerkezetek
{
    // --- osztályok ---
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T>? kov;

        public LancElem(T tart, LancElem<T>? kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }

    public class LancoltVerem<T> : Verem<T>
    {
        LancElem<T> fej;
        public bool Ures
        {
            get
            {
                return fej == null;
            }
        }

        public T Felso()
        {
            if(fej != null)
            {
                return fej.tart;
            }
            throw new NincsElemKivetel();
        }

        public void Verembe(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);
            fej = uj;
        }

        public T Verembol()
        {
            if(fej != null)
            {
                T ertek = fej.tart;
                LancElem<T> q = fej;
                fej = fej.kov;
                return ertek;
            }
            throw new NincsElemKivetel();
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        LancElem<T>? fej;
        LancElem<T>? veg;

        public bool Ures => (fej == null);

        public T Elso()
        {
            if(fej != null)
            {
                return fej.tart;
            }
            throw new NincsElemKivetel();
        }

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if(veg != null)
            {
                veg.kov = uj;
            }
            else
            {
                fej = uj;
            }
            veg = uj;
        }

        public T Sorbol()
        {
            if (!Ures)
            {
                if(fej != null)
                {
                    T ertek = fej.tart;
                    LancElem<T> q = fej;
                    fej = fej.kov;
                    if(fej == null)
                    {
                        veg = null;
                    }
                    return ertek;
                }
            }
            throw new NincsElemKivetel();
        }
    }

    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        LancElem<T>? fej;
        public int n = 0;

        public LancoltLista()
        {
            fej = null;
        }

        public int Elemszam => n;

        public void Bejar(Action<T> muvelet)
        {
            LancElem<T> p = fej;
            while(p != null)
            {
                muvelet(p.tart);
                p = p.kov;
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (fej == null || index == 0)
            {
                LancElem<T> uj = new LancElem<T>(ertek, fej);
                fej = uj;
                n++;
            }
            else
            {
                LancElem<T> p = fej;
                int i = 1;
                while (p.kov != null && i < index)
                {
                    p = p.kov;
                    i++;
                }
                if(i <= index)
                {
                    LancElem<T> uj = new LancElem<T>(ertek, p.kov);
                    p.kov = uj;
                    n++;
                }
                else
                {
                    throw new HibasIndexKivetel();
                }
            }
        }

        public void Hozzafuz(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (fej == null)
            {
                fej = uj;
                n++;
            }
            else
            {
                LancElem<T> p = fej;
                while (p.kov != null)
                {
                    p = p.kov;
                }
                p.kov = uj;
                n++;
            }
        }

        public T Kiolvas(int index)
        {
            LancElem<T> p = fej;
            int i = 0;
            while (p != null && i < index)
            {
                p = p.kov;
                i++;
            }
            if(p != null)
            {
                return p.tart;
            }
            throw new HibasIndexKivetel();
        }


        public void Modosit(int index, T ertek)
        {
            LancElem<T>? p = fej;
            int i = 0;
            while(p != null && i < index)
            {
                p = p.kov;
                i++;
            }
            if(p != null)
            {
                p.tart = ertek;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Torol(T ertek)
        {
            LancElem<T> p = fej;
            LancElem<T> e = null;
            do
            {
                while (p != null && !p.tart.Equals(ertek))
                {
                    e = p;
                    p = p.kov;
                }
                if(p != null)
                {
                    LancElem<T> q = p.kov;
                    if(e == null)
                    {
                        fej = q;
                        n--;
                    }
                    else
                    {
                        e.kov = q;
                        n--;
                    }
                    p = q;
                }
            } while (p != null);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // --- bejarók ---

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        LancElem<T> aktualis;
        LancElem<T> fej;

        public LancoltListaBejaro(LancElem<T> fej)
        {
            this.fej = fej;
            Reset();
        }

        public T Current => aktualis.tart;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if(aktualis == null)
            {
                aktualis = fej;
            }
            else
            {
                aktualis = aktualis.kov;
            }
            return aktualis != null;
        }

        public void Reset()
        {
            aktualis = null;
        }
    }
}
