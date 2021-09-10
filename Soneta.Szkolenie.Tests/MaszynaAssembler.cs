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
    
        public static class MaszynaAssemblers
        {

        static public IRowBuilder<Maszyna> NowaMaszyna<T>(this IRowBuilder<T> builder, string nrboczny,  string model) where T : GuidedRow
            => builder.GetChild<Maszyna>()
            .Enqueue(m =>
            {
                m.NrBoczny = nrboczny;
                m.Model = model;
                m.DataProd = Date.Today;
            });

        }
    
}
