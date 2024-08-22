using FitnessWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessWebAPI.BLL.Interfaces
{
    public interface ICommentBLL
    {
        public string PublishComment(string token, Comment comment);
        public string ReplyComment(string token, int parentCommentID, Comment replyComment);
        public List<Comment> GetCommentByPostID(string token, int postID);
        public List<Comment> GetCommentByCommentID(string token, int commentID);
        public string DeleteComment(string token, int commentID);
        public string LikeComment(string token, int commentID);

    }
}
