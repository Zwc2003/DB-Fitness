using Fitness.BLL.Core;
using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL
{
    public class PostBLL :IPostBLL
    {

        private readonly JWTHelper _jwtHelper = new();

        public string Post(string token, Post post)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            post.userID = tokenRes.userID;
            post.postID = 0;
            post.likesCount = 0;
            post.forwardCount = 0;
            post.postTime = DateTime.Now;
            post.commentsCount = 0;
            //post.refrencePostID = -1;
            int st;
            post.userName = UserDAL.GetUserByUserID(tokenRes.userID,out st).userName;
            int postId = PostDAL.Post(post);
            if (postId == 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    postID = postId,
                    message = "发帖失败"
                });
            }

            Publish publish = new Publish();
            publish.postID = postId;
            publish.userID = post.userID;
            publish.publishTime = post.postTime;
            //Console.WriteLine(publish.postID);
            PublishDAL.Post(publish);
            return JsonConvert.SerializeObject(new
            {
                postID = postId,
                message = "发帖成功"
            });
        }


        public Post GetPostByPostID(string token, int postID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            try
            {
                var post = PostDAL.GetPostByPostID(postID);
                return post;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching achievements for userID: {postID}: {ex.Message}");
                throw;
            }
        }

        public List<Post> GetPostByUserID(string token,int userID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userId = tokenRes.userID;
            if (userID != -1)
                userId = userID;
            try
            {
                DataTable result = PostDAL.GetByUserID(userId);
                if (result == null || result.Rows.Count == 0)
                {
                    return null;
                }
                var posts = new List<Post>();
                foreach (DataRow row in result.Rows)
                {
                    posts.Add(new Post ()
                    {
                        postID = Convert.ToInt32(row["postID"]),
                        userID = Convert.ToInt32(row["userID"]),
                        postTitle = row["postTitle"].ToString(),
                        postContent = row["postContent"].ToString(),
                        postCategory = row["postCategory"].ToString(),
                        postTime = Convert.ToDateTime(row["postTime"]),
                        likesCount = Convert.ToInt32(row["likesCount"]),
                        forwardCount = Convert.ToInt32(row["forwardCount"]),
                        commentsCount = Convert.ToInt32(row["commentsCount"]),
                        userName = row["userName"].ToString()
                    });
                }

                return posts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching achievements for userID: {userId}: {ex.Message}");
                throw;
            }
        }

        public List<Post> GetAllPost(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            try
            {
                DataTable result = PostDAL.GetAll();
                if (result == null || result.Rows.Count == 0)
                {
                    return null;
                } 
                var posts = new List<Post>();
                foreach (DataRow row in result.Rows)
                {
                    posts.Add(new Post()
                    {
                        postID = Convert.ToInt32(row["postID"]),
                        userID = Convert.ToInt32(row["userID"]),
                        postTitle = row["postTitle"].ToString(),
                        postContent = row["postContent"].ToString(),
                        postCategory = row["postCategory"].ToString(),
                        postTime = Convert.ToDateTime(row["postTime"]),
                        likesCount = Convert.ToInt32(row["likesCount"]),
                        forwardCount = Convert.ToInt32(row["forwardCount"]),
                        commentsCount = Convert.ToInt32(row["commentsCount"]),
                        userName = row["userName"].ToString()
                    });
                }

                return posts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        //var results = postBLL.Search(token, "关键字", category: "技术", dateRange: "2024-01-01,2024-08-01", sortBy: "postTime");
        public List<Post> Search(string token, string query, string category = null, string dateRange = null, string sortBy = "postTime")
        {
            // 验证Token
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (tokenRes == null)
            {
                throw new UnauthorizedAccessException("无效的Token");
            }
            // 调用DAL层的Search方法
            return PostDAL.Search(query, category, dateRange, sortBy);
        }


        public string Delete(string token,int postID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int hostId = PublishDAL.GetHost(postID);
        /*  if (hostId == -1)
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "删帖失败：该帖子不存在"
                });
            }*/
            int userId = tokenRes.userID;
            string role =tokenRes.Role;
            if (hostId != userId && role!="admin")
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "身份权限不符"
                });
            }
            PublishDAL.Delete(postID);
            PostDAL.Delete(postID);
            return JsonConvert.SerializeObject(new
            {
                message = "删除帖子成功"
            });
        }

        public string LikePost(string token ,int postID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (PostDAL.Like(postID))
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "成功点赞帖子"
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "点赞帖子失败"
                });
            }
            //todo
            //通知被点赞方，接入成就系统
        }

        public string CancleLike(string token, int postID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (PostDAL.CancleLike(postID))
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "成功取消点赞"
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "取消点赞失败"
                });
            }
        }

        public string Forward(string token, int postId)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            //Post post = PostDAL.GetByPostID(postId);
            var post = PostDAL.GetPostByPostID(postId);
            int userId = tokenRes.userID;
            int st;
            PostDAL.Forward(postId);
            Post post1 = new Post() {
                postTitle =post.postTitle,
                postContent = post.postContent,
                postCategory = post.postCategory,
                userID =userId, 
                refrencePostID = post.postID,
                userName =UserDAL.GetUserByUserID(userId,out st).userName
            };
            Post(token,post1);
            return JsonConvert.SerializeObject(new
            {
                message = "成功转发"
            });
        }
    }
}
