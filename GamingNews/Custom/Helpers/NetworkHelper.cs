using System.Net.NetworkInformation;

namespace GamingWeb.Custom.Helpers
{
    public class NetworkHelper
    {
        public bool IsConnectedToInternet()
        {
            string host = "http://www.c-sharpcorner.com";  
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return result;
        }
    }
}
