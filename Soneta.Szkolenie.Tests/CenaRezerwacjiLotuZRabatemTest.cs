using NUnit.Framework;
using Soneta.Szkolenie.UI;
using Soneta.Types;
using Soneta.CRM;
using Soneta.Business.UI;
using Soneta.Szkolenie.Assembler.Tests;


namespace Soneta.Szkolenie.Tests
{
    public class CenaRezerwacjiLotuZRabatemTest : MojTestBase
    {
        public override void TestSetup()
        {
            LoadAssembly("Soneta.Szkolenie");
            base.TestSetup();

            // Dodanie kontrahentów za pomocą utworzonych Assemblerów 
            NowyTest()
               .NowyKontrahent("Nowy1")
               .NowyKontrahent("Nowy2","10")
               .NowyKontrahent("Nowy3","30")

           // Dodanie Lotu za pomocą utworzonych Assemblerów
               .NowyLot("WR","Lot na Wrocławiemb","Wrocław",3000)

           // Dodanie Maszyny za pomoca utworzonych Assemblerów
               .NowaMaszyna("KL-XPT","xxxxxx")
               .Build();

            // Wybór kontrahentów na których zostanie ustawiony Rabat 
            Context.Set(new Kontrahent[3] {
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy1"],
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy2"],
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy3"],
                });
        }

        
        [Test]
        public void CenaRezerwacjiLotuZRabatem_DodacFalse_ObnizycFalse()
        {
            using (DbLogout())
            {

                // Czy worker można opakować w Assembler ??? - żeby test miał kilka linijek bo ustawiam to w każdym 
                var worker = Context.CreateObject(null, typeof(UstawRabatWorker),
                    new UstawRabatWorkerParams(Context)
                    {
                        Rabat = Percent.Parse("20%")
                    }) as UstawRabatWorker;


                var result = worker.UstawRabat() as MessageBoxInformation;
                if (result != null)
                {
                    result.YesHandler();
                }

                NowyTest().
                    SprawdzCeneLotuZRabatem("1", "Nowy1", "WR", "KL-XPT", 2400).
                    SprawdzCeneLotuZRabatem("2", "Nowy2", "WR", "KL-XPT", 2400).
                    SprawdzCeneLotuZRabatem("3", "Nowy3", "WR", "KL-XPT", 2100).
                Build();

            }

         }


        [Test]
        public void CenaRezerwacjiLotuZRabatem_DodacTrue_ObnizycFalse()
        {
            using (DbLogout())
            {

                // Czy worker można opakować w Assembler ??? - żeby test miał kilka linijek bo ustawiam to w każdym 
                var worker = Context.CreateObject(null, typeof(UstawRabatWorker),
                    new UstawRabatWorkerParams(Context)
                    {
                        Rabat = Percent.Parse("20%"),
                        DodawacRabaty = true

                    }) as UstawRabatWorker;


                var result = worker.UstawRabat() as MessageBoxInformation;
                if (result != null)
                {
                    result.YesHandler();
                }

                NowyTest().
                    SprawdzCeneLotuZRabatem("1", "Nowy1", "WR", "KL-XPT", 2400).
                    SprawdzCeneLotuZRabatem("2", "Nowy2", "WR", "KL-XPT", 2100).
                    SprawdzCeneLotuZRabatem("3", "Nowy3", "WR", "KL-XPT", 1500).
                Build();

            }

        }

        [Test]
        public void CenaRezerwacjiLotuZRabatem_DodacFalse_ObnizycTrue()
        {
            using (DbLogout())
            {

                // Czy worker można opakować w Assembler ??? - żeby test miał kilka linijek bo ustawiam to w każdym 
                var worker = Context.CreateObject(null, typeof(UstawRabatWorker),
                    new UstawRabatWorkerParams(Context)
                    {
                        Rabat = Percent.Parse("20%"),
                        ObnizacRabaty = true

                    }) as UstawRabatWorker;


                var result = worker.UstawRabat() as MessageBoxInformation;
                if (result != null)
                {
                    result.YesHandler();
                }

                NowyTest().
                    SprawdzCeneLotuZRabatem("1", "Nowy1", "WR", "KL-XPT", 2400).
                    SprawdzCeneLotuZRabatem("2", "Nowy2", "WR", "KL-XPT", 2400).
                    SprawdzCeneLotuZRabatem("3", "Nowy3", "WR", "KL-XPT", 2400).
                Build();

            }

        }

    }
}
