using Oracle.ManagedDataAccess.Client;
using System.Collections.Specialized;
using System.Data;

namespace Fitness.DAL.Core
{
    public class OracleHelper
    {
        // 数据库连接字段
        //public static string connectionString = "Data Source=localhost:1521/orcl;User Id=C##gd;Password=123456;";
        private static readonly string connectionString = "Data Source=120.26.138.61:1521/XE;User Id=FITNESSDB;Password=tongjiSSEDBMS2024;";
        public static DataTable ExecuteTable(string cmdText, params OracleParameter[] oracleParameters)
        {
            using OracleConnection conn = new(connectionString);
            conn.Open();
            using OracleCommand cmd = new(cmdText, conn);
            cmd.Parameters.AddRange(oracleParameters);
            OracleDataAdapter adapter = new(cmd);
            DataSet ds = new();
            adapter.Fill(ds);
            return ds.Tables[0];
        }


        public static int ExecuteNonQuery(string cmdText, OracleTransaction transaction = null, params OracleParameter[] oracleParameters)
        {
            OracleConnection conn = null;
            OracleTransaction localTransaction = null;
            try
            {
                if (transaction == null)
                {
                    conn = new OracleConnection(connectionString);
                    conn.Open();
                    localTransaction = conn.BeginTransaction();
                }
                else
                {
                    conn = transaction.Connection;
                    localTransaction = transaction;
                }

                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    cmd.Transaction = localTransaction;
                    cmd.Parameters.AddRange(oracleParameters);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (localTransaction != null && localTransaction.Connection == conn)
                    {
                        localTransaction.Commit();
                    }

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                if (localTransaction != null && localTransaction.Connection == conn)
                {
                    localTransaction.Rollback();
                }
                Console.WriteLine($"Error executing non-query command: {ex.Message}");
                throw;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public static object ExecuteScalar(string cmdText, params OracleParameter[] oracleParameters)
        {
            using OracleConnection conn = new(connectionString);
            conn.Open();
            using OracleCommand cmd = new(cmdText, conn);
            cmd.Parameters.AddRange(oracleParameters);
            return cmd.ExecuteScalar();
        }
    }
}
