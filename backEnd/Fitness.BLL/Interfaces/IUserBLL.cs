﻿using Fitness.Models;
using System.Web;

namespace Fitness.BLL.Interfaces
{

     public interface IUserBLL
     {
        //返回操作状态消息+token
        public LoginToken Login(string email, string password, string role);
        //返回操作状态信息
        public string Register(string role, string accountName, string email, string password, string coachName);

        public string RefreshToken(string oldToken);
        public User GetProfile(string token,string role,int userID= 999999);

        public string UpdateProfile(string token,expandUserInfo userinfo);
        public List<expandUserInfo> GetProfileByName(string token,string username);

        //权限管理
        public string removeUser(string token,int userID);

        public string banPost(string token,int userID);

    }
}