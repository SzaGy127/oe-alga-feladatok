using System;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    public class SzotarElem<K, T>
    {
        public K? kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        public SzotarElem<K, T>[] E;
        public Func<K, int> h;
        private LancoltLista<SzotarElem<K, T>> U = new LancoltLista<SzotarElem<K, T>>();

        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x=> x.GetHashCode())
        {
            
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K,int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            h = (x => Math.Abs(hasitoFuggveny(x)) % E.Length);
        }

        private SzotarElem<K, T> KulcsKeres(K kulcs)
        {
            if (E[h(kulcs)] != null && EqualityComparer<K>.Default.Equals(E[h(kulcs)].kulcs, kulcs))
            {
                return E[h(kulcs)];
            }
            if (U != null)
            {
                SzotarElem<K, T> e = null;
                U.Bejar(x => { if (EqualityComparer<K>.Default.Equals(x.kulcs, kulcs)) { e = x; } });
                if (e != null) { return e; }
            }
            return null;
        }

        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);
            if(meglevo != null)
            {
                meglevo.tart = ertek;
            }
            else
            {
                SzotarElem<K, T> uj = new SzotarElem<K, T>(kulcs, ertek);
                if (E[h(kulcs)] == null)
                {
                    E[h(kulcs)] = uj;
                }
                else
                {
                    U.Hozzafuz(uj);
                }
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);
            if (meglevo != null) 
            {
                return meglevo.tart;
            }
            else
            {
                throw new HibasKulcsKivetel();
            }
        }

        public void Torol(K kulcs)
        {
            if (E[h(kulcs)] != null
                && EqualityComparer<K>.Default.Equals(E[h(kulcs)].kulcs, kulcs))
            {
                E[h(kulcs)] = null;
            }
            else
            {
                SzotarElem<K, T> e = null;
                if (U != null) 
                { 
                U.Bejar(x => { if (EqualityComparer<K>.Default.Equals(x.kulcs, kulcs)) { e = x; } });
                }
                else
                {
                    throw new HibasKulcsKivetel();
                }
                if (e != null)
                {
                    U.Torol(e);
                }
                else
                {
                    throw new HibasKulcsKivetel();
                }
            }
        }
    }
}
