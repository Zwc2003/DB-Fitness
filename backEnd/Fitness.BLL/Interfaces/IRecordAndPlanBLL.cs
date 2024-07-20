using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL.Interfaces
{
    public interface IRecordAndPlanBLL
    {
        public string Create(FoodPlanInfo foodPlanInfo);
    }
}
