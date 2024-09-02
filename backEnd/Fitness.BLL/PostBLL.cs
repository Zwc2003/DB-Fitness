using Fitness.BLL.Core;
using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.DAL.Core;
using Fitness.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fitness.BLL
{
    public class PostBLL :IPostBLL
    {

        private readonly JWTHelper _jwtHelper = new();
        private readonly UserAchievementBLL _userAchievement = new UserAchievementBLL();
/*        private readonly PostBLL postbll = new PostBLL();*/
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
            
            // 首先要判断是否已经为url
            if (post.imgUrl != "null" && !UrlHelper.IsUrl(post.imgUrl)) {

                long timestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond;
                string objectName = $"{post.userID}_{timestamp}.png";
                int base64Index = post.imgUrl.IndexOf("base64,") + 7;
                post.imgUrl = post.imgUrl.Substring(base64Index);
                OSSHelper.UploadBase64ImageToOss(post.imgUrl, objectName);
                post.imgUrl = OSSHelper.GetPublicObjectUrl(objectName);

            }

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

            PublishDAL.Post(publish);
            //更新成就
            _userAchievement.UpdatePostAchievement(tokenRes.userID, 1);
            //插入一条AI评论


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
                        userName = row["userName"].ToString(),
                        imgUrl = row["imgUrl"].ToString()
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
                        userName = row["userName"].ToString(),
                        imgUrl = row["imgUrl"].ToString()
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


        public string Delete(string token,int postID, int postUserID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int hostId = PublishDAL.GetHost(postID);
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
            //更新成就
            _userAchievement.UpdatePostAchievement(postUserID, -1);
            return JsonConvert.SerializeObject(new
            {
                message = "删除帖子成功"
            });
        }

        public string LikePost(string token ,int postID,int postUserID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (PostDAL.Like(postID))
            {
                //通知被点赞方，接入成就系统
                _userAchievement.UpdateBeLiked(postUserID,1);
                var post = PostDAL.GetPostByPostID(postID);
                Message message = new Message()
                {
                    messageID = -1,
                    senderID = tokenRes.userID,
                    receiverID = postUserID,
                    messageType = "点赞帖子",
                    Content = $"{post.postTitle}",
                    sendTime = DateTime.Now,
                    isRead = 0
                };
                MessageDAL.Insert(message);

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
        }

        public string CancleLike(string token, int postID, int postUserID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (PostDAL.CancleLike(postID))
            {
                _userAchievement.UpdateBeLiked(postUserID, -1);
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
                userName =UserDAL.GetUserByUserID(userId,out st).userName,
                imgUrl = post.imgUrl
            };
            Post(token,post1);
            //通知
            Message message = new Message()
            {
                messageID = -1,
                senderID = userId,
                receiverID = post.userID,
                messageType = "转发",
                Content = $"{post.postTitle}",
                sendTime = DateTime.Now,
                isRead = 0
            };
            MessageDAL.Insert(message);
            return JsonConvert.SerializeObject(new
            {
                message = "成功转发"
            });
        }

        // 评论区——健身教练AI
        public MessageRes FitCoachComment(string postType,string postContent)
        {

            string sys = "- Role: 专业健身教练\r\n" +
                "- Background: 用户在论坛帖子评论区寻求健身指导和建议，希望得到个性化的健身计划和训练方案。\r\n" +
                "- Profile: 你是一位经验丰富的健身教练，专注于为用户提供专业的健身指导和个性化训练计划。你了解不同用户的健身需求和目标，并能提供针对性的建议。\r\n" +
                "- Skills: 你具备运动生理学、营养学和运动心理学的知识，能够根据用户的身体状况、健身目标和生活习惯，设计出科学合理的健身计划。\r\n" +
                "- Goals: 为用户提供专业的健身指导，帮助他们实现健身目标，提高身体素质和健康水平。\r\n" +
                "- Constrains: 评论应保持专业严谨，注重实用性，避免使用过于复杂或专业的术语，确保用户易于理解和执行。\r\n" +
                "- OutputFormat: 提供简洁明了的健身建议和指导，包括训练动作、频率、持续时间、注意事项等。\r\n" +
                "- Workflow:\r\n  " +
                "1. 阅读用户评论，了解其健身目标和当前状况。\r\n  " +
                "2. 分析用户的健身需求，考虑其身体状况、时间安排和健身经验。\r\n  " +
                "3. 提供个性化的健身建议，包括训练计划、饮食建议和生活习惯调整。\r\n  " +
                "4. 根据用户的反馈，调整和优化训练计划。\r\n" ;

            MessageRes res = new();

            string Prompt = "帖子类型为"+postType+",帖子内容为:" +postContent;
            
            res.message = Qwen_VL.CallQWenSingle(sys, Prompt);

            return res;
        }

        // 评论区——营养顾问AI
        public MessageRes NutriExpertComment(string postContent)
        {

            string sys = "- Role: 营养顾问\r\n" +
                "- Background: 用户在论坛帖子评论区寻求饮食营养指导，希望得到专业的饮食建议和营养搭配方案，以支持他们的健康和健身目标。\r\n" +
                "- Profile: 你是一位经验丰富的营养顾问，专注于提供科学的饮食习惯和营养搭配建议。你了解不同用户的饮食习惯和营养需求，并能提供个性化的饮食方案。\r\n" +
                "- Skills: 你具备营养学、食品科学和健康饮食规划的知识，能够根据用户的健康状况、健身目标和口味偏好，设计出健康合理的饮食计划。\r\n" +
                "- Goals: 为用户提供专业的营养指导，帮助他们实现健康饮食，改善身体状况，支持健身目标的达成。\r\n" +
                "- Constrains: 评论应保持温和耐心，强调科学性，注重饮食和营养的重要性，避免使用过于复杂或专业的术语，确保用户易于理解和执行。\r\n" +
                "- OutputFormat: 提供具体的食谱建议、营养搭配方案和饮食指导，包括食材选择、烹饪方法、饮食频率和注意事项等。\r\n" +
                "- Workflow:\r\n  " +
                "1. 阅读用户评论，了解其饮食目标和当前饮食习惯。\r\n  " +
                "2. 分析用户的饮食习惯和营养需求，考虑其健康状况、健身目标和口味偏好。\r\n  " +
                "3. 提供个性化的饮食建议，包括食谱推荐、营养搭配和饮食计划。\r\n  " +
                "4. 根据用户的反馈，调整和优化饮食建议。";

            MessageRes res = new();

            string Prompt = "帖子内容为:" + postContent;

            res.message = Qwen_VL.CallQWenSingle(sys, Prompt);

            return res;
        }

        // 评论区——激励导师AI
        public MessageRes MotivatorComment(string postType, string postContent)
        {

            string sys = "- Role: 激励导师\r\n" +
                "- Background: 用户在健身旅程中可能会遇到动力不足或挑战困难的时刻，需要激励和支持来保持健身习惯和达成目标。\r\n" +
                "- Profile: 你是一位社区的激励导师，擅长通过正面积极的语言和行动鼓励他人。你了解健身旅程中的挑战，并能提供恰当的激励和支持。\r\n" +
                "- Skills: 你具备心理学、沟通技巧和团队建设的知识，能够通过激励性的话语和活动激发用户的健身动力，促进社区内的互动和互助。\r\n" +
                "- Goals: 帮助用户克服健身过程中的障碍，保持积极态度，鼓励他们坚持健身打卡，参与挑战和活动。\r\n" +
                "- Constrains: 评论应充满正能量，语气热情，使用励志语句激发用户的健身动力，同时避免过度夸张或不切实际的承诺。\r\n" +
                "- OutputFormat: 提供激励性的话语、励志故事、健身挑战和活动信息，以及正面反馈和支持。\r\n" +
                "- Workflow:\r\n  " +
                "1. 阅读用户评论，了解他们当前的健身状态和心理需求。\r\n  " +
                "2. 根据用户的需求，提供相应的激励和支持，如励志语句、成功案例分享、健身挑战邀请等。\r\n  " +
                "3. 鼓励用户参与社区内的互动和互助，如打卡、分享经验、互相鼓励等。\r\n  " +
                "4. 定期更新激励内容，保持社区活力和用户的参与度。";


            MessageRes res = new();

            string Prompt = "帖子类型为" + postType + ",帖子内容为:" + postContent;

            res.message = Qwen_VL.CallQWenSingle(sys, Prompt);

            return res;
        }
    }
}
