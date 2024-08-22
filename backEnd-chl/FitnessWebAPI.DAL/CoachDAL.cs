﻿using FitnessWebAPI.DAL.Core;
using FitnessWebAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessWebAPI.DAL
{
    public class CoachDAL
    {
        // 将 DataRow 转换为 Coach 对象
        public static Coach ToModel(DataRow row)
        {
            Coach coach = new Coach
            (
               Convert.ToInt32(row["coachID"]),
               row["userName"].ToString(),
               Convert.ToInt32(row["Age"]),
               row["Gender"].ToString(),
               row["iconURL"].ToString(),
               Convert.ToInt32(row["isMember"]),
               row["coachName"].ToString()
            );
            return coach;
        }

        // 将 DataTable 转换为 Coach 对象列表
        public static List<Coach> ToModelList(DataTable dt)
        {
            List<Coach> coaches = new List<Coach>();
            foreach (DataRow row in dt.Rows)
            {
                coaches.Add(ToModel(row));
            }
            return coaches;
        }

        // 插入新的 Coach 记录
        public static bool Insert(Coach coach)
        {
            try
            {
                string sql = "INSERT INTO \"Coach\" (\"coachID\",\"userName\", \"Age\", \"Gender\", \"iconURL\", \"isMember\", \"coachName\") VALUES ( :coachID,:userName, :Age, :Gender, :iconURL, :isMember, :coachName)";

                OracleParameter[] parameters = new OracleParameter[]
                {
                new OracleParameter("coachID", OracleDbType.Int32) { Value = coach.coachID },
                new OracleParameter("userName", OracleDbType.Varchar2) { Value = coach.userName },
                new OracleParameter("Age", OracleDbType.Int32) { Value = coach.Age },
                new OracleParameter("Gender", OracleDbType.Varchar2) { Value = coach.Gender },
                new OracleParameter("iconURL", OracleDbType.Varchar2) { Value = coach.iconURL },
                new OracleParameter("isMember", OracleDbType.Int32) { Value = coach.isMember },
                new OracleParameter("coachName", OracleDbType.Varchar2) { Value = coach.coachName }
                };
                OracleHelper.ExecuteNonQuery(sql,null, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

}
