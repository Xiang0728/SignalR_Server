using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;



namespace Server.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class BroadCastController : ControllerBase
    {
       
        public class ChatHub : Hub
        {
            

            public Task SendMsg(GetUserName info)
            {
                
                if (info.isConnect == true)
                {
                   
                
                    return Clients.All.SendAsync("ReceiveMessage", info.UserName + "加入");
                   
                }
                else
                {  
                    return Clients.All.SendAsync("ReceiveMessage", info.UserName + "退出");
                }
                
            }

            


        }



        public class GetUserName
        {
            public string UserName { get; set; }
            public bool isConnect { get; set; }

        }
    }

}
