using System.Reflection.Emit;

namespace Fitness.Models
{
    public class LoginToken 
    { 
        public string token { get; set; }
        public string message { get; set;}

        public LoginToken(string _token,string _message) 
        {
            token = _token;
            message = _message;
        }
    }

    public class LoginInfo
    {
        public int userID { get; set; }
        public string hashedPassword { get; set; }
        public string Salt { get; set; }

        public LoginInfo(int userid,string hashedpwd,string salt){
            userID=userid;
            hashedPassword = hashedpwd;
            Salt = salt;
        }

    }


    public class User
    {
        //必须字段：基本信息
        public int userID { get; set; }

        public string userName { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Email { get; set; }

        public DateTime registrationTime { get; set; }= DateTime.Now;

        //拓展信息：个人信息补充，允许为空
        public string iconURL { get; set; } = "null"; //这里可以设置一个默认头像的URL
        public int Age { get; set; } = -1;
        public string Gender { get; set; } = "null";
        public string Tags { get; set; } = "null";
        public string Introduction { get; set; } = "null";

        // 目标类型
        public string goalType { get; set; } = "null";

        // 目标体重
        public float goalWeight { get; set; } = -1;

        public int isMember { get; set; } = 0;

        //用户账户状态信息
        public int isPost { get; set; } = 1;
        public int isDelete { get; set; }= 0;

        //仅供测试：基础构造函数
        public User(string name,string email,string password,string salt) { 
            userName = name;
            Email = email;
            Password = password;
            Salt = salt;
        }

        /*// 使用 basicUserInfo 和 expandUserInfo 初始化 User 实例的构造函数
        public User(basicUserInfo basicInfo, expandUserInfo expandInfo)
        {
            // 基本信息
            userID = basicInfo.userID;
            Password = basicInfo.Password;
            Email = basicInfo.Email;
            registrationTime = basicInfo.registrationTime;
            isPost = basicInfo.isPost ;
            isDelete = basicInfo.isDelete;
            Salt = basicInfo.Salt; 

            // 拓展信息
            userName = expandInfo.userName;
            iconURL = expandInfo.iconURL;
            Age = expandInfo.Age;
            Gender = expandInfo.Gender;
            Tags = expandInfo.Tags;
            Introduction = expandInfo.Introduction;
            isMember = expandInfo.isMember;
        }*/
    }

    public class Trainee
    {
        public int traineeID { get; set; }

        // 学员的用户名称
        public string userName { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string iconURL { get; set; }

        // 学员的真实姓名
        public string traineeName { get; set; } = "null";

        public Trainee(int traineeID, string userName, int age, string gender, string iconURL, string traineeName)
        {
            this.traineeID = traineeID;
            this.userName = userName;
            Age = age;
            Gender = gender;
            this.iconURL = iconURL;
            this.traineeName = traineeName;
        }
    }

    public class Coach
    {
        public int coachID { get; set; }

        //教练的用户名称
        public string userName { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string iconURL { get; set; }

        public int isMember { get; set; }

        //教练的真实名称
        public string coachName { get; set; }

        public Coach(int coachID, string userName, int age, string gender, string iconURL, int isMember, string coachName)
        {
            this.coachID = coachID;
            this.userName = userName;
            Age = age;
            Gender = gender;
            this.iconURL = iconURL;
            this.isMember = isMember;
            this.coachName = coachName;
        }

    }

    //不可由用户端的API请求发起修改，基本不变，暂时不支持修改密码和邮箱操作
    public class basicUserInfo
    {
        public int userID { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Email { get; set; }

        public DateTime registrationTime { get; set; } = DateTime.Now;

        //用户账户状态信息
        public int isPost { get; set; } = 1;
        public int isDelete { get; set; } = 0;

    }

    public class expandUserInfo
    {
        public int userID { get; set; }
        public string userName { get; set; }

        public string? iconURL { get; set; } //这里可以设置一个默认头像的URL

        public int? Age { get; set; } = -1;

        public string? Gender { get; set; } = "null";

        public string? Tags { get; set; } = "null";

        public string? Introduction { get; set; } = "null";

        public int isMember { get; set; } = 0;

        // 目标类型
        public string goalType { get; set; } = "null";
        // 目标体重
        public float goalWeight { get; set; } = -1;
        public expandUserInfo(int userId, string userName, string? iconURL, int? age, string? gender, string? tags, string? introduction, int isMember,string goaltype,float goalweight)
        {
            userID = userId;
            this.userName = userName;
            this.iconURL = iconURL;
            Age = age;
            Gender = gender;
            Tags = tags;
            Introduction = introduction;
            this.isMember = isMember;
            goalType = goaltype;
            goalWeight = goalweight;   
        }
    }


}