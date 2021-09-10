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

namespace Soneta.Szkolenie.Assembler.Tests
{
    
        public static class LotAssemblers
        {

        static public IRowBuilder<Lot> NowyLot<T>(this IRowBuilder<T> builder, string kod,  string nazwa , string miejscowosc, int cena) where T : GuidedRow
            => builder.GetChild<Lot>()
            .Enqueue(l =>
            {
                l.KodUslugi = kod;
                l.Nazwa = nazwa;
                l.LokalizacjaMiejscowosc = miejscowosc;
                l.Cena = cena;
            });

        }
    
}
