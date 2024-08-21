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

        // 预设食物表
        public MessageRes InsertFoodInfo(string foodName, int calorie);

        public MessageRes DeleteFoodInfo(string foodName);

        public MessageRes UpdateFoodInfo(string foodName, int calorie);

        public FoodsRes GetALLFoodsInfo();

        // 食谱
        public RecipesRes InsertRecipe(RecipesInfo recipesInfo);

        public MessageRes DeleteRecipe(int recipeID);

        public MessageRes UpdateRecipe(UpdateRecipesInfo updateRecipesInfo);

        public Recipes GetAllRecipes();


    }
}
