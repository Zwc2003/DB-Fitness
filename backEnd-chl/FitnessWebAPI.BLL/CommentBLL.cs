using FitnessWebAPI.BLL.Core;
using FitnessWebAPI.BLL.Interfaces;
using FitnessWebAPI.DAL;
using FitnessWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessWebAPI.BLL
{
    public class CommentBLL: ICommentBLL
    {
        //PublishComment   ReplyComment
        //GetCommentByPostID  GetCommentByCommentID
        //DeleteComment
        //likeComment

        private readonly JWTHelper _jwtHelper = new();

        public string PublishComment(string token, Comment comment)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);

            //comment.userID = tokenRes.userID;
            comment.commentTime = DateTime.Now;
            comment.likesCount = 0;
            comment.parentCommentID = -1;
            bool result = CommentDAL.Insert(comment);
            //帖子评论量+1
            result = result && CommentDAL.CommentsCountAddOne(comment.postID);
            //调用通知接口

            return result ? "发布评论成功" : "发布评论失败";
        }

        public string ReplyComment(string token, int parentCommentID, Comment replyComment)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);

            replyComment.userID = tokenRes.userID;
            replyComment.commentTime = DateTime.Now;
            replyComment.parentCommentID = parentCommentID;
            replyComment.likesCount = 0;

            bool result = CommentDAL.Insert(replyComment);
            //帖子评论量+1
            result = result && CommentDAL.CommentsCountAddOne(replyComment.postID);
            //调用通知接口

            return result ? "回复成功" : "回复失败";
        }

        public List<Comment> GetCommentByPostID(string token,int postID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            return CommentDAL.GetCommentByPostID(postID);
        }

        public List<Comment> GetCommentByCommentID(string token, int commentID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            return CommentDAL.GetCommentByCommentID(commentID);
        }

        public string DeleteComment(string token, int commentID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            bool result = false;
            if (tokenRes.Role == "admin" || CommentDAL.IsPostOwner(tokenRes.userID, commentID))
                result = CommentDAL.DeleteComment(commentID);
            else
                return "身份权限不符";
            return result ? "删除评论成功" : "删除评论失败";
        }

        public string LikeComment(string token, int commentID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            bool result = CommentDAL.LikeCountsAddOne(commentID);
            //调用通知接口

            return result ? "点赞成功" : "点赞失败";
        }
    }
}
