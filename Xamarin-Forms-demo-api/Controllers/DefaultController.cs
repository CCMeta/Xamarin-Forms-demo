using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Xamarin_Forms_demo_api.Controllers
{
    public abstract class DefaultController : ControllerBase
    {
        protected readonly int _uid;

        protected DefaultController(IHttpContextAccessor context)
        {
            if (context.HttpContext.Items.TryGetValue("uid", out var uid))
            {
                _uid = (int)uid;
                return;
            }
        }

    }
}
