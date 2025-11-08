using System;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        HatizsakProblema problema;

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public int LepesSzam { get; private set; }

        public float[,] TablazatFeltoltes()
        {
            LepesSzam = 0;
            float[,] output = new float[problema.N + 1, problema.Wmax + 1];
            for(int i = 0; i < problema.N + 1; i++)
            {
                for(int j = 0; j <  problema.Wmax+1; j++)
                {
                    if(i == 0 || j == 0)
                    {
                        output[i, j] = 0;
                        LepesSzam--;
                    }
                    else if(j > 0 && i > 0 && j < problema.W[i-1])
                    {
                        output[i, j] = output[i-1, j];
                    }
                    else if(i > 0 && j > 0 && j>= problema.W[i-1])
                    {
                        output[i, j] = Math.Max(output[i-1, j], output[i-1, j-problema.W[i-1]] + problema.P[i-1]);
                    }
                    LepesSzam++;
                }
            }
            return output;
        }

        public float OptimalisErtek()
        {
            return TablazatFeltoltes()[problema.N, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            float[,] tablazat = TablazatFeltoltes();
            int t = problema.N;
            int h = problema.Wmax;
            bool[] output = new bool[t];
            while(t > 0 && h > 0)
            {
                if (tablazat[t,h] != tablazat[t - 1, h])
                {
                    output[t-1] = true;
                    h = h - problema.W[t-1];
                }
                t = t - 1;
            }
            return output;
        }
    }
}
