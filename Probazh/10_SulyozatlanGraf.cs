﻿using OE.ALGA.Optimalizalas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl : GrafEl<int>, IComparable
    {
        public int Honnan { get; }
        public int Hova { get; }
        public EgeszGrafEl(int honnan, int hova)
        {
            this.Honnan = honnan;
            this.Hova = hova;
        }
        public virtual int CompareTo(object? obj)
        {
            if (obj != null && obj is EgeszGrafEl b)
            {
                if (Honnan != b.Honnan)
                {
                    return Honnan.CompareTo(b.Honnan);
                }
                else
                {
                    return Hova.CompareTo(b.Hova);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        int n;
        bool[,] tomb2D;
        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            tomb2D = new bool[n, n];
        }

        public int CsucsokSzama { get { return n; } }

        public int ElekSzama
        {
            get
            {
                var count = 0;
                for (int i = 0; i < tomb2D.GetLength(0); i++)
                {
                    for (int j = 0; j < tomb2D.GetLength(1); j++)
                    {
                        if (tomb2D[i,j] == true)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                Halmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }
        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                Halmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < tomb2D.GetLength(0); i++)
                {
                    for (int j = 0; j < tomb2D.GetLength(1); j++)
                    {
                        if (tomb2D[i, j])
                        {
                            elek.Beszur(new EgeszGrafEl(i, j));
                        }
                    }
                }
                return elek;
            }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            var szomszedok = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                if (tomb2D[csucs, i])
                {
                    szomszedok.Beszur(i);
                }
            }
            return szomszedok;
        }

        public void UjEl(int honnan, int hova)
        {
            tomb2D[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return tomb2D[honnan, hova];
        }
    }
    public static class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            var S = new LancoltSor<V>();
            S.Sorba(start);
            var F = new FaHalmaz<V>();
            F.Beszur(start);
            while (!S.Ures)
            {
                var k = S.Sorbol();
                muvelet(k);
                g.Szomszedai(k).Bejar((x) =>
                {
                    if (!F.Eleme(x))
                    {
                        S.Sorba(x);
                        F.Beszur(x);
                    }
                });

            }
            return F;
        }
        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            var F = new FaHalmaz<V>();
            MelysegiBejarasRekurzio(g, start,F, muvelet);
            return F;
        }
        public static void MelysegiBejarasRekurzio<V,E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet(k);
            g.Szomszedai(k).Bejar((x) =>
            {
                if (!F.Eleme(x))
                {
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
                }
            });

        }
    }
}
