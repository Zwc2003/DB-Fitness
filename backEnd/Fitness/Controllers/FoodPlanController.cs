using Fitness.BLL;
using Fitness.BLL.Interfaces;
using Fitness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MealPlansController : ControllerBase
    {
        private readonly IFoodPlanBLL _foodPlanBLL;

        public MealPlansController(IFoodPlanBLL foodPlanBLL)
        {
            _foodPlanBLL = foodPlanBLL;
        }

        // 创建一条饮食计划
        [HttpPost]
        public ActionResult<FoodPlanRes> Create(FoodPlanInfo foodPlanInfo)
        {
            return _foodPlanBLL.Create(foodPlanInfo);
        }

        // 根据userID获取所有饮食计划的缩略信息
        [HttpGet]
        public ActionResult<GetAllFoodPlanNoDetailsRes> GetAllNoDetails(int userID)
        {
            return _foodPlanBLL.GetAllNoDetails(userID);
        }

        // 根据userID获取所有饮食计划的详细信息
        [HttpGet]
        public ActionResult<GetAllFoodPlanDetailsRes> GetAllDetails(int userID)
        {
            return _foodPlanBLL.GetAllDetails(userID);
        }

        // 根据foodPlanID获取单条饮食计划的详细信息
        [HttpGet]
        public ActionResult<GetOneFoodPlanDetailRes> GetOneDetail(int foodPlanID)
        {
            return _foodPlanBLL.GetOneDetail(foodPlanID);
        }

        // 根据foodPlanID删除单条饮食计划
        [HttpDelete]
        public ActionResult<MessageRes> Delete(int foodPlanID)
        {
            return _foodPlanBLL.DeleteFoodPlan(foodPlanID);
        }

        // 更新饮食计划内容
        [HttpPut]
        public ActionResult<MessageRes> Update(UpdateFoodPlanInfo updateFoodPlanInfo)
        {
            return _foodPlanBLL.UpdateFoodPlan(updateFoodPlanInfo);
        }

        // 更新饮食计划的完成状态
        [HttpPut]
        public ActionResult<MessageRes> UpdateState(int foodPlanID, int state)
        {
            return _foodPlanBLL.UpdateFoodPlanState(foodPlanID,state);
        }

        // 获取食物数据库中的所有信息
        [HttpGet]
        public ActionResult<FoodsRes> GetFoodsInfo()
        {
            return _foodPlanBLL.GetALLFoodsInfo();
        }
    }
}
