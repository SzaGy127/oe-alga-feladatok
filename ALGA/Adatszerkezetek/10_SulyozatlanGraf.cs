using System;
using System.Data.SqlTypes;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl : IComparable<EgeszGrafEl>, GrafEl<int>
    {
        public int Honnan => honnan;

        public int Hova => hova;

        int honnan;
        int hova;

        public EgeszGrafEl(int honnan, int hova)
        {
            this.honnan = honnan;
            this.hova = hova;
        }

        public int CompareTo(EgeszGrafEl? temp)
        {   
            if (temp.Honnan != Honnan)
            {
                if (temp.Honnan > Hova)
                {
                    return -1;
                }
                return 1;
            }
            else
            {
                if (Hova != temp.Hova)
                {
                    if (Hova > temp.Hova)
                    {
                        return 1;
                    }
                    return -1;
                }
                return 0;
            }
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        public int CsucsokSzama => csucsokSzama;

        public int ElekSzama => ElSzam();

        public Halmaz<int> Csucsok => CsucsokVissz();

        public Halmaz<EgeszGrafEl> Elek 
        {
            get 
            {
                FaHalmaz<EgeszGrafEl> fahalmaz = new();
                for(int i = 0; i < csucsokSzama; i++)
                {
                    for(int j = 0; j < csucsokSzama; j++)
                    {
                        if (M[i, j])
                        {
                            fahalmaz.Beszur(new EgeszGrafEl(i, j));
                        }
                    }
                }
                return fahalmaz;
            }
        }

        int csucsokSzama;
        public bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int csucsokSzama)
        {
            this.csucsokSzama = csucsokSzama;
            M = new bool[csucsokSzama, csucsokSzama];
        }

        public int ElSzam()
        {
            int temp = 0;
            for (int i = 0; i < csucsokSzama; i++)
            {
                for (int j = 0; j < csucsokSzama; j++)
                {
                    if (M[i, j] == true) temp++;
                }
            }
            return temp;
        }

        public FaHalmaz<int> CsucsokVissz()
        {
            FaHalmaz<int> output = new FaHalmaz<int>();
            for(int i = 0; i < csucsokSzama; i++)
            {
                output.Beszur(i);
            }
            return output;
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> kimenet = new FaHalmaz<int>();
            Elek.Bejar
                (
                x => 
                {
                    if(x.Honnan == csucs)
                    {
                        kimenet.Beszur(x.Hova);
                    }
                }
                );
            return kimenet;
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            if (M[honnan, hova])
            {
                return true;
            }
            return false;
        }
    }
    
    public class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> eljaras) where V : IComparable<V>
        {
            Sor<V> S = new LancoltSor<V>();
            S.Sorba(start);
            Halmaz<V> output = new FaHalmaz<V>();
            output.Beszur(start);
            while (!S.Ures)
            {
                V k = S.Sorbol();
                eljaras(k);
                g.Szomszedai(k).Bejar(x => { if (!output.Eleme(x)) { S.Sorba(x); output.Beszur(x); } });
            }
            return output;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> eljaras) where V : IComparable<V>
        {
            Halmaz<V> output = new FaHalmaz<V>();
            MelysegiBejarasRekurzio(g, start, output, eljaras);
            return output;
        }

        public static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V start, Halmaz<V> output, Action<V> eljaras) where V : IComparable<V>
        {
            output.Beszur(start);
            eljaras(start);
            g.Szomszedai(start).Bejar(x => { if (!output.Eleme(x)) { MelysegiBejarasRekurzio(g, x, output, eljaras); } });
        }
    }
}
