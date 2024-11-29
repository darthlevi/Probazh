using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    internal class _02_FunkcionalisParadigma
    {
    }
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable<T> where T : IVegrehajthato
    {
        public FeltetelesFeladatTarolo(int méret) : base(méret)
        {
            BejaroFeltetel = x => true;
        }
        public void FeltetelesVegrehajtas(Predicate<T> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
        public Predicate<T> BejaroFeltetel { get; set; }
        public IEnumerator<T> GetEnumerator()
        {
            if (BejaroFeltetel == null)
            {
                BejaroFeltetel = x => true;
            }
            return new FeltetesFeladatTaroloBejaro<T>(tarolo, BejaroFeltetel);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
    public class FeltetesFeladatTaroloBejaro<T> : IEnumerator<T> where T:IVegrehajthato
    {


        private T[] tarolo;
        private int n;
        private int AktualisIndex = -1;
        public Predicate<T> BejaroFeltetel;
        public FeltetesFeladatTaroloBejaro(T[] tarolo, Predicate<T> feltetel)
        {
            this.tarolo = tarolo;
            this.BejaroFeltetel = feltetel;
            n = tarolo.Length;
        }
        public T Current => tarolo[AktualisIndex];

        object IEnumerator.Current => AktualisIndex;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (++AktualisIndex < n)
            {
                if (tarolo[AktualisIndex] != null && BejaroFeltetel(tarolo[AktualisIndex]))
                {
                    return true;
                }
                else { return MoveNext(); }
            }
            return false;
        }
        public void Reset()
        {
            AktualisIndex = -1;
        }
    }

}
