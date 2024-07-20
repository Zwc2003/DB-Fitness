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
        private readonly IRecordAndPlanBLL _recordAndPlanBLL;

        private RecordAndPlanBLL a = new();

        [HttpPost]
        public ActionResult<string> Create(FoodPlanInfo foodPlanInfo)
        {
            return a.Create(foodPlanInfo);
        }


    }
}
