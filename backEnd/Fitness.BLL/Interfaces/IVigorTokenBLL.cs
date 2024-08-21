using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL.Interfaces
{
    public interface IVigorTokenBLL
    {
        public BalanceRes GetBalance(int userID);

        public MessageRes UpdateBalance(int userID, string reason, int change);

    }
}
