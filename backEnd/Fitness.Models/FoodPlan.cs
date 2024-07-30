using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class Foods
    {
        public string foodName { get; set; }

        public string calorie { get; set; }
    }

    

    public class MealRecordsFood
    {
        public int recordID { get; set; }

        public string foodName { get; set; }

        public int foodAmount { get; set; }

    }

    public class FoodPlan
    {
        public int? foodPlanID { get; set; }

        public int userID { get; set; }

        public DateTime? createTime { get; set; }

        public DateTime? date { get; set; }

        public int mealType { get; set; }

        public int state { get; set; }

        public int? numsOfTypes { get; set; }
    }

    public class FoodPlanFood
    {
        public int foodPlanID { get; set; }

        public string foodName { get; set; }

        public int foodAmount { get; set; }
    }


    public class Food
    {
        public string foodName { get; set; }
        public int quantity { get; set; }
    }

   

    // 定义创建饮食计划功能-前端发送数据格式
    public class FoodPlanInfo
    {
        public int userID { get; set; }
        public DateTime? date { get; set; }
        public int mealType { get; set; }
        public int state { get; set; }
        public List<Food> foods { get; set; }

    }

    // 定义创建饮食计划功能-后端响应格式
    public class FoodPlanRes
    {
        public string message { get; set; }

        public int planID { get; set; }
    }


    
    public class SinglePlanNoDetail
    {
        public int foodPlanID { get; set; }
        public DateTime date { get; set; }
        public int mealType { get; set; }
        public int state { get; set; }
        public int numOfTypes { get; set; }
    }

    // 定义获取某个用户所有饮食计划缩略信息-后端响应
    public class GetAllFoodPlanNoDetailsRes 
    {

        public List<SinglePlanNoDetail> plans { get; set; } = new ();
    }

    public class GetOneFoodPlanDetailRes
    {
        public int foodPlanID { get; set; }
        public DateTime date { get; set; }
        public int mealType { get; set; }
        public int state { get; set; }
        public int numOfTypes { get; set; }
        public List<Food> foods { get; set; } = new();

    }

    // 定义获取某个用户所有饮食计划详细信息-后端响应
    public class GetAllFoodPlanDetailsRes
    {

        public List<GetOneFoodPlanDetailRes> plans { get; set; } = new();
    }


    // 定义更新FoodPlan的前端发送数据格式
    public class UpdateFoodPlanInfo
    {
        public int foodPlanID { get; set; }
        public int mealType { get; set; }
        public List<Food> foods { get; set; } = new();
    }

    
    public class MessageRes
    {
        public string message { get; set; }
    }

    public class FoodInfo
    {
        public string foodName { get;set; }
        public int calorie { get; set; }
    }

    public class FoodsRes
    {
        public List<FoodInfo> foodsInfo { get; set; } = new();
    }

}
