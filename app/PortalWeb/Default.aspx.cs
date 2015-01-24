
using log4net;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger("Test");
    protected void Page_Load(object sender, EventArgs e)
    {
        log.Debug("test");
    }
}