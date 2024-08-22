using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FitnessWebAPI.BLL.Interfaces;
using FitnessWebAPI.Models;

namespace Back_end_Web_API_chl.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        public UserController(IUserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        [HttpGet]
        public ActionResult<LoginToken> Login(string email,string password,string role)
        {
            return _userBLL.Login(email,password,role);
        }

        [HttpPost]
        public ActionResult<string> SendVerificationCode(string email)
        {
            return _userBLL.SendVerificationCode(email);
        }
        [HttpPost]
        public ActionResult<string> Register(RegisterInfo registerInfo)
        {
            return _userBLL.Register(registerInfo);
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

        [HttpGet]
        public ActionResult<User> GetProfile(string token,int userID)
        {
            return _userBLL.GetProfile(token, "others", userID);
        }

        [HttpPost]
        public ActionResult<string> UpdateProfile(string token, [FromBody]expandUserInfo userinfo)
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
    }
}
