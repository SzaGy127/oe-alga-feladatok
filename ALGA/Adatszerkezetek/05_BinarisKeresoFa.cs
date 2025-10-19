using System;
using System.ComponentModel.Design;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> where T : IComparable<T>
    {
        public T tart;
        public FaElem<T> bal;
        public FaElem<T> jobb;

        public FaElem(T tart, FaElem<T> bal, FaElem<T> jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        FaElem<T> gyoker;

        public FaHalmaz()
        {

        }

        public void Bejar(Action<T> muvelet)
        {
            PreorderBejaras(gyoker, muvelet);
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        public FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if(p == null)
            {
                return new FaElem<T>(ertek, null, null);
            }
            else
            {
                if(p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else if(p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabaBeszur(p.jobb, ertek);
                }
            }
            return p;

        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        public bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    return ReszfaEleme(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        return ReszfaEleme(p.jobb, ertek);
                    }
                    else return true;
                }
            }
            else return false;
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }

        public FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if(p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else
                {
                    if(p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabolTorol(p.jobb, ertek);
                    }
                    else
                    {
                        if(p.bal == null)
                        {
                            p = p.jobb;
                        }
                        else
                        {
                            if(p.jobb == null)
                            {
                                p = p.bal;
                            }
                            else
                            {
                                KetGyerekesTorles(p, p.bal);
                            }
                        }
                    }
                }
                return p;
            }
            throw new NincsElemKivetel();
        }

        public FaElem<T> KetGyerekesTorles(FaElem<T> e, FaElem<T> r)
        {
            if(r.jobb != null)
            {
                r.jobb = KetGyerekesTorles(e, r.jobb);
                return r;
            }
            e.tart = r.tart;
            r = r.bal;
            return r;
        }

        public void PreorderBejaras(FaElem<T> p, Action<T> muvelet)
        {
            if(p != null) 
            {
                muvelet(p.tart);
                PreorderBejaras(p.bal, muvelet);
                PreorderBejaras(p.jobb, muvelet);
            }
        }

        //  bit ahead of myself
        /*public void InorderBejaras(FaElem<T> p, Action<T> muvelet)
        {
            if(p != null)
            {
                InorderBejaras(p.bal, muvelet);
                muvelet(p.tart);
                InorderBejaras(p.jobb, muvelet);
            }
        }

        public void PostorderBejaras(FaElem<T> p, Action<T> muvelet)
        {
            PostorderBejaras(p.bal, muvelet);
            PostorderBejaras(p.jobb, muvelet);
            muvelet(p.tart);
        }*/
    }
}
