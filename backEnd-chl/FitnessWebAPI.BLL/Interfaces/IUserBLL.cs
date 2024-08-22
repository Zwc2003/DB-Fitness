using FitnessWebAPI.Models;
using System.Web;

namespace FitnessWebAPI.BLL.Interfaces
{

     public interface IUserBLL
     {
        //返回操作状态消息+token
        public LoginToken Login(string email, string password, string role);
        //返回操作状态信息
        public string Register(RegisterInfo registerInfo);
        public string SendVerificationCode(string email);
        public string RefreshToken(string oldToken);
        public User GetProfile(string token,string role,int userID= 999999);

        public string UpdateProfile(string token,expandUserInfo userinfo);
        public List<expandUserInfo> GetProfileByName(string token,string username);

        //权限管理
        public string removeUser(string token,int userID);

        public string banPost(string token,int userID);

    }
}
