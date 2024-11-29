namespace Probazh
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Láncolt lista létrehozása és feltöltése
            LancoltLista<ISzines> szinesLista = new LancoltLista<ISzines>();
            szinesLista.Beszur(0, new SzinesElem("piros"));
            szinesLista.Beszur(1, new SzinesElem("kék"));
            szinesLista.Beszur(2, new SzinesElem("zöld"));
            szinesLista.Beszur(3, new SzinesElem("kék"));
            szinesLista.Beszur(4, new SzinesElem("piros"));

            Console.WriteLine("Eredeti lista:");
            szinesLista.Bejar(elem => Console.WriteLine($"- {elem.Szin}"));

            // Kiválogatás: csak a kék színű elemek
            var kivalogatottLista = szinesLista.Kivalogat("kék");
            Console.WriteLine("\nKék elemek:");
            kivalogatottLista.Bejar(elem => Console.WriteLine($"- {elem.Szin}"));

            // Színcsere feltétellel
            szinesLista.SzinCsere(elem => elem.Szin == "piros", "arany");
            Console.WriteLine("\nSzíncsere után (piros → arany):");
            szinesLista.Bejar(elem => Console.WriteLine($"- {elem.Szin}"));
        }
    }
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
    public interface ISzines
    {
        public string Szin { get; }
        void Atszinez(string szin);
    }
    public class LancoltLista<T> where T : ISzines
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
                    throw new InvalidOperationException("Hibás index.");
                }
            }
            Elemszam++;
        }
        public void SzinCsere(string mirol, string mire)
        {
            var jelenlegi = fej;
            while (jelenlegi != null)
            {
                if (jelenlegi.tart.Szin == mirol)
                {
                    jelenlegi.tart.Atszinez(mire);
                }
                jelenlegi = jelenlegi.kov;
            }
        }
        public LancoltLista<T> Kivalogat(string szin)
        {
            var eredmenyLista = new LancoltLista<T>();
            var jelenlegi = fej;

            while (jelenlegi != null)
            {
                if (jelenlegi.tart.Szin == szin)
                {
                    eredmenyLista.Beszur(eredmenyLista.Elemszam, jelenlegi.tart);
                }
                jelenlegi = jelenlegi.kov;
            }

            return eredmenyLista;
        }
        public void SzinCsere(Func<T, bool> kellCsere, string ujSzin)
        {
            var jelenlegi = fej;

            while (jelenlegi != null)
            {
                if (kellCsere(jelenlegi.tart))
                {
                    jelenlegi.tart.Atszinez(ujSzin);
                }
                jelenlegi = jelenlegi.kov;
            }
        }
    }
    public class SzinesElem : ISzines
    {
        public string Szin { get; private set; }

        public SzinesElem(string szin)
        {
            Szin = szin;
        }

        public void Atszinez(string szin)
        {
            Szin = szin;
        }
    }
}
