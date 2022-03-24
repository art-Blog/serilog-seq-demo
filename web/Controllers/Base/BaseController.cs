using Microsoft.AspNetCore.Mvc;

namespace seq_demo.Controllers.Base;

public class BaseController : Controller
{
    /// <summary>
    /// 取得用戶端的IP位址
    /// </summary>
    protected string ClientIPAddress
    {
        get
        {
            string clientIP = string.Empty;

#if DEBUG
            clientIP = "211.75.237.68";
#else

                if (!string.IsNullOrEmpty(Request.Headers["X-Forwarded-For"]))
                    clientIP = Request.Headers["X-Forwarded-For"].Contains(",")
                             ? Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault()
                             : Request.Headers["X-Forwarded-For"].ToString().Split(';').FirstOrDefault();
                else
                    clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                if (clientIP == "::1") clientIP = "127.0.0.1";

                int portIndex = clientIP.IndexOf(":");
                if (portIndex > -1)
                    clientIP = clientIP.Substring(0, portIndex);

#endif

            return clientIP;
        }
    }
}