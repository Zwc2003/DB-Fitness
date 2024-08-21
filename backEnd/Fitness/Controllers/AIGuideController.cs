using Fitness.BLL;
using Fitness.BLL.Interfaces;
using Fitness.Models;
using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AIGuideController : ControllerBase
    {
        private readonly IFitnessALGuideBLL _fitnessALGuideBLL;
        private readonly IVigorTokenBLL _vigorTokenBLL;

        public AIGuideController(IFitnessALGuideBLL fitnessALGuideBLL, IVigorTokenBLL vigorTokenBLL)
        {
            _fitnessALGuideBLL = fitnessALGuideBLL;
            _vigorTokenBLL = vigorTokenBLL;
        }

        // 上传健身截图创建记录
        [HttpPost]
        public ActionResult<ScreenshotRes> Create(ScreenshotInfo screenshotInfo)
        {
            Console.WriteLine($"健身截图创建");
            return _fitnessALGuideBLL.Create(screenshotInfo);
        }


        // 获取所有健身截图记录
        [HttpGet]
        public ActionResult<SuggestionDetailRes> GetAllDetails(int userID)
        {
            Console.WriteLine($"获取所有健身截图记录");
            return _fitnessALGuideBLL.GetAllDetails(userID);
        }


        // 获取AI对健身截图的建议
        [HttpGet]
        public ActionResult<MessageRes> GetAISuggestion(int screenshotID)
        {
            Console.WriteLine($"获取AI建议{screenshotID}");
            return _fitnessALGuideBLL.GetAISuggestion(screenshotID);
        }


        // 根据screenshotID删除单条健身动作记录
        [HttpDelete]
        public ActionResult<MessageRes> Delete(int screenshotID)
        {
            Console.WriteLine($"删除健身动作截图记录{screenshotID}");
            return _fitnessALGuideBLL.DeleteFitnessSuggestion(screenshotID);
        }

        // 获取健身器材使用方法
        [HttpGet]
        public ActionResult<SingleDetailEqui> GetEquipmentGuide(string equipmentName)
        {
            Console.WriteLine($"获取健身器材详情{equipmentName}");
            return _fitnessALGuideBLL.GetOneEquiGuide(equipmentName);
        }

        // 获取所有健身器材的缩略使用方法
        [HttpGet]
        public ActionResult<AllEquiGuidesRes> GetALLEquipmentGuide()
        {
            Console.WriteLine($"获取所有健身器材的信息");
            return _fitnessALGuideBLL.GetALLEquiGuides();
        }

        // 获取所有健身器材的缩略使用方法
        [HttpGet]
        public ActionResult<RandomEquiGuidesRes> GetRandomEquipmentGuide()
        {
            Console.WriteLine($"随机获取7种健身器材的缩略信息");
            return _fitnessALGuideBLL.GetRandomEquiGuides();
        }
        
        // 增
        [HttpPost]
        public ActionResult<FitnessEquipmentRes> InsertEquipmentGuide(FitnessEquipment fitnessEquipment)
        {
            Console.WriteLine($"插入健身器材记录{fitnessEquipment.equipmentName}");
            return _fitnessALGuideBLL.InsertEquipment(fitnessEquipment);
        }

        // 删
        [HttpDelete]
        public ActionResult<MessageRes> DeleteEquipmentGuide(string equipmentName)
        {
            Console.WriteLine($"删除健身器材记录{equipmentName}");
            return _fitnessALGuideBLL.DeleteFitnessEquipment(equipmentName);
        }

        // 改
        [HttpPut]
        public ActionResult<MessageRes> UpdateEquipmentGuide(FitnessEquipment fitnessEquipment)
        {
            Console.WriteLine($"更新健身器材记录{fitnessEquipment.equipmentName}");
            return _fitnessALGuideBLL.UpdateFitnessEquipment(fitnessEquipment);
        }



        // 修改后的AI智能问答API
        [HttpGet]
        public async Task LLM(string equipmentName,string messages)
        {
            Console.WriteLine($"AI问答{equipmentName}");

            // 解析messages为List<AIMessage>
            var messageList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AIMessage>>(messages);

            // 设置响应头，开启流式传输
            Response.ContentType = "text/event-stream";
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            AIMessage message = new()
            {
                role = "system",
                content = "你是一个资深的健身教练，负责向学员讲解各种健身器材的使用方法和注意事项，你最熟悉的健身器材是" + equipmentName
            };

            AIInfo info = new()
            {
                equipmentName = equipmentName,
                messages = messageList
            };

            info.messages.Insert(0, message);

            // 调用多次生成AI的响应内容
            await foreach (var chunk in _fitnessALGuideBLL.EquipmentMultiAIDialogStream(info))
            {
                

                Console.WriteLine($"Sending chunk: {chunk}");
                await Response.WriteAsync($"data:{chunk}\n\n");
                await Response.Body.FlushAsync();  // 确保每个块立即发送到前端
            }

            Console.WriteLine("Sending [DONE]");
            await Response.WriteAsync("data:[DONE]\n\n");
            await Response.Body.FlushAsync();


        }


    }
}
