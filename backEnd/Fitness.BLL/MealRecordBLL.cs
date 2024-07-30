using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Fitness.BLL.Interfaces;
using Fitness.DAL;
using System.Data;
using Fitness.DAL.Core;

namespace Fitness.BLL
{
    public class MealRecordBLL:IMealRecordBLL
    {
        MealRecordDAL mealRecordDAL = new();


        string prePrompt = "- Role: 健身系统内的饮食健康专家\r\n" +
            "- Background: 用户希望通过上传饮食图像和提供饮食信息来获得专业的饮食建议，以帮助他们达到健身目标并改善健康状况。\r\n" +
            "- Profile: 你是一位经验丰富的营养师，具备深厚的营养学知识和丰富的实践经验，能够通过分析用户的饮食内容提供个性化的饮食建议。\r\n" +
            "- Skills: 营养学知识、饮食分析、健康建议、沟通技巧。\r\n- Goals: 为用户提供科学的饮食建议，帮助他们实现健身目标并维持健康的生活方式。\r\n" +
            "- Constrains: 建议应基于用户上传的饮食图像 \r\n- OutputFormat: 不要有任何多余的问候语，直接给出建议！\r\n" +
            "- Workflow:\r\n  " +
            "1. 分析用户上传的饮食图像和信息。\r\n  " +
            "2. 根据营养学原则评估饮食的均衡性和健康性。\r\n  " +
            "3. 提供针对性的饮食建议和改进方案。\r\n  " +
            "4. 给出未来餐饮的建议，包括食材选择、饮食结构和饮食习惯。"+
            "用户的餐饮信息如下：";


        public MealRecordRes Create(MealRecordInfo mealRecordInfo)
        {
            
            MealRecordRes mealRecordRes = new()
            {
                message = "饮食记录插入失败！",
                recordID = -1
            };

            long timestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond;

            // 使用时间戳创建唯一的 objectName
            string objectName = $"{mealRecordInfo.userID}_{timestamp}.png";


            OSSHelper.UploadBase64ImageToOss(mealRecordInfo.mealPhoto, objectName);
            mealRecordInfo.mealPhoto = OSSHelper.GetPublicObjectUrl(objectName);

            int recordID = mealRecordDAL.InsertMealRecord(mealRecordInfo);
            if (recordID != -1)
            {
                mealRecordRes.message = "饮食记录插入成功！";
                mealRecordRes.recordID = recordID;

                for(int i=0;i<mealRecordInfo.foods.Count;i++)
                {
                    prePrompt += mealRecordInfo.foods[i].foodName + ":" + mealRecordInfo.foods[i].quantity.ToString() + "克;";

                    MealRecordFood mealRecordFood = new()
                    {
                        recordID = recordID,
                        foodName = mealRecordInfo.foods[i].foodName,
                        foodAmount = mealRecordInfo.foods[i].quantity
                    };

                    mealRecordDAL.InsertMealRecordsFood(mealRecordFood);

                }

                mealRecordRes.message = "饮食记录插入成功";
                mealRecordRes.recordID = recordID;

                // 异步任务在后台执行
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 大模型生成
                        string result = await Qwen_VL.CallQWen(prePrompt, mealRecordInfo.mealPhoto);

                        // 更新建议
                        mealRecordDAL.UpdateAdvicing(recordID, result);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"后台处理失败: {ex.Message}");
                    }
                });
            }

            return mealRecordRes;

        }

        public GetAllMealRecordsNoDetailsRes GetAllNoDetails(int userID)
        {
            DataTable dt = mealRecordDAL.GetMealRecordByUsrID(userID);
            GetAllMealRecordsNoDetailsRes res = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SingleMealRecordNoDetail single = new ();
                DataRow dr = dt.Rows[i];
                single.recordID = Convert.ToInt32(dr["recordID"]);
                single.mealTime = Convert.ToDateTime(dr["mealTime"]);
                single.mealPhoto = dr["mealPhoto"].ToString();
                single.createdTime = Convert.ToDateTime(dr["createTime"]);
                res.records.Add(single);

            }
            return res;
        }

        public GetAllMealRecordsDetailsRes GetAllDetails(int userID)
        {
            DataTable dt = mealRecordDAL.GetMealRecordByUsrID(userID);
            GetAllMealRecordsDetailsRes res = new();

            

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GetOneMealRecordDetailsRes single = new();
                DataRow dr = dt.Rows[i];
                single.recordID = Convert.ToInt32(dr["recordID"]);
                single.mealTime = Convert.ToDateTime(dr["mealTime"]);
                single.mealPhoto = dr["mealPhoto"].ToString();
                single.createdTime = Convert.ToDateTime(dr["createTime"]);
                single.diningAdvice = dr["diningAdvice"].ToString();

                int totalcalorie = 0;
                

                DataTable dtDetailFoods = mealRecordDAL.GetMealRecordsFoodsByID(single.recordID);


                for (int j = 0; j < dtDetailFoods.Rows.Count; j++)
                {
                    FoodRecord food = new();
                    DataRow drDetailFoods = dtDetailFoods.Rows[j];
                    food.foodName = drDetailFoods["foodName"].ToString();
                    food.quantity = Convert.ToInt32(drDetailFoods["foodAmount"]);
                    DataTable dtCalorie = mealRecordDAL.GetOneFoodCalorie(food.foodName);
                    DataRow drCalorie = dtCalorie.Rows[0];
                    food.calorie = Convert.ToInt32(drCalorie["calorie"])*food.quantity; // 这个食物的所有calorie
                    totalcalorie += food.calorie;
                    single.foods.Add(food);
                }

                single.totalCalorie = totalcalorie;

                res.records.Add(single);

            }
            return res;
        }

        public GetOneMealRecordDetailsRes GetOneDetail(int recordID)
        {

            GetOneMealRecordDetailsRes res = new();
            DataTable dtMealRecord = mealRecordDAL.GetMealRecordByRecordID(recordID);
            DataTable dtMealRecordFood = mealRecordDAL.GetMealRecordsFoodsByID(recordID);

            DataRow drMealRecord = dtMealRecord.Rows[0];

            res.recordID = Convert.ToInt32(drMealRecord["recordID"]);
            res.mealTime = Convert.ToDateTime(drMealRecord["mealTime"]);
            res.mealPhoto = drMealRecord["mealPhoto"].ToString();
            res.createdTime = Convert.ToDateTime(drMealRecord["createTime"]);
            res.diningAdvice = drMealRecord["diningAdvice"].ToString();

            int totalCalorie = 0;

            for (int i = 0; i < dtMealRecordFood.Rows.Count; i++)
            {
                FoodRecord food = new();
                DataRow drDetailFoods = dtMealRecordFood.Rows[i];
                food.foodName = drDetailFoods["foodName"].ToString();
                food.quantity = Convert.ToInt32(drDetailFoods["foodAmount"]);
                DataTable dtCalorie = mealRecordDAL.GetOneFoodCalorie(food.foodName);
                DataRow drCalorie = dtCalorie.Rows[0];
                food.calorie = Convert.ToInt32(drCalorie["calorie"]) * food.quantity; // 这个食物的所有calorie
                totalCalorie += food.calorie;
                res.foods.Add(food);
            }
            res.totalCalorie = totalCalorie;

            return res;
        }

        public MessageRes DeleteMealRecord(int recordID)
        {
            MessageRes deleteRes = new();

            bool res = mealRecordDAL.DeleteMealRecordByRecordID(recordID);

            if (res)
                deleteRes.message = "饮食计划删除成功";
            else
                deleteRes.message = "饮食计划删除失败";

            return deleteRes;

        }

        public MessageRes UpdateMealRecords(UpdateMealRecordInfo updateMealRecordInfo)
        {
            MessageRes res = new MessageRes();

            int recordID = updateMealRecordInfo.recordID;
            DateTime mealTime = updateMealRecordInfo.mealTime;
            string mealPhoto = updateMealRecordInfo.mealPhoto;

            if (!mealRecordDAL.UpdateMealRecordByRecordID(recordID, mealTime, mealPhoto))
            {
                res.message = "饮食记录更新失败！--删除缩略信息时出错！";
                return res;
            }


            if (!mealRecordDAL.DeletMealRecordFoodByID(recordID))
            {
                res.message = "饮食计划更新失败！--删除详细食物信息时出错！";
                return res;
            }


            for (int i = 0; i < updateMealRecordInfo.foods.Count; i++)
            {

                MealRecordFood mealRecordFood = new()
                {
                    recordID = recordID,
                    foodName = updateMealRecordInfo.foods[i].foodName,
                    foodAmount = updateMealRecordInfo.foods[i].quantity
                };
                mealRecordDAL.InsertMealRecordsFood(mealRecordFood);
            }

            res.message = "饮食记录更新成功！";
            return res;

        }






    }
}
