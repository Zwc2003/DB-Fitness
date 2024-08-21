using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.DAL.Core;
using Newtonsoft.Json;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Fitness.DAL;
using Fitness.BLL;
using Fitness.Models;

namespace Fitness.BLL
{
    public sealed class UserFitnessPlanGoalBll
    {

        private static readonly UserFitnessPlanGoalBll instance = new UserFitnessPlanGoalBll();
        private UserFitnessPlanGoalBll()
        {
        }
        public static UserFitnessPlanGoalBll Instance
        {
            get
            {
                return instance;
            }
        }

        public static string Set(int userId, string goal, int duration)
        {
            if (!UserFitnessPlanGoalDal.Exist(userId))
                UserFitnessPlanGoalDal.Init(userId);
            UserFitnessPlanGoalDal.Update(userId, goal, duration);
            return JsonConvert.SerializeObject(new
            {
                isSuccessful = "true"
            });
        }

        public static string Get(int userId)
        {
            return UserFitnessPlanGoalDal.Get(userId);
        }


    }
}
