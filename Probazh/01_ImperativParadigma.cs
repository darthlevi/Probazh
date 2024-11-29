using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        public void Vegrehajtas()
        {

        }
    }
    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        public T[] tarolo;
        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
        }
        public int n;
        public void Felvesz(T elem)
        {
            if (n < tarolo.Length)
            {
                tarolo[n] = elem;
                n++;
            }
            else { throw new TaroloMegteltKivetel(); }
        }
        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        public IEnumerator<T> GetEnumerator()  //BejáróLétrehozás
        {
            return new FeladatTaroloBejaro<T>(tarolo, n);
        }
        IEnumerator IEnumerable.GetEnumerator() // nemkell
        {
            return GetEnumerator();
        }
    }
    public class TaroloMegteltKivetel : Exception { }
    public interface IFuggo
    {
        bool FuggosegTeljesul { get; }
    }
    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int meret) : base(meret) { }
        public override void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                { tarolo[i].Vegrehajtas(); }
            }
        }
    }
    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        private T[] tarolo;
        private int n;
        private int AktualisIndex = -1;
        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo; this.n = n;
        }
        public T Current => tarolo[AktualisIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (n > AktualisIndex)
            {
                AktualisIndex++;
                return true;
            }
            else { return false; }
        }
        public void Reset()
        {
            AktualisIndex = -1;
        }
    }
}
