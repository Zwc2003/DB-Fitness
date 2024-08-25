using Fitness.Models;
using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.BLL.Core;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Newtonsoft.Json;
using System.Net;

namespace Fitness.BLL
{
        public class UserBLL : IUserBLL
        {
            private readonly VerificationHelper _verificationHelper;

            public UserBLL(VerificationHelper verificationHelper)
            {
                _verificationHelper = verificationHelper;
            }
            public string Register(RegisterInfo registerInfo)
            {
                int status;
                var flag = UserDAL.IsEmailRegistered(registerInfo.email);

                if (flag) return JsonConvert.SerializeObject(new
                {
                    message = "注册失败：邮箱已存在"
                });

                if (!_verificationHelper.ValidateVerificationCode(registerInfo.email, registerInfo.verificationCode))
                {
                    return JsonConvert.SerializeObject(new
                    {
                        message = "验证码错误或已过期"
                    });
                }
                registerInfo.coachName = "null";
                // 对密码进行加密
                string salt;
                string hashedPassword = PasswordHelper.HashPassword(registerInfo.password, out salt);
                //先统一向User表中插入一条
                User user = new User(registerInfo.accountName, registerInfo.email, hashedPassword, salt);
                int res = UserDAL.Insert(user);
                if (res == -1)
                    return JsonConvert.SerializeObject(new
                    {
                        message = "注册失败：插入User表失败！"
                    });
                if (registerInfo.role == "coach")
                {
                    Coach coach = new Coach(res, user.userName, user.Age, user.Gender, user.iconURL, user.isMember, registerInfo.coachName);
                    CoachDAL.Insert(coach);
                }
                _verificationHelper.RemoveVerificationCode(registerInfo.email);
            int st;
            var vigorTokenBLL = new VigorTokenBLL();
            vigorTokenBLL.UpdateBalance(UserDAL.GetLoginInfoByEmail(registerInfo.email,out st).userID, "注册FitFit成功，获得2000活力币", 2000);
                return JsonConvert.SerializeObject(new
                {
                    message = "成功注册"
                });
            }
            public string SendVerificationCode(string email)
            {
                var code = _verificationHelper.GenerateVerificationCode();
                _verificationHelper.StoreVerificationCode(email, code);

                // 调用发送邮件的逻辑
                _verificationHelper.SendVerificationEmail(email, code);

                return JsonConvert.SerializeObject(new
                {
                    message = "验证码已发送"
                });
            }

            private readonly JWTHelper _jwtHelper = new();
            public LoginToken Login(string email, string password, string role)
            {
                int status;
                LoginInfo loginInfo = UserDAL.GetLoginInfoByEmail(email, out status);
                if (status == 2) { return new LoginToken("InvalidToken", "邮箱不存在或错误"); }
                if (status == 1) return new LoginToken("Invalid", "Internal server error. Please try again later.");
                bool isPass = PasswordHelper.VerifyPassword(password, loginInfo.hashedPassword, loginInfo.Salt);
            if (isPass) {
                var res = new LoginToken(_jwtHelper.GenerateToken(loginInfo.userID, role), "登录成功");
                int userID = _jwtHelper.ValidateToken(res.token).userID;
                DateTime dt_last = UserDAL.GetLastLoginTime(userID);
                DateTime dt_now = DateTime.Now;
                UserDAL.UpdateLoginTime(userID,dt_now);
                //活力币+1
                //判断是否首次登录系统
                bool isSameDate = dt_last.Date == dt_now.Date;
                if (!isSameDate)
                {
                    var vigorTokenBLL = new VigorTokenBLL();
                    vigorTokenBLL.UpdateBalance(userID, $"本日{dt_now.Date}首次登录系统,获得50活力币", 50);
                }
                return res;
            }
                return new LoginToken("InvalidToken", "密码错误");
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

            public User GetProfile(string token, string role, int userID = -1)
            {
                int st;
                //可以添加一些出错的处理：方便前后端对接出错时处理
                TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
                if (role == "personal")
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

                return "更新成功";
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

            public expandUserInfo GetProfileByUserID(string token, int userID)
            {
                int st;
                TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
                //！！！补充：未查到的信息返回
                var res = UserDAL.GetUserByUserID(userID, out st);
                var result = new expandUserInfo(userID, res.userName, res.iconURL, res.Age, res.Gender, res.Tags, res.Introduction, res.isMember, res.goalType, res.goalWeight);
                if (st == 2)
                    return null;
                else return result;
            }

            public List<expandUserInfo> GetAllUser(string token)
            {
                int st;
                TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
                //返回一些基本的信息：ID,头像，昵称，年龄，性别，粉丝数 
                //！！！补充：未查到的信息返回
                var res = UserDAL.GetAllUser(out st);
                if (st == 2)
                    return null;
                else return res;
            }

            //权限管理
            public string removeUser(string token, int userID)
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

            public string banPost(string token, int userID) //可以设置禁止发帖的天数
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