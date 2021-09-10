using NUnit.Framework;
using Soneta.Business;
using Soneta.Business.Db;
using Soneta.Config;
using Soneta.CRM;
using Soneta.Test;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Szkolenie.UI;

namespace Soneta.Szkolenie.Assembler.Tests
{
    
        public static class RezerwacjeAssemblers
        {

        static public IRowBuilder<Rezerwacja> SprawdzCeneLotuZRabatem<T>(this IRowBuilder<T> builder, string nrrezerwacji,string kontrahent, string lot, string maszyna, Currency cenaporabacie) where T : GuidedRow
            => builder.GetChild<Rezerwacja>()
            .Enqueue(r =>
            {
                r.NrRezerwacji = nrrezerwacji;
                r.Klient = r.Session.Get<CRMModule>().Kontrahenci.WgKodu[kontrahent];
                r.Lot = r.Session.Get<SzkolenieModule>().Loty.WgKod[lot];
                r.Maszyna = r.Session.Get<SzkolenieModule>().Maszyny.WgNrBoczny[maszyna];

                Assert.AreEqual(r.CenaLotu, cenaporabacie);
            });

        }
    
}
