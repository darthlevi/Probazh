using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        public int n { get; private set; }
        public int[] M { get; private set; }
        public T[,] R { get; private set; }
        public Func<int, T, bool> ft;
        public Func<int, T, T[], bool> fk;
        public Func<T[], float> josag;
        public int LepesSzam { get; protected set; }
        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
            LepesSzam = 0;
        }
        protected virtual void Backtrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            int i = -1;
            while (i < M[szint] - 1)
            {
                LepesSzam++;
                i++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, E.Length);
                            }
                            van = true;
                        }
                        else
                        {
                            Backtrack(szint + 1, ref E, ref van, ref O);
                        }
                    }
                }
            }
        }
        public T[] OptimalisMegoldas()
        {
            bool van = false;
            T[] E = new T[n];
            T[] O = new T[n];
            Backtrack(0, ref E, ref van, ref O);
            if (van)
            { return O; }
            else { throw new Exception("Nincs megoldás"); }
        }
    }
    public class VisszalepesesHatizsakPakolas
    {
        public HatizsakProblema problema;
        public int LepesSzam { get; protected set; }
        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }
        public virtual bool[] OptimalisMegoldas()
        {
            int n = problema.N;
            int[] M = new int[n];
            for (int i = 0; i < n; i++)
            {
                M[i] = 2;
            }
            bool[,] R = new bool[n, 2];
            for (int i = 0; i < n; i++)
            {
                R[i, 0] = true;
                R[i, 1] = false;
            }
            Func<int, bool, bool> ft = (szint, ertek) => { return !ertek || problema.W[szint] <= problema.Wmax; };
            Func<int, bool, bool[], bool> fk = (szint, ertek, megoldas) => { return problema.OsszSuly(megoldas) <= problema.Wmax && (!ertek || problema.OsszSuly(megoldas) + problema.W[szint] <= problema.Wmax); };
            Func<bool[], float> josag = problema.OsszErtek;
            var vlmi = new VisszalepesesOptimalizacio<bool>
                (
                    n,
                    M,
                    R,
                    ft,
                    fk,
                    josag
                );
            bool[] optimalismegoldas = vlmi.OptimalisMegoldas();
            LepesSzam = vlmi.LepesSzam;
            return optimalismegoldas;
        }
        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }
    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        public Func<int, T[], float> fb { get; set; }
        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag, Func<int, T[], float> fb) : base(n, M, R, ft, fk, josag)
        {
            this.fb = fb;
        }
        protected override void Backtrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            int i = -1;
            while (i < M[szint] - 1)
            {
                LepesSzam++;
                i++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, E.Length);
                            }
                            van = true;
                        }
                        else
                        {
                            if (josag(E) + fb(szint, E) > josag(O))
                            {
                                Backtrack(szint + 1, ref E, ref van, ref O);
                            }
                        }
                    }
                }
            }
        }
    }
    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema)
        {

        }
        public override bool[] OptimalisMegoldas()
        {
            int n = problema.N;
            int[] M = new int[n];
            for (int i = 0; i < n; i++)
            {
                M[i] = 2;
            }
            bool[,] R = new bool[n, 2];
            for (int i = 0; i < n; i++)
            {
                R[i, 0] = true;
                R[i, 1] = false;
            }
            Func<int, bool, bool> ft = (szint, ertek) => { return !ertek || problema.W[szint] <= problema.Wmax; };
            Func<int, bool, bool[], bool> fk = (szint, ertek, megoldas) => { return problema.OsszSuly(megoldas) <= problema.Wmax && (!ertek || problema.OsszSuly(megoldas) + problema.W[szint] <= problema.Wmax); };
            Func<bool[], float> josag = problema.OsszErtek;
            Func<int, bool[], float> fb = (szint, megoldas) =>
            {
                float genyo = 0;
                for (int i = szint + 1; i < n; i++)
                {
                    if (problema.OsszSuly(megoldas) + problema.W[i] <= problema.Wmax)
                    { genyo += problema.P[i]; }

                }
                return genyo;
            };

                var vlmi = new SzetvalasztasEsKorlatozasOptimalizacio<bool>
                    (
                        n,
                        M,
                        R,
                        ft,
                        fk,
                        josag,
                        fb
                    );
                bool[] optimalismegoldas = vlmi.OptimalisMegoldas();
                LepesSzam = vlmi.LepesSzam;
                return optimalismegoldas;
            }
            }
    }
