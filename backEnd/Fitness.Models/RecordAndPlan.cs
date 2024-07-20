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

    public class MealRecords
    {
        public int recordID { get; set; }

        public int userID { get; set; }

        public string createTime { get; set; }

        public string mealTime { get; set; }

        public string? diningAdvice { get; set; }

        public string? mealPhoto { get; set; }
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
    }

    public class FoodPlanFood
    {
        public int foodPlanID { get; set; }

        public string foodName { get; set; }

        public int foodAmount { get; set; }
    }

    public class FoodPlanInfo
    {
        public int userID { get; set; }
        public DateTime? date { get; set; }
        public int mealType { get; set; }
        public int state { get; set; }
       
        public class Food
        {
            public string foodName { get; set; }
            public int quantity { get; set; }
        }

        public List<Food> foods { get; set; }
    }

    
}
