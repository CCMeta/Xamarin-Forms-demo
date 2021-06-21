using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Services
{
    public class ChatHubUserProvider: IUserIdProvider
    {
        public ChatHubUserProvider()
        {
        }

        public virtual string GetUserId(HubConnectionContext connection)
        {
            var context = GetHttpContextExtensions.GetHttpContext(connection);
            Console.WriteLine(context.Request.QueryString);
            Console.WriteLine(connection.ConnectionId);
            Console.WriteLine(connection.Features.Count());
            Console.WriteLine(connection.Items.Count());
            return "myuserid";
            //return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
