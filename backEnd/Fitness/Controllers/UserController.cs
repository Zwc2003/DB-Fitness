using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitness.BLL.Interfaces;
using Fitness.Models;
using Fitness.BLL;
using Fitness.BLL.Core;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly IVigorTokenBLL _vigorTokenBLL;
        private readonly JWTHelper _jwtHelper = new();

        public UserController(IUserBLL userBLL,IVigorTokenBLL vigorTokenBLL)
        {
            _userBLL = userBLL;
            _vigorTokenBLL = vigorTokenBLL;
        }

        [HttpGet]
        public ActionResult<LoginToken> Login(string email,string password,string role)
        {
            Console.WriteLine("登录");
            return _userBLL.Login(email,password,role);
        }

        [HttpPost]
        public ActionResult<string> Register(string role,string userName, string email, string password,string coachName)
        {
            return _userBLL.Register(role,userName,email,password,coachName);
        }
        [HttpGet]
        public ActionResult<string> RefreshToken(string oldToken)
        {
            return _userBLL.RefreshToken(oldToken);
        }

        [HttpGet]
        public ActionResult<User> GetPersonalProfile(string token) { 
            return _userBLL.GetProfile(token,"personal");
        }
        [HttpPost]
        public ActionResult<string> UpdateProfile(string token, expandUserInfo userinfo)
        {
            return _userBLL.UpdateProfile(token,userinfo);//前端不要修改userID字段，保持不变传回即可
        }
        [HttpGet]  
        public ActionResult<List<expandUserInfo>>  SearchUserByName(string token, string username)
        {
            return _userBLL.GetProfileByName(token, username);
        }

        //权限管理
        [HttpPost]
        public ActionResult<string> RemoveUser(string token,int userID)
        {
            return _userBLL.removeUser(token,userID);
        }

        [HttpPost]
        public ActionResult<string> BanPost(string token,int userID)
        {
            return _userBLL.banPost(token,userID);
        }

        // zwc 
        [HttpGet]
        public ActionResult<BalanceRes> GetVigorTokenBalance1(string token)
        {
            
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userID = tokenRes.userID;

            Console.WriteLine("获取活力币余额",userID);
            return _vigorTokenBLL.GetBalance(userID);
        }

        [HttpGet]
        public ActionResult<VigorTokenRecordList> GetVigorTokenReacords(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userID = tokenRes.userID;
            Console.WriteLine("获取活力币记录");
            return _vigorTokenBLL.GetAllVigorTokenRecords(userID);
        }
    }
}
