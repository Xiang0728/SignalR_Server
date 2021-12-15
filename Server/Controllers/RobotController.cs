using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("GetPrice")]
        public async Task<IActionResult> GetPrice(Price.Code code)
        {
            string[] Msg = new string[3];//回傳前端資料
            System.DateTime currentTime = new System.DateTime();
            string time = currentTime.ToString("t");
            Msg[2] = time;
            var CodeName = code.number;
            var url = "https://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch=tse_" + CodeName + ".tw&json=1&delay=0";
            try
            {
               
                using (var client = new HttpClient())
                {
                    var res = client.GetAsync(url).Result;
                    if (res.IsSuccessStatusCode)
                    {                      
                       var result = await res.Content.ReadAsStringAsync();

                        
                        var obj = JsonConvert.DeserializeObject<Price.Root>(result);
                        if (obj==null||obj.msgArray[0].a == null || obj.msgArray[0].b == null)
                        {
                            Msg[0] = "查無資料";
                            return Ok(Msg);
                        }
                        else
                        {
                            var ArraySell = obj.msgArray[0].a.Split("_");
                            var ArrayBuy = obj.msgArray[0].b.Split("_");
                            var Sell = ArraySell[0];
                            var Buy = ArrayBuy[0];
                            double BasePrice = double.Parse(obj.msgArray[0].y);
                            double NowPrice;
                            if (obj.msgArray[0].z == "-")
                            {
                                NowPrice = double.Parse(Sell);
                            }
                            else
                            {
                                NowPrice = double.Parse(obj.msgArray[0].z);
                            }
                            double Spread = Math.Round(NowPrice - BasePrice, 2, MidpointRounding.AwayFromZero);
                            if (Spread > 0)
                            {
                                Msg[1] = "Red";

                            }
                            else if (Spread < 0)
                            {
                                Msg[1] = "Green";
                            }
                            else
                            {
                                Msg[1] = "Blue";
                            }

                            //"Low : " + ArrayBuy[0] + " High : " + ArraySell[0];
                            Msg[0] = NowPrice + " " + Spread;

                            return Ok(Msg);
                        }
                    }
                    else
                    {
                        Msg[0] = "查無資料";
                        return Ok(Msg);
                    }
                }

            }


            catch (Exception ex)
            {
                Msg[0] = "查無資料";
                return Ok(Msg);
            }

        }
    }
}
