using Ndtech.PortalModel.ViewModel;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Filter
{
    public class ServiceRunerFilter<IReturn> : ServiceRunner<IReturn>
    {
        ILog log = LogManager.GetLogger("runner");
        public ServiceRunerFilter(ServiceStack.WebHost.Endpoints.IAppHost appHost, ActionContext actionContext) : base(appHost, actionContext) { }
        public override void OnBeforeExecute(IRequestContext requestContext, IReturn request)
      {
          base.OnBeforeExecute(requestContext, request);
      }
      public override object OnAfterExecute(IRequestContext requestContext, object response)
      {
          return base.OnAfterExecute(requestContext, response);
      }
      public override object HandleException(IRequestContext requestContext, IReturn request, Exception ex)
      {
          log.Error(requestContext.AbsoluteUri);
          return base.HandleException(requestContext, request, ex);
      }

      private void Write(string err)
      {
          DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
          string dir = dirInfo.FullName;
          System.IO.FileStream file = new FileStream(dir + Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar + "error" + DateTime.Today.ToString("yyyy-MM-dd") + ".log", FileMode.Append);
          StreamWriter writer = new StreamWriter(file);

          writer.WriteLine(err);
          writer.Flush();
          file.Close();
      }
    }
}
