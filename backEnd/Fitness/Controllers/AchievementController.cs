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
    public class AchievementController : ControllerBase
    {
        private readonly JWTHelper _jwtHelper = new();

        [HttpGet]
        public string GetAchievement(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserAchievementBll.GetUserAchievement(userId);
        }

        [HttpGet]
        public string GetAchievementRanking(string token, int achievementId)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserAchievementBll.GetAchievementRank(userId, achievementId);
        }

        [HttpPost]
        public string UpdateFitnessPlanAchievement(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            return UserAchievementBll.UpdateFitnessPlanAchievement(userId);
        }
    }
}
