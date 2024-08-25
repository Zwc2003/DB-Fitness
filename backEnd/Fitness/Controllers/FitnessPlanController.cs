using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Fitness.DAL;
using Fitness.BLL;
using Fitness.Models;
using Fitness.BLL.Core;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FitnessPlanController : ControllerBase
    {
        private JWTHelper _jwtHelper = new();
        [HttpGet]
        public string PostFitness(string token, double height, double weight, double BMI, double bodyFatRate)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return FitnessBLL.Update(userId, height, weight, BMI, bodyFatRate);
        }

        [HttpGet]
        public string PostPhysicalTest(string token, int pushups, int squats, int situps, int pullup, int longDistance)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return PhysicalTestBLL.Update(userId, pushup, squats, situps, pullups, longDistance);
        }

        [HttpGet]
        public string GetFitness(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return FitnessBLL.Get(userId);
        }

        [HttpGet]
        public string GetPhysicalTest(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return PhysicalTestBLL.Get(userId);
        }

        [HttpGet]
        public string SetGoal(string token, string goal, int duration)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserFitnessPlanGoalBLL.Set(userId, goal, duration);
        }

        [HttpGet]
        public string GetGoal(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserFitnessPlanGoalBLL.Get(userId);
        }

        [HttpGet]
        public string GetPlan(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return WorkoutBLL.GetPlan(userId);
        }
    }
}
