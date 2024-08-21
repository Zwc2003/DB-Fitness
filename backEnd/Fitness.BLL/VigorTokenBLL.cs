using Fitness.BLL.Interfaces;
using Fitness.DAL;
using System.Data;
using Fitness.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.Models;
using Fitness.BLL.Core;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Fitness.BLL
{
    public class VigorTokenBLL: IVigorTokenBLL
    {
        VigorTokenDAL vigorTokenDAL = new();

        public BalanceRes GetBalance(int userID)
        {
            DataTable dt = vigorTokenDAL.GetVigorTokenBalance(userID);

            DataRow dr = dt.Rows[0];

            BalanceRes res = new()
            {
                balance = Convert.ToInt32(dr["vigorTokenBalance"])
            };

            return res;
        }

        public MessageRes UpdateBalance(int userID, string reason, int change)
        {
            int balance = GetBalance(userID).balance;

            VigorTokenInfo vigorTokenInfo = new()
            {
                userID = userID,

                reason = reason,

                change = change,

                balancce = balance + change,

                createTime = DateTime.Now

            };

            MessageRes res = new()
            {
                message = "更新活力币余额失败!"
            };

            if(vigorTokenDAL.UpdateVigorTokenBalance(change,userID))
            {
                if(vigorTokenDAL.InsertVigorTokenRecord(vigorTokenInfo))
                {
                    res.message = "更新活力币余额成功！";
                }
            }

            return res;
        }

        

    }
}
