﻿using Fitness.BLL;
using Fitness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostBLL _postBLL = new PostBLL();

        // 发布帖子
        [HttpPost]
        public ActionResult<string> PublishPost(string token, [FromBody] Post post)
        {
            string result = _postBLL.Post(token, post);
            return result;
        }

        // 获取某个用户的所有帖子
        [HttpGet]
        public ActionResult<List<Post>> GetPostByUserID(string token,int userID)
        {
            List<Post> posts = _postBLL.GetPostByUserID(token,userID);
            return posts;
        }

        // 获取用户自己的所有帖子
        [HttpGet]
        public ActionResult<List<Post>> GetPersonalPost(string token)
        {
            List<Post> posts = _postBLL.GetPostByUserID(token, -1);
            return posts;
        }

        [HttpGet]
        public ActionResult<Post> GetPostByPostID(string token,int postID)
        {
            Post post = _postBLL.GetPostByPostID(token, postID);
            return post;
        }

        // 获取所有帖子
        [HttpGet]
        public ActionResult<List<Post>> GetAllPost(string token)
        {
            List<Post> posts = _postBLL.GetAllPost(token);
            return posts;
        }

        // 删除帖子
        [HttpDelete]
        public ActionResult<string> DeletePostByPostID(string token, int postID)
        {
            string result = _postBLL.Delete(token, postID);
            return result;
        }

        // 点赞帖子
        [HttpGet]
        public ActionResult<string> LikePost(string token, int postID)
        {
            string result = _postBLL.LikePost(token, postID);
            return result;
        }

        // 取消点赞帖子
        [HttpGet]
        public ActionResult<string> CancleLikePost(string token, int postID)
        {
            string result = _postBLL.CancleLike(token, postID);
            return result;
        }

        // 转发帖子
        [HttpGet]
        public ActionResult<string> ForwardPost(string token, int postID)
        {
            string result = _postBLL.Forward(token, postID);
            return result;
        }

        // 搜索帖子
        [HttpGet]
        public ActionResult<List<Post>> SearchPost(
            [FromHeader] string token,
            [FromQuery] string query,
            [FromQuery] string category = null,
            [FromQuery] string dateRange = null,
            [FromQuery] string sortBy = "postTime")
        {
            List<Post> posts = _postBLL.Search(token, query, category, dateRange, sortBy);
            return posts;
        }
    }
}
