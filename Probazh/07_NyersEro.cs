using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    #region 1.feladat
    public class HatizsakProblema
    {
        public int N { get; }
        public int Wmax { get; }
        public int[] W { get; }
        public float[] P { get; }
        public HatizsakProblema(int n, int wmax, int[] w, float[] p)
        {
            N = n;
            Wmax = wmax;
            W = w;
            P = p;
        }
        public int OsszSuly(bool[] pakolas)
        {
            var temp = 0;
            for (int i = 0; i < pakolas.Length; i++)
            {
                if (pakolas[i])
                {
                    temp = temp + W[i];
                }
            }
            return temp;
        }
        public float OsszErtek(bool[] pakolas)
        {
            float osszErtek = 0;
            for (int i = 0; i < N; i++)
            {
                if (pakolas[i])
                {
                    osszErtek += P[i];
                }
            }
            return osszErtek;
        }
        public bool Ervenyes(bool[] pakolas)
        {
            if (OsszSuly(pakolas)<= Wmax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion
    #region 2.feladat
    public class NyersEro<T>
    {
        public int M { get; }
        public Func<int, T> Generator { get; }
        public Func<T, float> Josag { get; }
        private int lepesszam;
        public int LepesSzam => lepesszam;
        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            M = m;
            Generator = generator;
            Josag = josag;
            lepesszam = 0;
        }
        public T OptimalisMegoldas()
        {
            T o = Generator(1);
            for (int i = 2; i < M; i++)
            {
                lepesszam++;
                T x = Generator(i);
                if (Josag(x) > Josag(o))
                {
                    o = x;
                }
            }
            return o;
        }
    }
    #endregion
    #region 3.feladat
    public class NyersEroHatizsakPakolas
    {
        private HatizsakProblema problema;
        private int lepesszam;
        public int LepesSzam => lepesszam;
        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            lepesszam = 0;
        }
        private bool[] Generator(int index)
        {
            bool[] pakolas = new bool[problema.N];
            for (int i = 0; i < problema.N; i++)
            {
                pakolas[i] = (index & (1 << i)) != 0;
            }
            return pakolas;
        }
        private float Josag(bool[] pakolas)
        {
            if (!problema.Ervenyes(pakolas))
            {
                return -1;
            }
            return problema.OsszErtek(pakolas);
        }
        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> nyersEro = new NyersEro<bool[]>(1 << problema.N, Generator, Josag);
            bool[] optimalisPakolas = nyersEro.OptimalisMegoldas();
            lepesszam = nyersEro.LepesSzam;
            return optimalisPakolas;
        }
        public float OptimalisErtek()
        {
            return Josag(OptimalisMegoldas()); ;
        }
    }
    #endregion

}
