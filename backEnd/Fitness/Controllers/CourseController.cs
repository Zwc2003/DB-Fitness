using Fitness.BLL;
using Fitness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseBLL _courseBLL;

        public CourseController()
        {
            _courseBLL = new CourseBLL();
        }

        [HttpPost]
        public ActionResult<string> PublishCourse([FromHeader] string token, [FromBody] Course course)
        {
            return _courseBLL.PublishCourse(token, course);
        }

        [HttpPut]
        public ActionResult<string> ModifyCourse([FromHeader] string token, [FromBody] Course course)
        {
            return _courseBLL.ModifyCourse(token, course);
        }

        [HttpDelete]
        public ActionResult<string> DeleteCourseByClassID([FromHeader] string token, int classID)
        {
            return _courseBLL.DeleteCourseByClassID(token, classID);
        }

        [HttpGet]
        public ActionResult<Course> GetCourseByClassID([FromHeader] string token, int classID)
        {
            return _courseBLL.GetCourseByClassID(token, classID);
        }

        [HttpGet]
        public ActionResult<string> GetAllCourse()
        {
            return _courseBLL.GetAllCourse();
        }

        // public string BeTrainee(string token,Trainee trainee) 
        [HttpPost]
        public ActionResult<string> BeTrainee(string token, Trainee trainee)
        {
            return _courseBLL.BeTrainee(token,trainee);
        }


        [HttpPost]
        public ActionResult<string> ReserveCourse([FromHeader] string token, int classID, [FromQuery] string payMethod)
        {
            return _courseBLL.ReserveCourse(token, classID, payMethod);
        }

        [HttpPost]
        public ActionResult<string> PayCourseFare([FromHeader] string token, [FromQuery] int bookID, [FromQuery] int amount, [FromQuery] string payMethod)
        {
            return _courseBLL.PayCourseFare(token, bookID, amount, payMethod);
        }

        [HttpPost]
        public ActionResult<string> CancelCourse([FromHeader] string token, int bookID)
        {
            return _courseBLL.CancelCourse(token, bookID);
        }

        [HttpGet]
        public ActionResult<List<BookCourseInfo>> GetCourseByUserID([FromHeader] string token)
        {
            return _courseBLL.GetCourseByUserID(token);
        }

        [HttpGet]
        public ActionResult<List<Course>> SearchCourse([FromHeader] string token, [FromQuery] string keyword, [FromQuery] int typeID = -1, [FromQuery] int minPrice = 0, [FromQuery] int maxPrice = int.MaxValue)
        {
            return _courseBLL.SearchCourse(token, keyword, typeID, minPrice, maxPrice);
        }

        [HttpPost]
        public ActionResult<string> GradeCourse([FromHeader] string token, int classID, [FromQuery] int grade)
        {
            return _courseBLL.GradeCourse(token, classID, grade);
        }

        [HttpPost]
        public ActionResult<string> PublishComment([FromHeader] string token, int classID, [FromQuery] string comment)
        {
            return _courseBLL.PublishComment(token, classID, comment);
        }

        [HttpGet]
        public ActionResult<List<string>> GetCourseCommentByClassID([FromHeader] string token, int classID)
        {
            return _courseBLL.GetCourseCommentByClassID(token, classID);
        }

        [HttpGet]
        public ActionResult<double> GetAverageGrade(string token, int classID) { 
            return _courseBLL.GetAverageGrade(token, classID);
        }
    }
}
