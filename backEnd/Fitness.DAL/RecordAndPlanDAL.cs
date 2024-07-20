using Fitness.DAL.Core;
using Fitness.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.DAL
{
    public class RecordAndPlanDAL
    {
        private static FoodPlan FoodPlanToModel(DataRow row)
        {
            FoodPlan foodPlan = new()
            {
                foodPlanID = Convert.ToInt32(row["foodPlanID"]),
                userID = Convert.ToInt32(row["userID"]),
                createTime = Convert.ToDateTime(row["createTime"]),
                date = Convert.ToDateTime(row["date"]),
                mealType = Convert.ToInt32(row["mealType"]),
                state = Convert.ToInt32(row["state"])
            };

            return foodPlan;
        }

        public static List<FoodPlan> FoodPlanToModelList(DataTable dt)
        {
            List<FoodPlan> foodPlans = new();
            for(int i=0;i<dt.Rows.Count;i++)
            {
                DataRow dr = dt.Rows[i];
                FoodPlan foodPlan = FoodPlanToModel(dr);
                foodPlans.Add(foodPlan);
            }
            return foodPlans;
        }

        // 成功插入一条饮食计划时，返回这条饮食计划的foodPlanID
        public bool InsertFoodPlan(FoodPlan foodPlan)
        {
            const int maxRetries = 3; // 定义最大重试次数
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    string sql = "INSERT INTO \"FoodPlan\" (\"userID\", \"createTime\", \"date\", \"mealType\", \"state\")" +
                        "VALUES (:\"userID\", :\"createTime\", :\"date\", :\"mealType\", :\"state\")";
                       

                    OracleParameter[] oracleParameters = new OracleParameter[]
                    {
                        new OracleParameter("\"userID\"",OracleDbType.Int32) { Value = foodPlan.userID},
                        new OracleParameter("\"createTime\"",OracleDbType.TimeStamp) { Value = DateTime.Now },
                        new OracleParameter("\"date\"",OracleDbType.Date) { Value = foodPlan.date },
                        new OracleParameter("\"mealType\"",OracleDbType.Int32) { Value = foodPlan.mealType },
                        new OracleParameter("\"state\"",OracleDbType.Int32){ Value = foodPlan.state },
                        
                    };
                    OracleHelper.ExecuteNonQuery(sql, null,oracleParameters);
                    //OracleHelper.ExecuteNonQuery("COMMIT;");

                    
                    return true; // 获取输出参数的值
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ORA-02185"))
                    {
                        retryCount++;
                        if (retryCount >= maxRetries)
                        {
                            // 如果达到最大重试次数，抛出异常或记录错误
                            Console.Error.WriteLine($"Failed to insert garden after {maxRetries} retries. Error: {ex.Message}");
                            throw; // 抛出异常，也可以选择不抛出而返回 false 或通知管理员
                        }
                        else
                        {
                            // 可以在这里添加延时，避免立即重试造成更多的竞争
                            System.Threading.Thread.Sleep(1000); // 延迟1秒
                        }
                    }
                    else
                    {
                        // 其他类型的异常处理
                        Console.Error.WriteLine($"Unexpected error occurred: {ex.Message}");
                        throw; // 或者根据具体情况进行处理
                    }
                }
            }

            // 如果循环结束还没有成功，返回 -1
            return false;
        }

        public bool InsertFoods(Foods food)
        {
            const int maxRetries = 3; // 定义最大重试次数
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    string sql = "INSERT INTO Foods(foodName, calorie) " +
                        "VALUES(:foodName, :calorie)";
                    OracleParameter[] oracleParameters = new OracleParameter[]
                    {
                        new OracleParameter("foodName",OracleDbType.NVarchar2) { Value = food.foodName},
                        new OracleParameter("calorie",OracleDbType.Int32) { Value = food.calorie },
                    };
                    OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);
                 
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ORA-02185"))
                    {
                        retryCount++;
                        if (retryCount >= maxRetries)
                        {
                            // 如果达到最大重试次数，抛出异常或记录错误
                            Console.Error.WriteLine($"Failed to insert garden after {maxRetries} retries. Error: {ex.Message}");
                            throw; // 抛出异常，也可以选择不抛出而返回 false 或通知管理员
                        }
                        else
                        {
                            // 可以在这里添加延时，避免立即重试造成更多的竞争
                            System.Threading.Thread.Sleep(1000); // 延迟1秒
                        }
                    }
                    else
                    {
                        // 其他类型的异常处理
                        Console.Error.WriteLine($"Unexpected error occurred: {ex.Message}");
                        throw; // 或者根据具体情况进行处理
                    }
                }
            }

            // 如果循环结束还没有成功，返回 false 或抛出异常
            return false;
        }

        public bool InsertFoodPlanFood(FoodPlanFood foodPlanFood)
        {
            const int maxRetries = 3; // 定义最大重试次数
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    string sql = "INSERT INTO Foods(foodPlanID, foodName, foodAmount) " +
                        "VALUES(:foodPlanID, :foodName, :foodAmount)";
                    OracleParameter[] oracleParameters = new OracleParameter[]
                    {
                        new OracleParameter("foodPlanID",OracleDbType.Int32) { Value = foodPlanFood.foodPlanID},
                        new OracleParameter("foodName",OracleDbType.NVarchar2) { Value = foodPlanFood.foodName },
                        new OracleParameter("foodAmount",OracleDbType.Int32) {Value = foodPlanFood.foodAmount}
                    };
                    OracleHelper.ExecuteNonQuery(sql, null,oracleParameters);
                    OracleHelper.ExecuteNonQuery("commit;");
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ORA-02185"))
                    {
                        retryCount++;
                        if (retryCount >= maxRetries)
                        {
                            // 如果达到最大重试次数，抛出异常或记录错误
                            Console.Error.WriteLine($"Failed to insert garden after {maxRetries} retries. Error: {ex.Message}");
                            throw; // 抛出异常，也可以选择不抛出而返回 false 或通知管理员
                        }
                        else
                        {
                            // 可以在这里添加延时，避免立即重试造成更多的竞争
                            System.Threading.Thread.Sleep(1000); // 延迟1秒
                        }
                    }
                    else
                    {
                        // 其他类型的异常处理
                        Console.Error.WriteLine($"Unexpected error occurred: {ex.Message}");
                        throw; // 或者根据具体情况进行处理
                    }
                }
            }

            // 如果循环结束还没有成功，返回 false 或抛出异常
            return false;
        }
    }
}
