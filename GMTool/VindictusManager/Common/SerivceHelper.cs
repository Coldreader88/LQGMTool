using System;
using System.Linq;
using System.ServiceProcess;

namespace Vindictus.Common
{
    public class SerivceHelper
    {

        public static ServiceController GetService(string serviceName,string machineName= "localhost")
        {
            ServiceController cs = new ServiceController();
            cs.MachineName = machineName; //主机名称
            cs.ServiceName = serviceName; //服务名称
            cs.Refresh();
            return cs;
        }

        public static bool StartService(string name, string machineName = "localhost")
        {
            if (!ExistService(name))
            {
                return false;
            }
           return StartService(GetService(name, machineName));
        }
        public static bool StopService(string name, string machineName = "localhost")
        {
            if (!ExistService(name))
            {
                return false;
            }
            return StopService(GetService(name, machineName));
        }
        public static bool StopService(ServiceController cs)
        {
            if (!IsRunningService(cs))
            {
                return true;
            }
            try
            {
                cs.Stop();
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
        public static bool StartService(ServiceController cs)
        {
            if (IsRunningService(cs))
            {
                return true;
            }
            try
            {
                cs.Start();
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
        public static bool IsRunningService(string name, string machineName = "localhost")
        {
            if (!ExistService(name))
            {
                return false;
            }
            return IsRunningService(GetService(name, machineName));
        }
        public static bool IsRunningService(ServiceController cs) {
        	return cs!=null&& (cs.Status == ServiceControllerStatus.Running || cs.Status == ServiceControllerStatus.StartPending);
        }
        public static bool ExistService(string serviceName)
        {
            var services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
