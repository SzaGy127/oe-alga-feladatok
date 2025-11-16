using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Reflection.Emit;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        public int n;
        public int[] M;
        public T[,] R;
        public Func<int, T, bool> ft;
        public Func<int, T, T[], bool> fk;
        public Func<T[], float> josag;

        public VisszalepesesOptimalizacio(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            M = m;
            R = r;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public int LepesSzam { get; private set; }

        public void BackTrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            if (szint == n) return;
            int i = 0;
            while(i < M[szint])
            {
                LepesSzam++;
                if (ft(szint, R[szint, i]))
                {
                    if(fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if(szint == n-1)
                        {
                            if(!van || josag(E) > josag(O))
                            {
                                for (int z = 0; z < E.Length; z++)
                                {
                                    O[z] = E[z];
                                }
                            }
                            van = true;
                        }
                        else
                        {
                            BackTrack(szint + 1, ref E, ref van, ref O);
                        }
                    }
                }
                i++;
            }
        }

       

        public T[] OptimalisMegoldas()
        {
            T[] O = new T[n];
            T[] E = new T[n];
            bool van = false;
            BackTrack(0,  ref E,  ref van, ref O);
            return O;
        }
    }

    public class VisszalepesesHatizsakPakolas
    {
        public HatizsakProblema problema;

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool ft(int szint, bool van)
        {
            return true;
        }

        public bool fk(int szint, bool van, bool[] E)
        {
            int ossz = 0;
            for(int i = 0; i < problema.N; i++)
            {
                if (E[i] || (i== szint || van))
                {
                    ossz += problema.W[i];
                }
            }
            return ossz <= problema.Wmax;
        }

        public float josag(bool[] E)
        {
            return problema.OsszErtek(E);
        }

        public int LepesSzam { get; private set; }

        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.N];
            bool[,] R = new bool[problema.N, 2];
            for (int i = 0; i < problema.N; i++) 
            {
                M[i] = 2;
                R[i, 0] = true;
            }
            VisszalepesesOptimalizacio<bool> VisszOpt = new VisszalepesesOptimalizacio<bool>
                (
                problema.N, M, R, ft, fk, josag
                );
            bool[] E = new bool[problema.N];
            bool[] O = new bool[problema.N];
            bool van = false;
            VisszOpt.BackTrack(0,  ref E,  ref van, ref O);
            LepesSzam = VisszOpt.LepesSzam;
            return O;   
        }

        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }

    public class SzetvalasztasEsKorlatozas<T> : VisszalepesesOptimalizacio<T>
    {
        public SzetvalasztasEsKorlatozas(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag, Func<int, T[], float> fb) : base(n, m, r, ft, fk, josag)
        {
            this.fb = fb;
        }

        Func<int, T[], float> fb;
        public int LepesSzam { get; private set; }

        public void BackTrack(int szint, T[] E, bool van, T[] O)
        {
            int i = 0;
            while (i < M[szint])
            {
                LepesSzam++;
                if (ft(szint, R[szint, i]))
                {
                    LepesSzam++;
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                O = E;
                            }
                            van = true;
                        }
                        else
                        {
                            BackTrack(szint + 1, ref E, ref van, ref O);
                        }
                    }
                }
                i++;
            }
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema)
        {
        }

        public float fb(int szint, bool[] E)
        {
            float ossz = 0;
            for (int i = szint + 1; i < problema.N; i++)
            {
                ossz += problema.P[i];
            }
            return ossz;
        }

        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.N];
            bool[,] R = new bool[problema.N, 2];
            for (int i = 0; i < problema.N; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            SzetvalasztasEsKorlatozas<bool> opt = new SzetvalasztasEsKorlatozas<bool>(problema.N, M, R, ft, fk, josag, fb);
            int n = problema.N;
            bool[] E = new bool[n];
            bool[] O = new bool[n];
            bool van = false;
            opt.BackTrack(0, E, van, O);
            return O;
        }
    }
}
