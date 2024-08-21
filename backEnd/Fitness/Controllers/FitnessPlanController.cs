using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Fitness.DAL;
using Fitness.BLL;
using Fitness.Models;
using Fitness.BLL.Core;
using Fitness.BLL.Interfaces;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FitnessPlanController : ControllerBase
    {
        private readonly JWTHelper _jwtHelper = new();
        private readonly IVigorTokenBLL _vigorTokenBLL;

        [HttpPost]
        public string PostFitness(string token, double height, double weight, double BMI, double bodyFatRate)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return FitnessBll.Update(userId, height, weight, BMI, bodyFatRate);
        }

        [HttpPost]
        public string PostPhysicalTest(string token, int pushups, int squats, int situps, int pullups, int longDistance)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return PhysicalTestBll.Update(userId, pushups, squats, situps, pullups, longDistance);
        }

        [HttpGet]
        public string GetFitness(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return FitnessBll.Get(userId);
        }

        [HttpGet]
        public string GetPhysicalTest(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return PhysicalTestBll.Get(userId);
        }

        [HttpPost]
        public string SetGoal(string token, string goal, int duration)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserFitnessPlanGoalBll.Set(userId, goal, duration);
        }

        [HttpGet]
        public string GetGoal(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserFitnessPlanGoalBll.Get(userId);
        }

        [HttpGet]
        public string GetPlan(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return WorkoutBll.GetPlan(userId);
        }
    }
}
