using Fitness.BLL.Interfaces;
using Fitness.DAL;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL
{
    public class RecordAndPlanBLL : IRecordAndPlanBLL
    {
        RecordAndPlanDAL recordAndPlanDAL = new();

        // 创建一条饮食记录
        public string Create(FoodPlanInfo foodPlanInfo)
        {

            FoodPlan foodPlan = new()
            {
                foodPlanID = 0,
                userID = foodPlanInfo.userID,
                createTime = new DateTime(1919, 10, 12),
                date = foodPlanInfo.date,
                mealType = foodPlanInfo.mealType,
                state = foodPlanInfo.state
        
            };
            
            bool foodPlanID = recordAndPlanDAL.InsertFoodPlan(foodPlan);

            if (!foodPlanID) 
            {
                return "插入失败";
            }

            for(int i = 0;i<foodPlanInfo.foods.Count;i++)
            {

                FoodPlanFood foodPlanFood = new()
                {
                    foodPlanID = 1,
                    foodName = foodPlanInfo.foods[i].foodName,
                    foodAmount = foodPlanInfo.foods[i].quantity
                };
                recordAndPlanDAL.InsertFoodPlanFood(foodPlanFood);
            }


            return "插入成功";
        }
    }
}
