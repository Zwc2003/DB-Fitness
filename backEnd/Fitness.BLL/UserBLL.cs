using Fitness.Models;
using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.BLL.Core;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace Fitness.BLL
{
    public class UserBLL : IUserBLL
    {
        private readonly JWTHelper _jwtHelper = new();
        public LoginToken Login(string email, string password, string role)
        {
            int status;
            LoginInfo loginInfo = UserDAL.GetLoginInfoByEmail(email, out status);
            if (status == 2) { return new LoginToken("InvalidToken", "Invalid Email."); }
            if (status == 1) return new LoginToken("Invalid", "Internal server error. Please try again later.");
            bool isPass = PasswordHelper.VerifyPassword(password, loginInfo.hashedPassword, loginInfo.Salt);
            if (isPass) return new LoginToken(_jwtHelper.GenerateToken(loginInfo.userID, role), "Login successful!");
            return new LoginToken("InvalidToken", "Invalid Password.");
        }

        public string RefreshToken(string oldToken)
        {
            string newToken = null;
            bool isValid;

            // 验证现有的 JWT
            TokenValidationResult validateRes = _jwtHelper.ValidateToken(oldToken);
            if (validateRes.IsValid == true)
            {
                // 从现有 JWT 中提取用户信息
                var userId = validateRes.userID;
                var role = validateRes.Role;

                // 生成新的 JWT
                newToken = _jwtHelper.GenerateToken(userId, role);
                return newToken;
            }
            return "InValid";
        }

        public string Register(string role,string accountName, string email, string password,string coachName)
        {
            int status;
            var flag = UserDAL.IsEmailRegistered(email);

            if (flag) return "Email already exists！";

            // 对密码进行加密
            string salt;
            string hashedPassword = PasswordHelper.HashPassword(password, out salt);
            //先统一向User表中插入一条
            User user = new User(accountName, email, hashedPassword, salt);
            int res=UserDAL.Insert(user);

            //！！！记得同步更新FoodPlan表
            if(res == -1)
                return "注册失败：插入User表失败！";
            if (role == "coach") { 
                Coach coach = new Coach(res,user.userName,user.Age,user.Gender,user.iconURL,user.isMember,coachName);
                CoachDAL.Insert(coach);
            }
            return "Successful registration!";

        }

        public User GetProfile(string token, string role,int userID =-1)
        {
            int st;
            //可以添加一些出错的处理：方便前后端对接出错时处理
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if(role =="personal")
                return UserDAL.GetUserByUserID(tokenRes.userID, out st);
            else// "others"
            {
                return UserDAL.GetUserByUserID(userID, out st);
            }
        }

        public string UpdateProfile(string token, expandUserInfo userinfo)
        {
            
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int st;
            //可以再利用st添加一些出错的处理：方便前后端对接出错时处理
            UserDAL.UpdateExpandUserInfo(tokenRes.userID, userinfo, out st);
            
            //！！！记得同步更新FoodPlan表

            return "Profile updated successfully.";
        }

        public List<expandUserInfo> GetProfileByName(string token, string searchname)
        {
            int st;
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            //返回一些基本的信息：ID,头像，昵称，年龄，性别，粉丝数 
            //！！！补充：未查到的信息返回
            var res = UserDAL.GetUserByUserName(searchname, out st);
            if (st == 2)
                return null;
            else return res;
        }

        //权限管理
        public string removeUser(string token,int userID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (tokenRes.Role == "admin")
            {
                UserDAL.SetIsDelete(userID, 1);
                return "Remove current user sucessfully.";
            }
            else
                return "You don't have that permission!";
        }

        public string banPost(string token,int userID) //可以设置禁止发帖的天数
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (tokenRes.Role == "admin")
            {
                UserDAL.SetIsPost(userID, 0);
                return "Successfully disabled the posting rights of the current user.";
            }
            else
                return "You don't have that permission!";
        }
    }
}