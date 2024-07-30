using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL.Interfaces
{
    public interface IFoodPlanBLL
    {
        public FoodPlanRes Create(FoodPlanInfo foodPlanInfo);

        public GetAllFoodPlanNoDetailsRes GetAllNoDetails(int userID);

        public GetAllFoodPlanDetailsRes GetAllDetails(int userID);

        public GetOneFoodPlanDetailRes GetOneDetail(int foodPlanID);

        public MessageRes DeleteFoodPlan(int foodPlanID);

        public MessageRes UpdateFoodPlan(UpdateFoodPlanInfo updateFoodPlanInfo);

        public MessageRes UpdateFoodPlanState(int foodPlanID,int state);

        public FoodsRes GetALLFoodsInfo();
    }
}
