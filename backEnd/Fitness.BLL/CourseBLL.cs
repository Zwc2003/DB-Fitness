using Fitness.BLL.Core;
using Fitness.DAL;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Fitness.DAL.Core;
using System.Data;
using System.Xml.Linq;
using Fitness.BLL.Interfaces;
using Newtonsoft.Json;

namespace Fitness.BLL
{
    public class CourseBLL : ICourseBLL
    {

        private readonly JWTHelper _jwtHelper = new();
        
        // 发布课程
        public string PublishCourse(string token,Course course)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (tokenRes.Role != "coach")
            {
                return "身份权限不符！";
            }

            OracleTransaction transaction = null;
            try
            {
                // 插入课程信息
                int res = CourseDAL.Insert(course);
                if (res == -1)
                {
                    //transaction.Rollback();
                    return "课程发布失败：无法插入课程信息";
                }
                Console.WriteLine(res);
                // 插入教练授课信息
                Teaches teaches = new Teaches
                {
                    coachID = tokenRes.userID,
                    classID = res,
                    typeID = course.typeID
                };

                if (!TeachesDAL.Insert(teaches, transaction))
                {
                    return "课程发布失败：无法插入教练授课信息";
                }

                return "课程发布成功";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发布课程时出错：{ex.Message}");
                return $"课程发布失败：{ex.Message}";
            }
        }

        // 修改课程
        public string ModifyCourse(string token,Course course)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            if (tokenRes.Role != "coach")
            {
                return "身份权限不符！";
            }

            try
            {
                if (!CourseDAL.UpdateByClassID(course))
                {
                    return "课程修改失败";
                }
                return "课程修改成功";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"修改课程时出错：{ex.Message}");
                return $"课程修改失败：{ex.Message}";
            }
        }

        // 根据classID删除课程
        public string DeleteCourseByClassID(string token,int classID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);

            if (tokenRes.Role != "admin" && tokenRes.Role != "coach")
            {
                return "身份权限不符！";
            }

                OracleTransaction transaction = null;
                try
                {
                    if (!CourseDAL.DeleteByClassID(classID, transaction))
                    {
                        return "课程删除失败：无法删除课程信息";
                    }

                    return "课程删除成功";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"删除课程时出错：{ex.Message}");
                    return $"课程删除失败：{ex.Message}";
                }
        }

        // 根据classID获取课程
        public Course GetCourseByClassID(string token, int classID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            try
            {
                return CourseDAL.GetCourseByClassID(classID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取课程时出错：{ex.Message}");
                return null;
            }
        }

        // 获取所有课程
        public string GetAllCourse()
        {
            try
            {
                List<Dictionary<string, string>> courseDetails = new List<Dictionary<string, string>>();
                List<Course> courses = new List<Course>();
                courses = CourseDAL.GetAllCourseRandomly();
                //Console.WriteLine(courses);
                foreach (var course in courses)
                {
                    string typeName = CourseDAL.GetTypeNameByTypeID(course.typeID);
                    int coachID = CourseDAL.GetCoachIDByClassID(course.classID);
                    string coachName = string.Empty;
                    string instructorHonors = string.Empty;
                    string iconURL = string.Empty;
                    var coach = CoachDAL.GetCoachByCoachID(coachID);
                    if (coach != null)
                    {
                        coachName = coach.coachName;
                        instructorHonors = coach.instructorHonors;
                        iconURL = coach.iconURL;
                    }

                    var courseInfo = new Dictionary<string, string>
        {
            { "coursePhotoUrl", course.coursePhotoUrl },
            { "courseName", course.courseName },
            { "courseDescription", course.courseDescription },
            { "courseStartTime", course.courseStartTime.ToString("yyyy-MM-dd") },
            { "courseEndTime", course.courseEndTime.ToString("yyyy-MM-dd") },
            { "courseGrade", course.courseGrade.ToString() },
            { "coursePrice", course.coursePrice.ToString() },
            { "coachName", coachName },
            { "instructorHonors", instructorHonors },
            { "iconURL", iconURL },
            { "features", course.features },
            { "courseType", typeName },
            { "classTime", course.classTime }
        };

                    courseDetails.Add(courseInfo);
                }

                return JsonConvert.SerializeObject(courseDetails, Formatting.Indented);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取所有课程时出错：{ex.Message}");
                return "";
            }
        }

        public string BeTrainee(string token,Trainee trainee) 
        {
            try
            {
                TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
                if (tokenRes.Role == "trainee" ) {
                    return "您已是学员身份！";
                }

                // 预订课程
                bool result = TraineeDAL.Insert(trainee);

                if (result)
                {
                    return "成功转变为学员身份！";
                }
                else
                {
                    return $"转变学员失败: {result}";
                }
            }
            catch (Exception ex)
            {
                return $"转变学员失败: {ex.Message}";
            }
        }

        // 预订课程
        public string ReserveCourse(string token ,int classID, string payMethod)
        {
            try
            {
                TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
                // 预订课程
                int result = BookDAL.Insert(new Book
                {
                    classID = classID,
                    traineeID = tokenRes.userID,
                    payMethod = payMethod
                });

                if (result != -1)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        bookID = result,
                        message = "课程预订成功"
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        bookID = -1,
                        message = "课程预订失败"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return JsonConvert.SerializeObject(new
                {
                    bookID = -1,
                    message = "课程预订失败"
                }) ; 
            }
        }

        // 支付课程费用
        public string PayCourseFare(string token,int bookID, int amount, string payMethod)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int traineeID = tokenRes.userID;
            OracleConnection conn = null;
            OracleTransaction transaction = null;
            try
            {
                // 更新Payment表
                bool paymentResult = PaymentDAL.Insert(new Payment
                {
                    bookID = bookID,
                    Amount = amount,
                    payMethod = payMethod
                }, transaction);

                if (!paymentResult)
                {
                    return "支付课程费用失败:更新Payment表异常！";
                }
                int fare = -amount;
                // !!! 调用支付接口
                var vigorTokenBLL = new VigorTokenBLL();
                vigorTokenBLL.UpdateBalance(traineeID, $"购买课程，预订账单号{bookID},耗费{amount}活力币", fare);

                int bookstatus = 1;
                // 更新Book表的支付状态和支付ID
                bool bookResult = BookDAL.UpdateBookStatusAndPaymentID(bookID, bookstatus,PaymentDAL.GetByBookID(bookID).paymentID);
                if (!bookResult)
                {
                    return "支付课程费用失败：更新Book表异常！";
                }

                //向Traniee表中插入一条信息


                // 向Advise表中插入一条信息
                bool adviseResult = AdviseDAL.Insert(new Advise
                {
                    classID = BookDAL.GetBookByID(bookID).classID,
                    coachID = TeachesDAL.GetByID(BookDAL.GetBookByID(bookID).classID).coachID,
                    traineeID = traineeID
                }, transaction);

                if (!adviseResult)
                {
                    return "支付课程费用失败:更新Advise表异常！";
                }

                // 向Participate表中插入一条信息
                bool participateResult = ParticipateDAL.Insert(new Participate
                {
                    classID = BookDAL.GetBookByID(bookID).classID,
                    traineeID = traineeID,
                    typeID = CourseDAL.GetCourseByClassID(BookDAL.GetBookByID(bookID).classID).typeID
                }, transaction);

                if (!participateResult)
                {
                    return "支付课程费用失败:更新Participate表异常！";
                }

                // 提交事务
                //transaction.Commit();
                return "支付课程费用成功!";
            }
            catch (Exception ex)
            {
                return $"支付失败: {ex.Message}";
            }
        }

        // 取消课程预订
        public string CancelCourse(string token,int bookID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            OracleConnection conn = null;
            OracleTransaction transaction = null;
            try
            {
                // 删除Participate记录
                bool participateResult = ParticipateDAL.DeleteByClassIDAndTraineeID(BookDAL.GetBookByID(bookID).classID, tokenRes.userID);
                if (!participateResult)
                {
                    return "取消课程预订失败：删除Participate记录异常！";
                }

                int amount = PaymentDAL.GetByBookID(bookID).Amount;
                int fare = -amount;

                //！！！调用退款接口
                var vigorTokenBLL = new VigorTokenBLL();
                vigorTokenBLL.UpdateBalance(tokenRes.userID, $"退出课程，预订账单号{bookID},退还{amount}活力币", fare);

                // 更改Book记录
                bool bookResult = BookDAL.UpdateBookStatusAndPaymentID(bookID,2,-1);
                if (!bookResult)
                {
                    return "取消课程预订失败：更改Book记录异常！";
                }
                return "取消课程成功！";
            }
            catch (Exception ex)
            {
                return $"取消失败: {ex.Message}";
            }
        }

        // 根据userID获取课程
        public List<BookCourseInfo> GetCourseByUserID(string token)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int userID =tokenRes.userID;

            try
            {
                return CourseDAL.GetCourseByUserID(userID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取课程时出错：{ex.Message}");
                return new List<BookCourseInfo>();
            }
        }

        // 搜索课程
        public List<Course> SearchCourse(string token,string keyword, int typeID = -1, int minPrice = 0, int maxPrice = int.MaxValue)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            // 检查关键字是否为空
            if (string.IsNullOrEmpty(keyword) && typeID == -1 && minPrice == 0 && maxPrice == int.MaxValue)
            {
                throw new ArgumentException("至少需要一个搜索条件");
            }

            // 调用 CourseDAL 的搜索方法
            return CourseDAL.SearchCourses(new CourseSearchParams {
                   CourseName = keyword,
                   TypeID = typeID,
                   MinPrice = minPrice,
                   MaxPrice = maxPrice
                    });
        }

        // 评分课程:或者评分教练比较好？
        public string GradeCourse(string token,int classID, int grade)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int traineeID = tokenRes.userID;
            // 获取参与记录
            Participate participate = ParticipateDAL.GetByClassIDAndTraineeID(classID, traineeID);

            if (participate == null)
            {
                return "未找到该课程的参与记录";
            }

            // 更新评分
            participate.Grade = grade;
            try
            {
                if (ParticipateDAL.Update(participate) > 0)
                {
                    return "评分成功";
                }
                else
                {
                    return "评分失败";
                }
            }
            catch (Exception ex)
            {
                return $"评分过程中发生错误: {ex.Message}";
            }
        }

        // 发布评论
        public string PublishComment(string token,int classID, string comment)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            int traineeID = tokenRes.userID;

            // 获取参与记录
            Participate participate = ParticipateDAL.GetByClassIDAndTraineeID(classID, traineeID);

            if (participate == null)
            {
                return "未找到该课程的参与记录";
            }

            // 更新评论
            participate.Evaluate = comment;

            try
            {
                if (ParticipateDAL.Update(participate) > 0)
                {
                    return "评论发布成功";
                }
                else
                {
                    return "评论发布失败";
                }
            }
            catch (Exception ex)
            {
                return $"评论发布过程中发生错误: {ex.Message}";
            }
        }

        // 根据课程ID获取评论
        public List<string> GetCourseCommentByClassID(string token,int classID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            try
            {
                List<string> comments = ParticipateDAL.GetCommentsByClassID(classID);
                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取评论失败: {ex.Message}");
                throw;
            }
        }

        //获取评分
        public double GetAverageGrade(string token, int classID)
        {
            TokenValidationResult tokenRes = _jwtHelper.ValidateToken(token);
            try
            {
                List<int> grades = ParticipateDAL.GetGradesByClassID(classID);
                //计算平均分
                double grade = grades.Average();
                return grade;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取评分失败: {ex.Message}");
                throw;
            }
        }

    }
}
