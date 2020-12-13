using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Services
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SessionService _sessionService;

        public AuthMiddleware(RequestDelegate next, SessionService sessionService)
        {
            _sessionService = sessionService;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //context.Request.Headers.Add("TOKEN", "a");
            if (context.Request.Headers.TryGetValue("TOKEN", out var token))
                if (_sessionService.Sessions.TryGetValue(token, out int uid))
                    context.Items["uid"] = uid;

            await _next(context);
        }
    }
}
