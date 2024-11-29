using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        private HatizsakProblema problema;
        public int LepesSzam { get; private set; }
        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }

        private float[,] TáblázatFeltöltés()
        {
            float[,] F = new float[problema.N + 1, problema.Wmax + 1];
            for (int t = 0; t < F.GetLength(0); t++)
            {
                for (int h = 0; h < F.GetLength(1); h++)
                {
                    LepesSzam++;
                    if (t == 0)
                    {
                        F[t, h] = 0;
                    }
                    else if (h == 0)
                    {
                        F[t, h] = 0;
                    }
                    else if (h < problema.W[t - 1])
                    {
                        F[t, h] = F[t - 1, h];
                    }
                    else if (h >= problema.W[t - 1])
                    {
                        F[t, h] = new float[] { F[t - 1, h], F[t - 1, h - problema.W[t - 1]] + problema.P[t - 1] }.Max();
                    }
                }
            }
            return F;
        }
        public float OptimalisErtek()
        {
            return TáblázatFeltöltés()[problema.N, problema.Wmax];
        }
        public bool[] OptimalisMegoldas()
        {
            float[,] F = TáblázatFeltöltés();
            bool[] O = new bool[problema.N];
            int t = problema.N;
            int h = problema.Wmax;
            while ((t > 0) && (h > 0))
            {
                if (F[t, h] != F[t - 1, h])
                {
                    O[t - 1] = true;
                    h = h - problema.W[t - 1];
                }
                t--;
            }
            return O;
        }
    }
}
