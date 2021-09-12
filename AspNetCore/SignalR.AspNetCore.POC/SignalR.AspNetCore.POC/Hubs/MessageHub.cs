using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalR.AspNetCore.POC.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub
    {
        // Envia mensagem para todos
        public Task SendMessageToAll(string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Envia mensagem para sí próprio.
        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        // Envia mensagem para um usuário especifico
        //public Task SendMessageToUser(string connectionId, string message)
        //{
        //    return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        //}

        // Envia mensagem para um usuário especifico
        public Task SendMessageToUser(string userId, string message)
        {
            return Clients.User(userId).SendAsync("ReceiveMessage", message);
        }

        // Adiciona ao grupo
        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        // Envia mensagem para o grupo
        public Task SendMessageToGroup(string group, string message)
        {
            return Clients.Group(group).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            ClaimsIdentity identity = (ClaimsIdentity)(Context.User.Identity!);
            string author = identity.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;

            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}