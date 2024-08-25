using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL.Interfaces
{
    public interface IPostBLL
    {
        public string Post(string token, Post post);
        public List<Post> GetPostByUserID(string token,int userID);
        public List<Post> GetAllPost(string token);
        public string Delete(string token, int postId);
        public string LikePost(string token, int postId);
        public string CancleLike(string token, int postId);
        public string Forward(string token, int postId);
        public List<Post> Search(string token, string query, string category = null, string dateRange = null, string sortBy = "postTime");

    }
}
