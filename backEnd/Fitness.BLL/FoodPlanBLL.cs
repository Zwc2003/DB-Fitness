using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL
{
    public class FoodPlanBLL : IFoodPlanBLL
    {
        FoodPlanDAL recordAndPlanDAL = new();

        // 创建一条饮食记录
        public FoodPlanRes Create(FoodPlanInfo foodPlanInfo)
        {
            FoodPlanRes foodPlanRes = new()
            {
                message = "饮食计划插入失败",
                planID = -1
            };

            FoodPlan foodPlan = new()
            {
                foodPlanID = 0,
                userID = foodPlanInfo.userID,
                createTime = new DateTime(1919, 10, 12),
                date = foodPlanInfo.date,
                mealType = foodPlanInfo.mealType,
                state = foodPlanInfo.state,
                numsOfTypes = foodPlanInfo.foods.Count

            };
            
            int foodPlanID = recordAndPlanDAL.InsertFoodPlan(foodPlan);

            if (foodPlanID==-1) 
                return foodPlanRes;


            for(int i = 0;i<foodPlanInfo.foods.Count;i++)
            {

                FoodPlanFood foodPlanFood = new()
                {
                    foodPlanID = foodPlanID,
                    foodName = foodPlanInfo.foods[i].foodName,
                    foodAmount = foodPlanInfo.foods[i].quantity
                };
                recordAndPlanDAL.InsertFoodPlanFood(foodPlanFood);
            }

            foodPlanRes.message = "饮食计划插入成功";
            foodPlanRes.planID = foodPlanID;

            return foodPlanRes;
        }

        public GetAllFoodPlanNoDetailsRes GetAllNoDetails(int userID)
        {
            DataTable dt = recordAndPlanDAL.GetFoodPlanByUsrID(userID);
            GetAllFoodPlanNoDetailsRes res = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SinglePlanNoDetail single = new SinglePlanNoDetail();
                DataRow dr = dt.Rows[i];
                single.foodPlanID = Convert.ToInt32(dr["foodPlanID"]);
                single.date = Convert.ToDateTime(dr["date"]);
                single.mealType = Convert.ToInt32(dr["mealType"]);
                single.state = Convert.ToInt32(dr["state"]);
                single.numOfTypes = Convert.ToInt32(dr["numOfTypes"]);
                res.plans.Add(single);

            }
            return res;
        }
        


        public GetAllFoodPlanDetailsRes GetAllDetails(int userID)
        {
            DataTable dt = recordAndPlanDAL.GetFoodPlanByUsrID(userID);
            GetAllFoodPlanDetailsRes res = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GetOneFoodPlanDetailRes single = new();
                DataRow dr = dt.Rows[i];
                single.foodPlanID = Convert.ToInt32(dr["foodPlanID"]);
                single.date = Convert.ToDateTime(dr["date"]);
                single.mealType = Convert.ToInt32(dr["mealType"]);
                single.state = Convert.ToInt32(dr["state"]);
                single.numOfTypes = Convert.ToInt32(dr["numOfTypes"]);

                DataTable dtDetailFoods = recordAndPlanDAL.GetFoodPlanFoodsByID(single.foodPlanID);
                for(int j= 0;j<dtDetailFoods.Rows.Count;j++)
                {
                    Food food = new();
                    DataRow drDetailFoods = dtDetailFoods.Rows[j];
                    food.foodName = drDetailFoods["foodName"].ToString();
                    food.quantity = Convert.ToInt32(drDetailFoods["foodAmount"]);
                    single.foods.Add(food);
                }
                res.plans.Add(single);

            }
            return res;
        }

        public GetOneFoodPlanDetailRes GetOneDetail(int foodPlanID)
        {

            GetOneFoodPlanDetailRes res = new();
            DataTable dtFoodPlan = recordAndPlanDAL.GetFoodPlanByPlanID(foodPlanID);
            DataTable dtFoodPlanFood = recordAndPlanDAL.GetFoodPlanFoodsByID(foodPlanID);

            DataRow drFoodPlan = dtFoodPlan.Rows[0];
            res.foodPlanID = Convert.ToInt32(drFoodPlan["foodPlanID"]);
            res.date = Convert.ToDateTime(drFoodPlan["date"]);
            res.mealType = Convert.ToInt32(drFoodPlan["mealType"]);
            res.state = Convert.ToInt32(drFoodPlan["state"]);
            res.numOfTypes = Convert.ToInt32(drFoodPlan["numOfTypes"]);



            for (int i = 0; i < dtFoodPlanFood.Rows.Count; i++)
            {
                Food food = new();
                DataRow drDetailFoods = dtFoodPlanFood.Rows[i];
                food.foodName = drDetailFoods["foodName"].ToString();
                food.quantity = Convert.ToInt32(drDetailFoods["foodAmount"]);
                res.foods.Add(food);
            }

            return res;
        }

        public MessageRes DeleteFoodPlan(int foodPlanID)
        {
            MessageRes deleteFoodPlanRes = new();
            
            bool res = recordAndPlanDAL.DeleteFoodPlanByPlanID(foodPlanID);

            if (res)
                deleteFoodPlanRes.message = "饮食计划删除成功";
            else
                deleteFoodPlanRes.message = "饮食计划删除失败";

            return deleteFoodPlanRes;

        }


        public MessageRes UpdateFoodPlan(UpdateFoodPlanInfo updateFoodPlanInfo)
        {
            MessageRes res = new MessageRes();

            int foodPlanID = updateFoodPlanInfo.foodPlanID;
            int mealType = updateFoodPlanInfo.mealType;
            int numOfTypes = updateFoodPlanInfo.foods.Count;

            if (!recordAndPlanDAL.UpdateFoodPlanByPlanID(foodPlanID, mealType, numOfTypes))
            {
                res.message = "饮食计划更新失败！(1)";
                return res;
            }
          

            if(!recordAndPlanDAL.DeletFoodPlanFoodByPlanID(foodPlanID))
            {
                res.message = "饮食计划更新失败！(2)";
                return res;
            }


            for (int i = 0; i < updateFoodPlanInfo.foods.Count; i++)
            {

                FoodPlanFood foodPlanFood = new()
                {
                    foodPlanID = foodPlanID,
                    foodName = updateFoodPlanInfo.foods[i].foodName,
                    foodAmount = updateFoodPlanInfo.foods[i].quantity
                };
                recordAndPlanDAL.InsertFoodPlanFood(foodPlanFood);
            }

            res.message = "饮食计划更新成功！";
            return res;

        }

        public MessageRes UpdateFoodPlanState(int foodPlanID, int state)
        {
            MessageRes res = new MessageRes();

            

            if (!recordAndPlanDAL.UpdateFoodPlanState(foodPlanID, state))
            {
                res.message = "饮食计划更新失败！(12)";
                return res;
            }


            res.message = "饮食计划更新成功！";
            return res;
        }

        public FoodsRes GetALLFoodsInfo()
        {
            DataTable dt = recordAndPlanDAL.GetALLFoodInfoData();
            FoodsRes foodsRes = new();
            

            for (int i=0;i<dt.Rows.Count;i++)
            {
                DataRow dr = dt.Rows[i];
                FoodInfo foodInfo = new();
                foodInfo.foodName = dr["foodName"].ToString();
                foodInfo.calorie = Convert.ToInt32(dr["calorie"]);
                foodsRes.foodsInfo.Add(foodInfo);
            }
            return foodsRes;
        }


    }
}
