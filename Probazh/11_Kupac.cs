using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritas;

        public Kupac(T[] e, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            E = e;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }
        public static int Szulo(int i) => (i - 1) / 2;
        public static int Bal(int i) => 2 * i + 1;
        public static int Jobb(int i) => 2 * i + 2;
        protected void Kupacol(int i)
        {
            int max = i;
            var b = Bal(i);
            var j = Jobb(i);
            if (b < n && nagyobbPrioritas(E[b], E[max]))
            {
                max = b;
            }
            if (j < n && nagyobbPrioritas(E[j], E[max]))
            {
                max = j;
            }
            if (max != i )
            {
                (E[i], E[max]) = (E[max], E[i]);
                Kupacol(max);
            }
        }
        protected void KupacotEpit()
        {
            for (int i = n / 2 - 1; i >=  0; i--)
            {
                Kupacol(i);
            }
        }
    }
    public class KupacRendezes<T> : Kupac<T> where T : IComparable
    {
        public KupacRendezes(T[] E) : base(E,E.Length, (x,y) =>x.CompareTo(y) >0) 
        {
            
        }
        public void Rendezes()
        {
            KupacotEpit();
            for (int i = n - 1; i > 0; i--)
            {
                (E[0], E[i]) = (E[i], E[0]);
                n--;
                Kupacol(0);
            }
        }
    }
    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int meret, Func<T,T,bool> nagyobbPrioritas) 
            :base(new T[meret],0, nagyobbPrioritas)
        {
        }
        private void KulcsotFelvisz(int i)
        {
            int sz = Szulo(i);
            if (sz >= 0 && nagyobbPrioritas(E[i], E[sz]))
            {
                (E[i], E[sz]) = (E[sz], E[i]);
                KulcsotFelvisz(sz);
            }
        }
        public void Sorba(T érték)
        {
            if (n < E.Length)
            {
                n++;
                E[n - 1] = érték;
                KulcsotFelvisz(n - 1);
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }
        public T Sorbol()
        {
            if (!Ures)
            {
                T max = E[0];
                E[0] = E[n - 1];
                n--;
                Kupacol(0);
                return max;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
        public void Frissit(T érték)
        {
            int i = 0;
            while (i < n && !E[i].Equals(érték))
            {
                i++;
            }
            if (i < n)
            {
                KulcsotFelvisz(i);
                Kupacol(i);
            }
            else
            {
                throw new NincsElemKivetel(); ;
            }
        }
        public T Elso()
        {
            if (Ures)
                throw new NincsElemKivetel(); ;
            return E[0];
        }
        public bool Ures => n == 0;
    }
}
