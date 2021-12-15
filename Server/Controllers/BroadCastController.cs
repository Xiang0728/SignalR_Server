using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using static Users;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class BroadCastController : ControllerBase
    {

        public static class UserHandler
        {            
            public static Dictionary<string, string> ConnectedIds = new Dictionary<string, string>();
        }

    
        public class ChatHub : Hub
        {
        
            public void GetMessage(string UserName, string Msg)
            {
                DateTime currentTime = new System.DateTime();
                string time = currentTime.ToString("t");
                Clients.All.SendAsync("SendMessage", UserName, Msg,time);
            }

            public void UserConnected(string name)
            {
                DateTime currentTime = new System.DateTime();
                string time = currentTime.ToString("t");

                string message = "系統 : 歡迎 " + name + " 加入聊天室 ";
                string successMsg = "系統 : 連線成功 ";
                Clients.AllExcept(Context.ConnectionId).SendAsync("NewConnect", message,time);
                Clients.Client(Context.ConnectionId).SendAsync("NewConnect",successMsg, time);
                
                UserHandler.ConnectedIds.Add(Context.ConnectionId, name);
            }
           
            public override Task OnConnectedAsync()
            {
                         
                //UserHandler.ConnectedIds.Add(Context.ConnectionId);              
                return base.OnConnectedAsync();
            }
            public override Task OnDisconnectedAsync(Exception exception)
            {
                DateTime currentTime = new System.DateTime();
                string time = currentTime.ToString("t");

                string message = "系統 : " + UserHandler.ConnectedIds[Context.ConnectionId] + " 離開聊天室 ";
                Clients.AllExcept(Context.ConnectionId).SendAsync("NewConnect", message, time);
                UserHandler.ConnectedIds.Remove(Context.ConnectionId);
                return base.OnDisconnectedAsync(exception);
            }
           


        }



       
    }

}
