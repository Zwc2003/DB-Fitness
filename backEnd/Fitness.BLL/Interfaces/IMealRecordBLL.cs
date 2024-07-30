using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL.Interfaces
{
    public interface IMealRecordBLL
    {
        public MealRecordRes Create(MealRecordInfo mealRecordInfo);

        public GetAllMealRecordsNoDetailsRes GetAllNoDetails(int userID);

        public GetAllMealRecordsDetailsRes GetAllDetails(int userID);

        public GetOneMealRecordDetailsRes GetOneDetail(int recordID);

        public MessageRes DeleteMealRecord(int recordID);
    }
}
