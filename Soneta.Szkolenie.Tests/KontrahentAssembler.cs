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
    
        public static class KontrahentAssemblers
        {

        static public IRowBuilder<Kontrahent> NowyKontrahent<T>(this IRowBuilder<T> builder, string kod, string cenarabatu = null, string nazwa = null) where T : GuidedRow
            => builder.GetChild<Kontrahent>()
            .Enqueue(k =>
            {
                k.Kod = kod;
                k.Nazwa = string.IsNullOrEmpty(nazwa) ? kod : nazwa;

                if (!string.IsNullOrEmpty(cenarabatu))
                {
                    k.RabatTowaru = Percent.Parse(cenarabatu);
                }


            });

        }
  



}
