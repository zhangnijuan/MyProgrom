using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ndtech.WarningEngine
{
  public class WarningService
    {
     static InquiryOffDateScheduler inquiryOffDate = new InquiryOffDateScheduler();
      public static void Start()
      {
          WarningPipeManager.Initialize();
          AssemblyName assemblyName = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Bin" + Path.DirectorySeparatorChar + "Ndtech.PortalService.dll");
          Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
          foreach (Type tp in assembly.GetTypes().Where<Type>(x => x.BaseType == typeof(AbstractListener)))
          {
 
                  AbstractListener listener = AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap
                      (AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Bin" + Path.DirectorySeparatorChar + "Ndtech.PortalService.dll", tp.FullName) as AbstractListener;
                  listener.Start();
          }
          inquiryOffDate.Start();

      }
      public static void Stop()
      {
          WarningPipeManager.Dispose();
          AssemblyName assemblyName = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Bin" + Path.DirectorySeparatorChar + "Ndtech.PortalService.dll");
          Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
          foreach (Type tp in assembly.GetTypes())
          {
              if (tp.BaseType == typeof(AbstractListener))
              {
                  AbstractListener listener = AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap
                      (AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Bin" + Path.DirectorySeparatorChar + "Ndtech.PortalService.dll", tp.FullName) as AbstractListener;
                  listener.Stop();

              }
          }
          inquiryOffDate.Stop();
      }
    }
}
