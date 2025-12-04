using System;
using System.ComponentModel;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        protected int n;
        protected int[] M;
        protected T[,] R;
        protected Func<int, T, bool> ft;
        protected Func<int, T, T[], bool> fk;
        protected Func<T[], float> josag;
        public int LepesSzam { get; protected set; }

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R,
            Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
            LepesSzam = 0;
        }




        protected virtual void Backtrack(int szint, ref T[] megoldas, ref bool letezik, ref T[] aktualisLegjobb)
        {
            int i = 0;
            while (i < M[szint])
            {
                LepesSzam++;

                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], megoldas))
                    {

                        megoldas[szint] = R[szint, i];
                        if (szint == n - 1)
                        {

                            if (!letezik || josag(megoldas) > josag(aktualisLegjobb))
                            {
                                aktualisLegjobb = (T[])megoldas.Clone();

                            }
                            letezik = true;
                        }
                        else
                        {
                            Backtrack(szint + 1, ref megoldas, ref letezik, ref aktualisLegjobb);
                        }
                    }
                }
                i++;
            }
        }


        public T[] OptimalisMegoldas()
        {
            bool van = false;
            T[] E = new T[n];
            T[] O = new T[n];
            Backtrack(0, ref E, ref van, ref O);
            if (van)
            {
                return O;
            }
            else { throw new Exception("Nincs megoldása"); }
        }

    }

    public class VisszalepesesHatizsakPakolas
    {
        public HatizsakProblema problema;
        public int LepesSzam { get; protected set; }

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public virtual bool[] OptimalisMegoldas()
        {
            int n = problema.N;
            int[] M = new int[n];
            bool[,] R = new bool[n, 2];
            for (int i = 0; i < n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (sz, r) => true;

            Func<int, bool, bool[], bool> fk = (sz, r, meg) =>
            {
                float suly = 0;
                for (int i = 0; i < sz; i++)
                    if (meg[i]) suly += problema.W[i];
                if (r) suly += problema.W[sz];
                return suly <= problema.Wmax;


            };

            var opt = new VisszalepesesOptimalizacio<bool>(n, M, R, ft, fk, problema.OsszErtek);
            bool[] legjobb = opt.OptimalisMegoldas();

            LepesSzam = opt.LepesSzam;
            return legjobb;
        }

        public double OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }

    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        public Func<int, T[], float> fb { get; }

        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] M, T[,] R,
            Func<int, T, bool> ft, Func<int, T, T[], bool> fk,
            Func<T[], float> josag, Func<int, T[], float> fb)
            : base(n, M, R, ft, fk, josag)
        {
            this.fb = fb;
        }


        protected override void Backtrack(int szint, ref T[] megoldas, ref bool letezik, ref T[] aktualisLegjobb)
        {
            int i = 0;
            while (i < M[szint])
            {
                LepesSzam++;


                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], megoldas))
                    {
                        megoldas[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!letezik || josag(megoldas) > josag(aktualisLegjobb))
                            {
                                aktualisLegjobb = (T[])megoldas.Clone();
                            }
                            letezik = true;
                        }
                        else
                        {
                            if (josag(megoldas) + fb(szint, megoldas) > josag(aktualisLegjobb))

                                Backtrack(szint + 1, ref megoldas, ref letezik, ref aktualisLegjobb);
                        }
                    }
                }
                i++;
            }
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema p) : base(p) { }

        public override bool[] OptimalisMegoldas()
        {
            int n = problema.N;
            int[] M = new int[n];
            bool[,] R = new bool[n, 2];
            for (int i = 0; i < n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (sz, r) => true;

            Func<int, bool, bool[], bool> fk = (sz, r, meg) =>
            {
                float s = 0;
                for (int i = 0; i < sz; i++)
                    if (meg[i]) s += problema.W[i];
                if (r) s += problema.W[sz];
                return s <= problema.Wmax;

            };

            Func<int, bool[], float> fb = (sz, meg) =>
            {

                float b = 0;
                for (int i = sz; i < problema.N; i++)
                {
                    if (problema.OsszSuly(meg) + problema.W[i] <= problema.Wmax)
                        b += problema.P[i];
                }
                return b;

            };

            var opt = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(n, M, R, ft, fk, problema.OsszErtek, fb);
            bool[] legjobb = opt.OptimalisMegoldas();

            LepesSzam = opt.LepesSzam;
            return legjobb;
        }
    }

}
