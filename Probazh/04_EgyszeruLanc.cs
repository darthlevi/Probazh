using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace OE.ALGA.Adatszerkezetek
{
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T>? kov;
        public LancElem(T tart, LancElem<T>? kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }
    public class LancoltVerem<T> : Verem<T>
    {
        private LancElem<T>? fej;
        public LancoltVerem()
        {
            fej = null;
        }
        public bool Ures
        {
            get
            {
                return fej == null;
            }

        }
        public T Felso()
        {
            if (fej != null)
            {
                return fej.tart;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
        public void Verembe(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);
            fej = uj;
        }

        public T Verembol()
        {
            if (fej != null)
            {
                T ertek = fej.tart;
                fej = fej.kov;
                return ertek;
            }
            else
            {
                throw new NincsElemKivetel();
            }

        }
        public void felszabadit(LancElem<T> q)
        {

        }
    }
    public class LancoltSor<T> : Sor<T>
    {
        private LancElem<T> fej;
        private LancElem<T> vege;
        public LancoltSor()
        {
            fej = null;
            vege = null;
        }
        public bool Ures
        {
            get
            {
                return fej == null;
            }
        }
        public T Elso()
        {
            if (fej != null)
            {
                return fej.tart;
            }
            else { throw new NincsElemKivetel(); }
        }

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (fej != null)
            {
                vege.kov = uj;
            }
            else
            {
                fej = uj;
            }
            vege = uj;
        }
        public T Sorbol()
        {

            if (fej != null)
            {
                T ertek = fej.tart;
                fej = fej.kov;
                if (fej == null)
                {
                    vege = null;
                }
                return ertek;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }
    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {

        private LancElem<T> fej;
        public int Elemszam { get; private set; }
        public LancoltLista()
        {
            fej = null;
            Elemszam = 0;
        }
        public void Bejar(Action<T> muvelet)
        {
            var p = fej;
            while (p != null)
            {
                muvelet(p.tart);
                p = p.kov;
            }

        }
        public void Beszur(int index, T ertek)
        {
            if (fej == null || index == 0)
            {
                LancElem<T> uj = new LancElem<T>(ertek, fej);
                fej = uj;
            }
            else
            {
                var p = fej;
                var i = 0;
                while (p.kov != null && i < index - 1)
                {
                    p = p.kov;
                    i++;
                }
                if (i == index - 1)
                {
                    LancElem<T> uj = new LancElem<T>(ertek, p.kov);
                    p.kov = uj;
                }
                else
                {
                    throw new NincsElemKivetel();
                }
            }
            Elemszam++;
        }
        public void Hozzafuz(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (fej == null)
            {
                fej = uj;
            }
            else
            {
                var p = fej;
                while (p.kov != null)
                {
                    p = p.kov;
                }
                p.kov = uj;
            }
            Elemszam++;
        }
        public T Kiolvas(int index)
        {
            var p = fej;
            var i = 0;
            while (p != null && i < index)
            {
                p = p.kov;
                i++;
            }
            if (p != null)
            {
                return p.tart;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
        public void Modosit(int index, T ertek)
        {
            var p = fej;
            var i = 0;
            while (p != null && i < index)
            {
                p = p.kov;
                i++;
            }
            if (p != null)
            {
                p.tart = ertek;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
        public void Torol(T ertek)
        {
            LancElem<T> p = fej;
            LancElem<T> e = null;
            bool found = false;
            if (fej == null)
            {
                throw new NincsElemKivetel();
            }
            else
            {
                while (p != null)
                {
                    if (Object.Equals(p.tart, ertek))
                    {
                        if (e == null)
                        {
                            fej = p.kov;
                        }
                        else
                        {
                            e.kov = p.kov;
                        }
                        Elemszam--;
                        found = true;
                    }
                    else
                    {
                        e = p;
                    }
                    p = p.kov;
                }
                if (!found)
                {
                    throw new NincsElemKivetel();
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    class LancoltListaBejaro<T> : IEnumerator<T>
    {
        private LancElem<T> fej;
        private LancElem<T> aktualisElem;
        public T Current
        {
            get
            {
                if (aktualisElem == null)
                {
                    throw new NincsElemKivetel();
                }
                return aktualisElem.tart;
            }
        }
        public LancoltListaBejaro(LancElem<T> fej)
        {
            this.fej = fej;
            this.aktualisElem = null;
        }
        object IEnumerator.Current => Current;
        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (aktualisElem == null)
            {
                aktualisElem = fej;
            }
            else
            {
                aktualisElem = aktualisElem.kov;
            }
            return aktualisElem != null;
        }
        public void Reset()
        {
            aktualisElem = null;
        }
    }
}
