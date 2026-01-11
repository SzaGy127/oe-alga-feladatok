using OE.ALGA.Adatszerkezetek.OE.ALGA.Adatszerkezetek;
using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public float Suly { get; }
        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            Suly = suly;
        }

    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        private int n;
        private float[,] M;
        public int CsucsokSzama { get { return n; } }

        public int ElekSzama
        {
            get
            {
                int db = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j])) 
                        {
                            db++; 
                        }
                    }
                }
                return db;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> halmaz = new();

                for (int i = 0; i < n; i++)
                {
                    halmaz.Beszur(i);
                }

                return halmaz;
            }

        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<SulyozottEgeszGrafEl> halmaz = new();

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            halmaz.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j]));
                        }
                    }
                }

                return halmaz;
            }
        }

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            this.M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = float.NaN;
                }
            }

        }

        public float Suly(int honnan, int hova)
        {
            if (!VezetEl(honnan, hova)) 
            {
                throw new NincsElKivetel();
            }
            return M[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> csucsok = new();

            for (int i = 0; i < n; i++)
            {
                if (VezetEl(csucs, i))
                { 
                    csucsok.Beszur(i); 
                }
            }
            return csucsok;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(M[honnan, hova]);
        }
    }

    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start)
        {
            HasitoSzotarTulcsordulasiTerulettel<V, float> L = new(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new(g.CsucsokSzama, (x, y) => L.Kiolvas(x).CompareTo(L.Kiolvas(y)) < 0);

            g.Csucsok.Bejar(x =>
            {
                L.Beir(x, float.MaxValue);
                S.Sorba(x);
            });

            L.Beir(start, 0);
            S.Frissit(start);

            while (!S.Ures)
            {
                V u = S.Sorbol();
                S.Frissit(start);

                g.Szomszedai(u).Bejar(x =>
                {
                    if (L.Kiolvas(u) + g.Suly(u, x) < L.Kiolvas(x))
                    {
                        L.Beir(x, L.Kiolvas(u) + g.Suly(u, x));
                        S.Frissit(x);
                    }
                });
            }
            return L;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start)
            where V : IComparable<V>
        {
            HasitoSzotarTulcsordulasiTerulettel<V, float> K = new(g.CsucsokSzama);
            HasitoSzotarTulcsordulasiTerulettel<V, V> P = new(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new(g.CsucsokSzama, (x, y) => K.Kiolvas(x).CompareTo(K.Kiolvas(y)) < 0);
            FaHalmaz<V> seged = new();
            g.Csucsok.Bejar(x =>
            {
                K.Beir(x, float.MaxValue);
                S.Sorba(x);
                seged.Beszur(x);
            });

            K.Beir(start, 0);
            S.Frissit(start);

            while (!S.Ures)
            {
                V u = S.Sorbol();
                seged.Torol(u);

                g.Szomszedai(u).Bejar(x =>
                {
                    if (seged.Eleme(x) && g.Suly(u, x) < K.Kiolvas(x))
                    {
                        K.Beir(x, g.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }
            return P;
        }

        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g)
            where E : SulyozottGrafEl<V>, IComparable<E>
        {
            FaHalmaz<E> A = new();
            Szotar<V, int> csucsHalmaz = new HasitoSzotarTulcsordulasiTerulettel<V, int>(g.CsucsokSzama);
            KupacPrioritasosSor<E> S = new KupacPrioritasosSor<E>(g.ElekSzama, (x, y) => x.Suly < y.Suly);

            int i = 0;
            g.Csucsok.Bejar(x =>
            {
                csucsHalmaz.Beir(x, i++);
            });

            g.Elek.Bejar(x =>
            {
                S.Sorba(x);
            });

            while (!S.Ures)
            {
                E e = S.Sorbol();
                int u = csucsHalmaz.Kiolvas(e.Honnan);
                int v = csucsHalmaz.Kiolvas(e.Hova);
                if (u != v)
                {
                    A.Beszur(e);
                    g.Csucsok.Bejar(x =>
                    {
                        if (csucsHalmaz.Kiolvas(x) == u)
                        {
                            csucsHalmaz.Beir(x, v);
                        }
                    });

                }
            }
            return A;
        }
    }
}
