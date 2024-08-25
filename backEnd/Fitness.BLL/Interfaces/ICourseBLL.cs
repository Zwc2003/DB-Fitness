using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL.Interfaces
{
    public interface ICourseBLL
    {
        public string PublishCourse(string token, Course course);
        public string ModifyCourse(string token, Course course);
        public string DeleteCourseByClassID(string token, int classID);
        public Course GetCourseByClassID(string token, int classID);
        public List<Course> GetAllCourse(string token);
        public string BeTrainee(string token, Trainee trainee);
        public string ReserveCourse(string token, int classID, string payMethod);
        public string PayCourseFare(string token, int bookID, int amount, string payMethod);
        public string CancelCourse(string token,int bookID);

        public List<BookCourseInfo> GetCourseByUserID(string token);
        public List<Course> SearchCourse(string token, string keyword, int typeID = -1, int minPrice = 0, int maxPrice = int.MaxValue);

        public string GradeCourse(string token, int classID, int grade);
        public string PublishComment(string token, int classID, string comment);
        public List<string> GetCourseCommentByClassID(string token, int classID);
        public double GetAverageGrade(string token, int classID);
    }
}
