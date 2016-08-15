#region

using System.Web.Http;

#endregion

namespace device.client.web.controllers
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}