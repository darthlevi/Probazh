using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> :Verem<T>
    {
        private T[] E;
        private int n;
        public bool Ures 
        { 
            get { return n <= -1; }
        }

        public TombVerem(int meret)
        {
            n = meret;
            E = new T[meret];
            n = -1;
        }
        public void Felszabadit()
        {
            Array.Clear(E);
            
        }
        public void Verembe(T ertek)
        {
            if (E.Length > n+1)
            {
                E[++n] = ertek;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }
        public T Verembol()
        {
            if (n>-1)
            {
                var save = E[n];
                Array.Clear(E, n--, 1);
                return save;
            }
            else
            {
                throw new NincsElemKivetel();

            }
        }
        public T Felso()
        {
            if (!Ures)
            {
                return E[n];
            }
            else { throw new NincsElemKivetel(); }
        }

    }

    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int e;
        private int u;
        private int n;
        public bool Ures
        {
            get { return n ==0; }
        }
        public TombSor(int meret)
        {
            E = new T[meret];
            n = 0;
            e = 0;
            u = 0;
        }
        public T Elso()
        {
            if (!Ures)
            {
                return E[u];
            }
            else
            { throw new NincsElemKivetel(); }
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                E[e] = ertek;
                e++;
                if (e == E.Length)
                {
                    e = 0;
                }
                n++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if(!Ures)
            {
               
                var save = E[u];
                u++;
                if (u == E.Length)
                {
                    u = 0;
                }
                n--;
                return save;
            }
            else
            { throw new NincsElemKivetel(); }
        }
    }

    public class TombLista<T> :  Lista<T>, IEnumerable<T>
    {
        private T[] E;
        private int n =0;

        public int Elemszam
        {
            get { return n; }
        }
        public TombLista(int meret)
        {
            E = new T[meret];
        }
        public TombLista() { E = new T[420]; }
        private void ppenlargement()
        {
            T[] enlargedpp = new T[E.Length*2];
            for (int i = 0; i < E.Length; i++)
            {
                enlargedpp[i] = E[i];
            } 
            E= enlargedpp;
        }
        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (n+1 > E.Length)
            {
                ppenlargement();
            }
            if (index <= n)
            {
                n++;
                for (int i = n-1; i > index; i--)
                {
                    E[i] = E[i - 1];
                }
                E[index] = ertek;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }
        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index<n && index>-1)
            {
                return E[index];
            }
            else
            {
                throw new HibasIndexKivetel();
            }
            
        }

        public void Modosit(int index, T ertek)
        {
            if (n + 1 > E.Length)
            {
                ppenlargement();
            }
            if (index < n)
            {
                E[index] = ertek;
            }
            else if (index == n)
            {
                E[index] = ertek;
                n++;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Torol(T ertek)
        {
            int fasz = 0;
            for (int i = 0; i < n+1; i++)
            {
                if (E[i].Equals(ertek))
                {
                    fasz++;
                }
                else
                {
                    E[i - fasz] = E[i];
                }
            }
            n = n - fasz;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public class TombListaBejaro<T> : IEnumerator<T>
    {
        private T[] E;
        private int n;
        private int aktualisIndex;
        public T Current => E[aktualisIndex];
        public TombListaBejaro(T[] E , int n)
        {
            this.E = E;
            this.n = n;
            aktualisIndex =  - 1;
        }
        

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (aktualisIndex+1<n)
            {
                aktualisIndex++;
                return true;
            }
            else { return false; }

            
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }
}
