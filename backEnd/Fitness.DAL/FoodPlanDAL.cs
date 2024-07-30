using Fitness.DAL.Core;
using Fitness.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.DAL
{
    public class FoodPlanDAL
    {


        // 成功插入一条饮食计划时，返回这条饮食计划的foodPlanID
        public int InsertFoodPlan(FoodPlan foodPlan)
        {
            int foodPlanID = -1;

            try
            {
                string sql = "INSERT INTO \"FoodPlan\" (\"userID\", \"createTime\", \"date\", \"mealType\", \"state\", \"numOfTypes\")" +
                             " VALUES (:\"userID\", :\"createTime\", :\"date\", :\"mealType\", :\"state\", :\"numOfTypes\")" +
                             " RETURNING \"foodPlanID\" INTO :\"foodPlanID\"";

                OracleParameter[] oracleParameters = new OracleParameter[]
                {
                    new OracleParameter("\"userID\"", OracleDbType.Int32) { Value = foodPlan.userID },
                    new OracleParameter("\"createTime\"", OracleDbType.TimeStamp) { Value = DateTime.Now },
                    new OracleParameter("\"date\"", OracleDbType.Date) { Value = foodPlan.date },
                    new OracleParameter("\"mealType\"", OracleDbType.Int32) { Value = foodPlan.mealType },
                    new OracleParameter("\"state\"", OracleDbType.Int32) { Value = foodPlan.state },
                    new OracleParameter("\"numOfTypes\"", OracleDbType.Int32) { Value = foodPlan.numsOfTypes },
                    new OracleParameter("\"foodPlanID\"", OracleDbType.Int32) { Direction = ParameterDirection.Output }
                };

                OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);

                foodPlanID = Convert.ToInt32(((Oracle.ManagedDataAccess.Types.OracleDecimal)oracleParameters[6].Value).Value);

                return foodPlanID;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error occurred while inserting food plan: {ex.Message}");
                throw;
            }
        }



        public bool InsertFoods(Foods food)
        {
            try
            {
                string sql = "INSERT INTO Foods(foodName, calorie) VALUES(:foodName, :calorie)";
                OracleParameter[] oracleParameters = new OracleParameter[]
                {
            new OracleParameter("foodName", OracleDbType.NVarchar2) { Value = food.foodName },
            new OracleParameter("calorie", OracleDbType.Int32) { Value = food.calorie }
                };

                OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error occurred while inserting food: {ex.Message}");
                throw;
            }
        }


        public bool InsertFoodPlanFood(FoodPlanFood foodPlanFood)
        {
            try
            {
                string sql = "INSERT INTO \"FoodPlanFood\"(\"foodPlanID\", \"foodName\", \"foodAmount\") " +
                             "VALUES(:\"foodPlanID\", :\"foodName\", :\"foodAmount\")";

                OracleParameter[] oracleParameters = new OracleParameter[]
                {
                    new OracleParameter("\"foodPlanID\"", OracleDbType.Int32) { Value = foodPlanFood.foodPlanID },
                    new OracleParameter("\"foodName\"", OracleDbType.NVarchar2) { Value = foodPlanFood.foodName },
                    new OracleParameter("\"foodAmount\"", OracleDbType.Int32) { Value = foodPlanFood.foodAmount }
                };

                OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error occurred while inserting food plan food: {ex.Message}");
                throw;
            }
        }



        // 不好用，不够灵活，代码可以参考
        private static FoodPlan FoodPlanToModel(DataRow row)
        {
            FoodPlan foodPlan = new()
            {
                foodPlanID = Convert.ToInt32(row["foodPlanID"]),
                userID = Convert.ToInt32(row["userID"]),
                createTime = Convert.ToDateTime(row["createTime"]),
                date = Convert.ToDateTime(row["date"]),
                mealType = Convert.ToInt32(row["mealType"]),
                state = Convert.ToInt32(row["state"])
            };

            return foodPlan;
        }
        // 不够灵活，代码可以参考
        public static List<FoodPlan> FoodPlanToModelList(DataTable dt)
        {
            List<FoodPlan> foodPlans = new();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                FoodPlan foodPlan = FoodPlanToModel(dr);
                foodPlans.Add(foodPlan);
            }
            return foodPlans;
        }

        public DataTable GetFoodPlanByUsrID(int userID)
        {
            try
            {
                string sql = "SELECT * FROM \"FoodPlan\" WHERE \"userID\"=:\"userID\"";
                DataTable dt = OracleHelper.ExecuteTable(sql,
                    new OracleParameter("\"userID\"", OracleDbType.Int32) { Value = userID });

                return dt;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);     
                return null;
            }
        }

        public DataTable GetFoodPlanFoodsByID(int foodPlanID)
        {
            try
            {
                string sql = "SELECT * FROM \"FoodPlanFood\" WHERE \"foodPlanID\"=:\"foodPlanID\"";
                DataTable dt = OracleHelper.ExecuteTable(sql,
                    new OracleParameter("\"foodPlanID\"", OracleDbType.Int32) { Value = foodPlanID });

                return dt;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public DataTable GetFoodPlanByPlanID(int foodPlanID)
        {
            try
            {
                string sql = "SELECT * FROM \"FoodPlan\" WHERE \"foodPlanID\"=:\"foodPlanID\"";
                DataTable dt = OracleHelper.ExecuteTable(sql,
                    new OracleParameter("\"foodPlanID\"", OracleDbType.Int32) { Value = foodPlanID });

                return dt;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

       
        public bool UpdateFoodPlanByPlanID(int foodPlanID,int mealType,int numOfTypes)
        {

            try
            {
                string sql = "UPDATE \"FoodPlan\" SET \"mealType\"=:mealType, \"numOfTypes\"=:numOfTypes " +
                                "WHERE \"foodPlanID\"=:foodPlanID";

                OracleParameter[] oracleParameters = new OracleParameter[]
                {
                    
                    new OracleParameter("mealType", OracleDbType.Int32) { Value = mealType },
                    new OracleParameter("numOfTypes", OracleDbType.Int32) { Value = numOfTypes },
                    new OracleParameter("foodPlanID", OracleDbType.Int32) { Value = foodPlanID },
                };

                OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);
                

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool UpdateFoodPlanState(int foodPlanID, int state)
        {

            try
            {
                string sql = "UPDATE \"FoodPlan\" SET \"state\"=:state " +
                                "WHERE \"foodPlanID\"=:foodPlanID";

                OracleParameter[] oracleParameters = new OracleParameter[]
                {

                    new OracleParameter("state", OracleDbType.Int32) { Value = state },
                    new OracleParameter("foodPlanID", OracleDbType.Int32) { Value = foodPlanID },
                };

                OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);


                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public bool DeleteFoodPlanByPlanID(int foodPlanID)
        {
            try
            {
                string sql = "DELETE FROM \"FoodPlan\" WHERE \"foodPlanID\" =:\"foodPlanID\"";
                OracleParameter[] oracleParameters = new OracleParameter[]
                {
                    new OracleParameter("foodPlanID", OracleDbType.Int32) { Value = foodPlanID }
                };
                OracleHelper.ExecuteNonQuery(sql,null, oracleParameters);
                
                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeletFoodPlanFoodByPlanID(int foodPlanID)
        {
            try
            {
                string sql = "DELETE FROM \"FoodPlanFood\" WHERE \"foodPlanID\" =:\"foodPlanID\"";
                OracleParameter[] oracleParameters = new OracleParameter[]
                {
                    new OracleParameter("foodPlanID", OracleDbType.Int32) { Value = foodPlanID }
                };
                OracleHelper.ExecuteNonQuery(sql, null, oracleParameters);

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public DataTable GetALLFoodInfoData()
        {
            try
            {
                string sql = "SELECT * FROM \"Foods\" ";
                OracleParameter[] oracleParameters = new OracleParameter[]{};
                DataTable dt = OracleHelper.ExecuteTable(sql, oracleParameters);
                

                return dt;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
