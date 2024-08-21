using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitness.BLL.Interfaces;
using Fitness.Models;
using Fitness.BLL;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly IVigorTokenBLL _vigorTokenBLL;

        public UserController(IUserBLL userBLL,IVigorTokenBLL vigorTokenBLL)
        {
            _userBLL = userBLL;
            _vigorTokenBLL = vigorTokenBLL;
        }

        [HttpGet]
        public ActionResult<LoginToken> Login(string email,string password,string role)
        {
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

        // zwc 待改成token
        [HttpGet]
        public ActionResult<BalanceRes> GetVigorTokenBalance(int userID)
        {
            return _vigorTokenBLL.GetBalance(userID);
        }
    }
}
