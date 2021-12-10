using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Users;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class BroadCastController : ControllerBase
    {

        

        public static class UserHandler
        {
            public static HashSet<string> ConnectedIds = new HashSet<string>();
        }

    
        public class ChatHub : Hub
        {

            public void GetMessage(string UserName, string Msg)
            {               
                Clients.All.SendAsync("SendMessage", UserName, Msg);
            }

            public void isConnect(string UserName)
           {
              
               Clients.All.SendAsync("ConnectUser", UserName +"加入");
            }


            public override Task OnConnectedAsync()
            {
                         
                UserHandler.ConnectedIds.Add(Context.ConnectionId);
                //return Clients.All.SendAsync("ConnectUser", ConnectUser + "加入");
                //return Clients.User(Context.ConnectionId, Context.ConnectionId);
                return base.OnConnectedAsync();
            }
            public override Task OnDisconnectedAsync(Exception exception)
            {
                   
                
                UserHandler.ConnectedIds.Remove(Context.ConnectionId);
                return Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId + "退出");
            }
           


        }



       
    }

}
