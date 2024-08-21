using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.DAL.Core;
using Newtonsoft.Json;
using System.Data;
using Fitness.DAL;
using Fitness.BLL;
using Fitness.Models;

namespace Fitness.DAL
{
    public sealed class WorkoutDal
    {
        private static readonly WorkoutDal instance = new WorkoutDal();
        private WorkoutDal()
        {
        }
        public static WorkoutDal Instance
        {
            get
            {
                return instance;
            }
        }

        public static List<Exercise> GetExercises(string workoutName)
        {
            string selectCommand = "SELECT \"exerciseName\" FROM \"Workout\" WHERE \"workoutName\" = :workoutName";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter(":workoutName", OracleDbType.NVarchar2) { Value = workoutName }
            };

            DataTable exerciseNamesTable = OracleHelper.ExecuteTable(selectCommand, parameters);
            List<Exercise> exercises = new List<Exercise>();
            foreach (DataRow row in exerciseNamesTable.Rows)
            {
                string exerciseName = row["exerciseName"].ToString();
                Exercise exercise = ExerciseDal.Get(exerciseName);
                if (exercise != null)
                {
                    exercises.Add(exercise);
                }
            }
            return exercises;
        }

        public static DataTable Get(string workoutName)
        {
            string workoutQuery = "SELECT \"exerciseName\", \"coverUrl\" FROM \"Workout\" WHERE \"workoutName\" = :workoutName";
            OracleParameter[] workoutParams =
            {
                        new OracleParameter(":workoutName", OracleDbType.NVarchar2) { Value = workoutName }
                    };
            DataTable workoutTable = OracleHelper.ExecuteTable(workoutQuery, workoutParams);
            return workoutTable;
        }
    }
}