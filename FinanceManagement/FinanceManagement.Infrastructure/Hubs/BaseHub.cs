using FinanceManagement.Infrastructure.Attributes;
using FinanceManagement.Infrastructure.Dto.Auth;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Hubs
{

    [BaseAuthorize]
    public class BaseHub<T> : Hub<T> where T : class
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await base.OnConnectedAsync();

            Log.Logger.Information("Client connected {UserId} {ConnectionId}", userId, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            var userId = Context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.UserId).Value;

            if (exception == null)
            {
                Log.Logger.Information("Client disconnected {UserId} {ConnectionId}", userId, Context.ConnectionId);
            }
            else
            {
                Log.Logger.Error(exception, "Client was disconnected {UserId} {ConnectionId}", userId, Context.ConnectionId);
            }
        }
    }
}
