﻿// 引入Oracle.ManagedDataAccess.Client命名空间，用于Oracle数据库操作
using Oracle.ManagedDataAccess.Client;
// 引入System.Data命名空间，用于数据操作
using System.Data;

namespace FitnessWebAPI.DAL.Core
{
    // 定义一个名为OracleHelper的公共类，用于Oracle数据库操作
    public class OracleHelper
    {
        // 私有静态只读字段，存储数据库连接字符串
        private static readonly string connectionString = "Data Source=120.26.138.61:1521/XE;User Id=FITNESSDB;Password=tongjiSSEDBMS2024;";

/*        public static OracleConnection GetConnection()
        {
            // 创建并返回新的 OracleConnection 对象
            return new OracleConnection(connectionString);
        }

        public static DataTable ExecuteTable(string cmdText, OracleTransaction transaction = null, params OracleParameter[] oracleParameters)
        {
            OracleConnection conn = null;
            OracleTransaction localTransaction = null;
            try
            {
                // 如果传入的事务为空，则创建新的连接和事务
                if (transaction == null)
                {
                    conn = new OracleConnection(connectionString);
                    conn.Open();
                    localTransaction = conn.BeginTransaction();
                }
                else
                {
                    // 使用传入的事务和连接
                    conn = transaction.Connection;
                    localTransaction = transaction;
                }

                // 使用查询命令和连接创建OracleCommand对象
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    cmd.Transaction = localTransaction;
                    cmd.Parameters.AddRange(oracleParameters);

                    // 创建OracleDataAdapter对象，用于填充数据
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    // 创建DataSet对象
                    DataSet ds = new DataSet();
                    // 使用适配器填充DataSet
                    adapter.Fill(ds);

                    // 提交事务
                    if (localTransaction != null && localTransaction.Connection == conn && transaction == null)
                    {
                        localTransaction.Commit();
                    }

                    // 返回DataSet的第一个DataTable
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                // 发生异常时回滚事务
                if (localTransaction != null && localTransaction.Connection == conn && transaction == null)
                {
                    localTransaction.Rollback();
                }
                Console.WriteLine($"Error executing table command: {ex.Message}");
                throw;
            }
            finally
            {
                // 关闭数据库连接
                if (conn != null && conn.State != ConnectionState.Closed && transaction == null)
                {
                    conn.Close();
                }
            }
        }

        // 执行非查询命令（如INSERT、UPDATE、DELETE），返回受影响的行数
        public static int ExecuteNonQuery(string cmdText, OracleTransaction transaction = null, params OracleParameter[] oracleParameters)
        {
            OracleConnection conn = null;
            OracleTransaction localTransaction = null;
            try
            {
                // 如果传入的事务为空，则创建新的连接和事务
                if (transaction == null)
                {
                    conn = new OracleConnection(connectionString);
                    conn.Open();
                    localTransaction = conn.BeginTransaction();
                }
                else
                {
                    // 使用传入的事务和连接
                    conn = transaction.Connection;
                    localTransaction = transaction;
                }

                // 创建OracleCommand对象并设置其属性
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    cmd.Transaction = localTransaction;
                    cmd.Parameters.AddRange(oracleParameters);
                    // 执行非查询命令并获取受影响的行数
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // 提交事务
                    if (localTransaction != null && localTransaction.Connection == conn && transaction == null)
                    {
                        localTransaction.Commit();
                    }

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                // 发生异常时回滚事务
                if (localTransaction != null && localTransaction.Connection == conn && transaction == null)
                {
                    localTransaction.Rollback();
                }
                Console.WriteLine($"Error executing non-query command: {ex.Message}");
                throw;
            }
            finally
            {
                // 关闭数据库连接
                if (conn != null && conn.State != ConnectionState.Closed && transaction == null)
                {
                    conn.Close();
                }
            }
        }*/


        // 执行查询命令并返回DataTable
        public static DataTable ExecuteTable(string cmdText, params OracleParameter[] oracleParameters)
        {
            // 使用连接字符串创建OracleConnection对象并在块结束时自动关闭连接
            using OracleConnection conn = new(connectionString);
            // 打开数据库连接
            conn.Open();
            // 使用查询命令和连接创建OracleCommand对象
            using OracleCommand cmd = new(cmdText, conn);
            // 添加查询参数
            cmd.Parameters.AddRange(oracleParameters);
            // 创建OracleDataAdapter对象，用于填充数据
            OracleDataAdapter adapter = new(cmd);
            // 创建DataSet对象
            DataSet ds = new();
            // 使用适配器填充DataSet
            adapter.Fill(ds);
            // 返回DataSet的第一个DataTable
            return ds.Tables[0];
        }

        // 执行非查询命令（如INSERT、UPDATE、DELETE），返回受影响的行数
        public static int ExecuteNonQuery(string cmdText, OracleTransaction transaction = null, params OracleParameter[] oracleParameters)
        {
            OracleConnection conn = null;
            OracleTransaction localTransaction = null;
            try
            {
                // 如果传入的事务为空，则创建新的连接和事务
                if (transaction == null)
                {
                    conn = new OracleConnection(connectionString);
                    conn.Open();
                    localTransaction = conn.BeginTransaction();
                }
                else
                {
                    // 使用传入的事务和连接
                    conn = transaction.Connection;
                    localTransaction = transaction;
                }

                // 创建OracleCommand对象并设置其属性
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    cmd.Transaction = localTransaction;
                    cmd.Parameters.AddRange(oracleParameters);
                    // 执行非查询命令并获取受影响的行数
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // 提交事务
                    if (localTransaction != null && localTransaction.Connection == conn)
                    {
                        localTransaction.Commit();
                    }

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                // 发生异常时回滚事务
                if (localTransaction != null && localTransaction.Connection == conn)
                {
                    localTransaction.Rollback();
                }
                Console.WriteLine($"Error executing non-query command: {ex.Message}");
                throw;
            }
            finally
            {
                // 关闭数据库连接
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        // 执行查询命令并返回单个值
        public static object ExecuteScalar(string cmdText, params OracleParameter[] oracleParameters)
        {
            // 使用连接字符串创建OracleConnection对象并在块结束时自动关闭连接
            using OracleConnection conn = new(connectionString);
            // 打开数据库连接
            conn.Open();
            // 使用查询命令和连接创建OracleCommand对象
            using OracleCommand cmd = new(cmdText, conn);
            // 添加查询参数
            cmd.Parameters.AddRange(oracleParameters);
            // 执行查询并返回结果
            return cmd.ExecuteScalar();
        }
    }
}

