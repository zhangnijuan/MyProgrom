using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.WarningEngine
{
    internal class SendSubscribeMessage :AbstractListener
    {
        public override string ListenedChannel
        {
            get
            {
                return "SubscribeMessage";
            }
        }
        protected override void Process(Info p_Info)
        {

        }
    }
}
