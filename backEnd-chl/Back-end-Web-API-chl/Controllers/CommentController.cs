using FitnessWebAPI.BLL;
using FitnessWebAPI.BLL.Interfaces;
using FitnessWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Back_end_Web_API_chl.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentBLL _commentBLL;

        public CommentController()
        {
            _commentBLL = new CommentBLL();
        }

        [HttpPost]
        public ActionResult<string> PublishComment( string token, [FromBody] Comment comment)
        {
            var result = _commentBLL.PublishComment(token, comment);
            if (result == "评论发布成功")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public ActionResult<string> ReplyComment(string token,int parentComentID, [FromBody] Comment comment)
        {
            var result = _commentBLL.ReplyComment(token, parentComentID, comment);
            if (result == "回复成功")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public ActionResult<List<Comment>> GetCommentByPostID(string token, int postID)
        {
            var comments = _commentBLL.GetCommentByPostID(token, postID);
            if (comments != null && comments.Count > 0)
            {
                return Ok(comments);
            }
            return NotFound("未找到相关评论");
        }

        [HttpGet]
        public ActionResult<Comment> GetCommentByCommentID(string token, int commentID)
        {
            var comment = _commentBLL.GetCommentByCommentID(token, commentID);
            if (comment != null)
            {
                return Ok(comment);
            }
            return NotFound("未找到该评论");
        }

        [HttpDelete]
        public ActionResult<string> DeleteComment(string token, int commentID)
        {
            var result = _commentBLL.DeleteComment(token, commentID);
            if (result == "评论删除成功")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public ActionResult<string> LikeComment(string token, int commentID)
        {
            var result = _commentBLL.LikeComment(token, commentID);
            if (result == "点赞成功")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
