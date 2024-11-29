using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
	#region 1. feladat
	class SzotarElem<K,T>
	{
		public K Kulcs;
		public T Tart;
        public SzotarElem(K kulcs, T tart)
        {
			this.Kulcs = kulcs;
			this.Tart = tart;
        }
    }
    #endregion
    #region 2.feladat
    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        private SzotarElem<K, T>[] E;
        private Func<K, int> h;
        LancoltLista<SzotarElem<K, T>> U = new LancoltLista<SzotarElem<K,T>>();
        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int>hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            h = x=> Math.Abs(hasitoFuggveny(x))%E.Length;
        }
        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret,x=>x.GetHashCode())
        {
           
        }
        private SzotarElem<K,T> KulcsKeres(K k)
        {
            int index = h(k);
            if (E[index] != null && E[index].Kulcs.Equals(k))
            {
                return E[index];
            }
            else
            {
                foreach (var elem in U)
                {
                    if (elem.Kulcs.Equals(k))
                    {
                        return elem;
                    }
                }
            }
            return null;
        }
        public void Beir(K kulcs, T ertek)
        {
            int index = h(kulcs);
            if (KulcsKeres(kulcs) != null)
            {
                E[index] = new SzotarElem<K, T>(kulcs, ertek);
            }
            else
            {
                SzotarElem<K,T> uj = new SzotarElem<K, T>(kulcs, ertek);
                if (E[index] == null)
                {
                    E[index] = uj;
                }
                else { U.Hozzafuz(uj); }
            }
        }

        public T Kiolvas(K kulcs)
        {
            var meglevo = KulcsKeres(kulcs);
            if (meglevo!= null)
            {
                return meglevo.Tart;
            }
            else
            {
                throw new HibasKulcsKivetel();
            }
        }

        public void Torol(K kulcs)
        {
            int index = h(kulcs);
            if (E[index]!=null && E[index].Kulcs.Equals(kulcs))
            {
                E[index] = null;
            }
            else
            {
                SzotarElem<K,T> e = null;
                foreach (var elem in U)
                {
                    if (elem.Kulcs.Equals(kulcs))
                    {
                        e = elem;
                    }
                }
                if (e != null)
                { U.Torol(e); }
            }
        }
    }
    #endregion
}
