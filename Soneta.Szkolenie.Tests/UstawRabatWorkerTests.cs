using NUnit.Framework;
using Soneta.Szkolenie.UI;
using Soneta.Types;
using Soneta.CRM;
using Soneta.Business.UI;
using Soneta.Szkolenie.Assembler.Tests;


namespace Soneta.Szkolenie.Tests
{
    public class UstawRabatWorkerTests : MojTestBase
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
               .NowyKontrahent("Nowy4")
               .NowyKontrahent("Nowy5","10")
               .NowyKontrahent("Nowy6","30")
               .Build();

            // Wybór kontrahentów na których zostanie ustawiony Rabat 
            Context.Set(new Kontrahent[3] {
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy1"],
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy2"],
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy3"],
                });
        }

        
        [Test]
        public void UstawRabatWorkerTest_DodacFalse_ObnizacFalse()
        {
            using (DbLogout())
            {
      
                var worker = Context.CreateObject(null, typeof(UstawRabatWorker),
                    new UstawRabatWorkerParams(Context)
                    {
                        Rabat = Percent.Parse("20%")
                    }) as UstawRabatWorker;

                Assert.DoesNotThrow(() => worker.UstawRabat(), "Wywołanie akcji \"Ustaw rabat\" spowodowało błąd.");


                var result = worker.UstawRabat() as MessageBoxInformation;
                if (result != null)
                {
                    result.YesHandler();
                }

                // Sprawdzenie poprawności działania workera - zmiana ustawinia Rabatu na Kontrahencie 
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy1"].RabatTowaru, Percent.Parse("20%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy2"].RabatTowaru, Percent.Parse("20%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy3"].RabatTowaru, Percent.Parse("30%"));

                // Sprawdzenie poprawności działania workera - brak zmiany Rabatu na Kontrahencie 
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy4"].RabatTowaru, Percent.Parse("0%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy5"].RabatTowaru, Percent.Parse("10%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy6"].RabatTowaru, Percent.Parse("30%"));
            }


         }


        [Test]
        public void UstawRabatWorkerTest_DodacTrue_ObnizycFalse()
        {
            using (DbLogout())
            {

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

                //zamienić, wiadomość 

                // Sprawdzenie poprawności działania workera - zmiana ustawinia Rabatu na Kontrahencie 
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy1"].RabatTowaru, Percent.Parse("20%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy2"].RabatTowaru, Percent.Parse("30%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy3"].RabatTowaru, Percent.Parse("50%"));

                // Sprawdzenie poprawności działania workera - brak zmiany Rabatu na Kontrahencie 
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy4"].RabatTowaru, Percent.Parse("0%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy5"].RabatTowaru, Percent.Parse("10%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy6"].RabatTowaru, Percent.Parse("30%"));

            }
        }

        [Test]
        public void UstawRabatWorkerTest_DodacFalse_ObnizycTrue()
        {
            using (DbLogout())
            {

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


                // Sprawdzenie poprawności działania workera - zmiana ustawinia Rabatu na Kontrahencie 
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy1"].RabatTowaru, Percent.Parse("20%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy2"].RabatTowaru, Percent.Parse("20%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy3"].RabatTowaru, Percent.Parse("20%"));

                // Sprawdzenie poprawności działania workera - brak zmiany Rabatu na Kontrahencie 
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy4"].RabatTowaru, Percent.Parse("0%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy5"].RabatTowaru, Percent.Parse("10%"));
                Assert.AreEqual(Session.GetCRM().Kontrahenci.WgKodu["Nowy6"].RabatTowaru, Percent.Parse("30%"));

            }


        }

    }
}
