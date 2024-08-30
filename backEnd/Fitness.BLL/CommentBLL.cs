using Fitness.BLL.Core;
using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fitness.BLL
{
    public class CommentBLL: ICommentBLL
    {
        //PublishComment   ReplyComment
        //GetCommentByPostID  GetCommentByCommentID
        //DeleteComment
        //likeComment

        private readonly JWTHelper _jwtHelper = new();
        private readonly UserAchievementBLL _userAchievement = new UserAchievementBLL();
        private readonly PostBLL postbll = new PostBLL();

        public string PublishComment(string token, Comment comment)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);

            comment.userID = tokenRes.userID;
            comment.commentTime = DateTime.Now;
            comment.likesCount = 0;
            comment.parentCommentID = -1;
            int st;
            comment.userName = UserDAL.GetUserByUserID(tokenRes.userID,out st).userName;
            bool result = CommentDAL.Insert(comment);
            //帖子评论量+1
            result = result && CommentDAL.CommentsCountAddOne(comment.postID);
            //更新成就
            int achieveUserID = postbll.GetPostByPostID(token, comment.postID).userID;
            _userAchievement.UpdateBeComment(achieveUserID, 1);
            //调用通知接口
            return result ? "发布评论成功" : "发布评论失败";
        }

        public string ReplyComment(string token, Comment replyComment)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);

            replyComment.userID = tokenRes.userID;
            replyComment.commentTime = DateTime.Now;
            replyComment.likesCount = 0;
            int st;
            replyComment.userName = UserDAL.GetUserByUserID(tokenRes.userID, out st).userName;
            bool result = CommentDAL.Insert(replyComment);
            //帖子评论量+1
            result = result && CommentDAL.CommentsCountAddOne(replyComment.postID);
            //更新成就
            int achieveUserID = postbll.GetPostByPostID(token, replyComment.postID).userID;
            _userAchievement.UpdateBeComment(achieveUserID, 1);
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

        public string DeleteComment(string token, int commentID,int postID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            bool result = false;
            PostBLL postBLL = new PostBLL();
            int PostUserID = postBLL.GetPostByPostID(token, postID).userID;
            bool isPostOwner = false;
            if (PostUserID == tokenRes.userID) { 
                isPostOwner = true;
            }
            if (tokenRes.Role == "admin" || isPostOwner)
            {
                CommentDAL.CommentsCountMinusOne(postID);
                result = CommentDAL.DeleteComment(commentID);
            }
            else
                return "身份权限不符";
            //更新成就
            _userAchievement.UpdateBeComment(PostUserID, -1);
            return result ?"评论删除成功" : "评论删除失败";
        }

        public string LikeComment(string token, int commentID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            bool result = CommentDAL.LikeCountsAddOne(commentID);
            //调用通知接口

            return result ? "点赞成功" : "点赞失败";
        }

        public string CancleLikeComment(string token, int commentID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            bool result = CommentDAL.LikeCountsMinusOne(commentID);
            return result ? "取消点赞成功" : "取消点赞失败";
        }

    }
}
