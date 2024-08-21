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
    public sealed class PhysicalTestBll
    {
        private static readonly PhysicalTestBll instance = new PhysicalTestBll();
        private PhysicalTestBll()
        {
        }
        public static PhysicalTestBll Instance
        {
            get
            {
                return instance;
            }
        }

        public static string Update(int userId, int pushups, int squats, int situps, int pullups, int longDistance)
        {
            string s = PhysicalTestDal.Update(userId, pushups, squats, situps, pullups, longDistance);
            return JsonConvert.SerializeObject(new
            {
                message = s
            });
        }

        public static string Get(int userId)
        {
            return PhysicalTestDal.Get(userId);
        }
    }
}
