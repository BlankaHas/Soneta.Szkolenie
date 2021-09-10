using Soneta.Business;
using Soneta.Business.Db;
using Soneta.Config;
using Soneta.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soneta.Szkolenie.Assembler.Tests
{
    public class MojTestBase : DbTransactionTestBase
    {
        static public IRowBuilder<CfgNode> Nowy()
        {
            return new RowBuilder<CfgNode>((t, ctx) => {
                return ctx.Session.Get<BusinessModule>().CfgNodes.Root;
            }, BuilderOptions.SetResultIntoContext_No | BuilderOptions.SessionMode_UseSession);
        }
    }
}
