using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.SMS
{
  public  interface ISendMessage
    {
        string SendSms(string p_PhoneNumber,  string p_Content);
        string Regist();
    }
}
