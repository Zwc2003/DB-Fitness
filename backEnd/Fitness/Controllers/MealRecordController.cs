using Fitness.BLL;
using Fitness.BLL.Interfaces;
using Fitness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MealRecordsController : ControllerBase
    {
        private readonly IMealRecordBLL _mealRecordBLL;

        public MealRecordsController(IMealRecordBLL mealRecordBLL)
        {
            _mealRecordBLL = mealRecordBLL;
        }

        // 创建一条饮食记录
        [HttpPost]
        public ActionResult<MealRecordRes> Create(MealRecordInfo mealRecordInfo)
        {
            return _mealRecordBLL.Create(mealRecordInfo);
        }

        // 根据userID获取所有饮食记录的缩略信息
        [HttpGet]
        public ActionResult<GetAllMealRecordsNoDetailsRes> GetAllNoDetails(int userID)
        {
            return _mealRecordBLL.GetAllNoDetails(userID);
        }

        // 根据userID获取所有饮食记录的详细信息
        [HttpGet]
        public ActionResult<GetAllMealRecordsDetailsRes> GetAllDetails(int userID)
        {
            return _mealRecordBLL.GetAllDetails(userID);
        }

        // 根据recordID获取单条饮食记录的详细信息
        [HttpGet]
        public ActionResult<GetOneMealRecordDetailsRes> GetOneDetail(int recordID)
        {
            return _mealRecordBLL.GetOneDetail(recordID);
        }

        // 根据recordID删除某条饮食记录
        [HttpGet]
        public ActionResult<MessageRes> Delete(int recordID)
        {
            return _mealRecordBLL.DeleteMealRecord(recordID);
        }
    }
}
