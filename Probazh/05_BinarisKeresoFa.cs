using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OE.ALGA.Adatszerkezetek
{
    class FaElem<T> where T : IComparable
    {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;
        public FaElem(T tart, FaElem<T>? bal, FaElem<T>? jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }
    public class FaHalmaz<T> : Halmaz<T> where T : IComparable
    {
        FaElem<T>? gyoker;
        #region Beszur
        static FaElem<T> ReszfabaBeszur(FaElem<T>? p, T ertek)
        {
            if (p == null)
            {
                FaElem<T>? uj = new FaElem<T>(ertek, null,null);
                return uj;
            }
            else
            {
                if (p.tart.CompareTo(ertek)>0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek)<0)
                    {
                        p.jobb = ReszfabaBeszur(p.jobb, ertek);
                    }

                }
                return p;
            }

        }
        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }
        #endregion
        #region Eleme
        static bool ReszfaEleme(FaElem<T>? p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    return ReszfaEleme(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        return ReszfaEleme(p.jobb, ertek);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }
        #endregion
        #region Torol
        static FaElem<T>? ReszfabolTorol(FaElem<T>? p,T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabolTorol(p.jobb, ertek);
                    }
                    else
                    {
                        if (p.bal == null)
                        {
                            FaElem<T>? q = p;
                            p = p.jobb;
                        }
                        else
                        {
                            if (p.jobb == null)
                            {
                                FaElem<T>? q = p;
                                p = p.bal;
                            }
                            else
                            {
                                p.bal = KetGyerekesTorles(p, p.bal);
                            }
                        }
                    }
                }
                return p;
            }
            else { throw new NincsElemKivetel(); }
               
        }
        static FaElem<T>? KetGyerekesTorles(FaElem<T>? e, FaElem<T>? r)
        {
            if (r.jobb ==null)
            {
                r.jobb = KetGyerekesTorles(e, r.jobb);
                return r;
            }
            else
            {
                e.tart =r.tart;
                FaElem<T>? q = r;
                r = r.bal;
                return r;
            }
        }
        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }
        #endregion
        #region Bejaro
        void RészfaBejárásPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p!= null)
            {
                muvelet(p.tart);
                RészfaBejárásPreOrder(p.bal, muvelet);
                RészfaBejárásPreOrder(p.jobb, muvelet);
            }
        }
        void RészfaBejárásInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p!= null)
            {
                RészfaBejárásInOrder(p.bal, muvelet);
                muvelet(p.tart);
                RészfaBejárásInOrder(p.jobb, muvelet);
            }
        }
        void RészfaBejárásPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                RészfaBejárásPostOrder(p.bal, muvelet);
                RészfaBejárásPostOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }
        public void Bejar(Action<T> muvelet)
        {
            RészfaBejárásPreOrder(gyoker,muvelet);
        }
        #endregion

    }
}
