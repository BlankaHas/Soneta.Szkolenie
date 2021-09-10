using NUnit.Framework;
using Soneta.Szkolenie.UI;
using Soneta.Types;
using Soneta.CRM;
using Soneta.Business.UI;
using Soneta.Szkolenie.Assembler.Tests;
using System;
using Soneta.Test;

namespace Soneta.Szkolenie.Tests
{
    public class UstawRabatWorkerTests : MojTestBase
    {
        public override void TestSetup()
        {
            LoadAssembly("Soneta.Szkolenie");
            base.TestSetup();

            // Dodanie kontrahentów za pomocą utworzonych Assemblerów 
            var arrayKontrahenciBuilder = new[] {
               NowyKontrahent("Nowy1"),
               NowyKontrahent("Nowy2", "10"),
               NowyKontrahent("Nowy3", "30"),
               NowyKontrahent("Nowy4"),
               NowyKontrahent("Nowy5", "10"),
               NowyKontrahent("Nowy6", "30"),
               };
            Kontrahent[] t =  arrayKontrahenciBuilder.Build();

            // chcemy utworzyc k. w okreslonej kategorii dostarczonej przez kontekst.
            var builder = NowyKontrahent("k.kat.")
                .Enqueue(x => { });
            Assert.That(builder.Build(), Is.Not.Null);

            builder.Enqueue(x => { });
            //Assert.That(builder.Build(), Is.Not.Null);

            //var refK = builder.Utwórz();
            Assert.That(builder.Utwórz(), Is.Not.Null);

            builder.Enqueue(x => { });
            Assert.That(builder.Utwórz(), Is.Not.Null);

            builder.Usuń().Utwórz();



            var c = new Soneta.Test.Helpers.ContextBasedRow<Kontrahent>(Context);
            var k = c.Row;
            var w = c.GetWorker<KontaktOsobaDodajOswiadczeniaWorker>();

            builder.Enqueue((kth, cx) =>
            {
                var cxRow = new Soneta.Test.Helpers.ContextBasedRow<Kontrahent>(kth, cx);
                var worker = cxRow.GetWorker<KontaktOsobaDodajOswiadczeniaWorker>();
                worker.DodajOświadczenia();
            });

            builder.Build( cx=> { cx.Set(new Kontrahent[] { }) } );









            // Wybór kontrahentów na których zostanie ustawiony Rabat 
            Context.Set(new Kontrahent[3] {
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy1"],
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy2"],
                    Session.GetCRM().Kontrahenci.WgKodu["Nowy3"],
                });
        }

        private IRowBuilder<Kontrahent> NowyKontrahent(string v1, string v2 = "0")
        {
            IRowBuilder<Kontrahent> b = new RowBuilder<Kontrahent>();
            b
                .Enqueue(x => x.Kod = v1)
                .Enqueue(x => x.RabatTowaru = Percent.Parse(v2));
            return b;
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
