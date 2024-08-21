using Fitness.DAL.Core;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.DAL;
using Fitness.BLL;
using Fitness.Models;

namespace Fitness.BLL
{
    public sealed class WorkoutBll
    {
        static List<string> loseWeight = new List<string> { "HIIT燃脂", "上肢力量初级", "HIIT燃脂", "告别驼背计划", "HIIT燃脂", "告别粗腿", "HIIT燃脂",
                                                     "HIIT燃脂", "燃胸计划", "HIIT燃脂", "上肢力量初级", "HIIT燃脂", "告别驼背计划", "HIIT燃脂",
                                                     "HIIT燃脂", "上肢力量初级", "HIIT燃脂", "燃胸计划", "HIIT燃脂", "告别粗腿", "HIIT燃脂",
                                                     "HIIT燃脂", "告别粗腿", "HIIT燃脂", "告别驼背计划", "HIIT燃脂", "燃胸计划", "HIIT燃脂"};

        static List<string> buildMuscle = new List<string> { "轰炸麒麟臂", "告别驼背计划", "腹肌撕裂", "上肢力量初级", "背部增肌训练", "告别粗腿", "臀腿雕刻",
                                                     "铠甲胸肌", "燃胸计划", "轰炸麒麟臂", "上肢力量初级", "腹肌撕裂", "背部增肌训练", "告别驼背计划",
                                                     "轰炸麒麟臂", "燃胸计划", "上肢力量初级", "背部增肌训练", "臀腿雕刻", "告别粗腿", "告别驼背计划",
                                                     "腹肌撕裂", "上肢力量初级", "腹肌撕裂", "告别驼背计划", "背部增肌训练", "告别粗腿", "臀腿雕刻"};

        static List<string> bodySculpt = new List<string> { "HIIT燃脂", "上肢力量初级", "腹肌撕裂", "告别驼背计划", "燃胸计划", "告别粗腿", "HIIT燃脂",
                                                     "HIIT燃脂", "告别驼背计划", "燃胸计划", "上肢力量初级", "腹肌撕裂", "臀腿雕刻", "HIIT燃脂",
                                                     "HIIT燃脂", "上肢力量初级", "腹肌撕裂", "告别驼背计划", "燃胸计划", "告别粗腿", "HIIT燃脂",
                                                     "HIIT燃脂", "告别驼背计划", "燃胸计划", "上肢力量初级", "腹肌撕裂", "臀腿雕刻", "HIIT燃脂"};
        private static readonly WorkoutBll instance = new WorkoutBll();
        private WorkoutBll()
        {
        }
        public static WorkoutBll Instance
        {
            get
            {
                return instance;
            }
        }

        private static string GetWeekday(int num)
        {
            if (num == 1) return "星期一";
            else if (num == 2) return "星期二";
            else if (num == 3) return "星期三";
            else if (num == 4) return "星期四";
            else if (num == 5) return "星期五";
            else if (num == 6) return "星期六";
            else return "星期日";
        }

        private static string GenerateWorkout(string goal)
        {
            List<string> planList = new List<string>();
            if (goal == "减脂")
                planList = loseWeight;
            else if (goal == "增肌")
                planList = buildMuscle;
            else if (goal == "塑型")
                planList = bodySculpt;
            List<object> plan = new List<object>();

            var workoutPlan = new List<List<Dictionary<string, object>>>();

            foreach (var week in Enumerable.Range(0, 4)) // 四组一周的计划
            {
                var weekPlan = new List<Dictionary<string, object>>();
                int num = 0;
                foreach (var workoutName in planList.Skip(week * 7).Take(7)) // 每周七天的计划
                {
                    num++;
                    DataTable workoutTable = WorkoutDal.Get(workoutName);

                    var workoutData = new Dictionary<string, object>
                    {
                        { "workoutName", workoutName },
                        { "coverUrl", workoutTable.Rows[0]["coverUrl"].ToString() },
                        { "timestamp", GetWeekday(num) }
                    };

                    var exercises = new List<Dictionary<string, object>>();

                    foreach (DataRow row in workoutTable.Rows)
                    {
                        Exercise exerciseTable = ExerciseDal.Get(row["exerciseName"].ToString());

                        var exerciseData = new Dictionary<string, object>
                        {
                            { "exerciseName", row["exerciseName"].ToString() },
                            { "explanation", exerciseTable.explanation },
                            { "gifUrl", exerciseTable.gifUrl },
                            { "coverUrl", exerciseTable.coverUrl },
                            { "equipmentName", exerciseTable.equipmentName },
                            { "part", exerciseTable.part },
                            { "time", exerciseTable.time },
                            { "count", exerciseTable.count }
                        };
                        exercises.Add(exerciseData);
                    }

                    workoutData.Add("exercises", exercises);
                    weekPlan.Add(workoutData);
                }

                workoutPlan.Add(weekPlan);
            }

            var result = new
            {
                message = "successful",
                duration = "4",
                plan = workoutPlan
            };
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string GetPlan(int userId)
        {
            Console.WriteLine("收到获取计划请求\n");
            string goal = UserFitnessPlanGoalDal.Get(userId);
            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(goal);
            if (data["message"] == "fail" || data["message"] == null)
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "fail",
                    error = "未设置健身目标"
                });
            }
            string fitness = FitnessDal.Get(userId);
            string physicalTest = PhysicalTestDal.Get(userId);
            Dictionary<string, object> fitnessTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(fitness);
            Dictionary<string, object> physicalTestTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(physicalTest);
            if (fitnessTable["message"] == "empty" || physicalTestTable["message"] == "empty")
            {
                return JsonConvert.SerializeObject(new
                {
                    message = "fail",
                    error = "未填写体测或体质表信息"
                });
            }
            return GenerateWorkout(data["goal"].ToString());
        }
    }
}
