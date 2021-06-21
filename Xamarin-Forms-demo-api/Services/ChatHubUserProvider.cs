using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Services
{
    public class ChatHubUserProvider: IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return "myuserid";
            //return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
