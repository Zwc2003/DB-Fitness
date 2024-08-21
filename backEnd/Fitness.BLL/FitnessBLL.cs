using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.DAL;
using Fitness.BLL;
using Fitness.Models;

namespace Fitness.BLL
{
    public sealed class FitnessBll
    {
        private static readonly FitnessBll instance = new FitnessBll();
        private FitnessBll()
        {
        }
        public static FitnessBll Instance
        {
            get
            {
                return instance;
            }
        }

        public static string Update(int userId, double height, double weight, double BMI, double bodyFatRate)
        {
            string s = FitnessDal.Update(userId, height, weight, BMI, bodyFatRate);
            return JsonConvert.SerializeObject(new
            {
                message = s
            });
        }

        public static string Get(int userId)
        {
            return FitnessDal.Get(userId);
        }
    }
}
