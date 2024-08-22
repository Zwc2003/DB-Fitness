using FitnessWebAPI.DAL.Core;
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
    public class MessageDAL
    {
        // 将DataRow转换为Messages对象
        public static Message ToModel(DataRow row)
        {
            return new Message
            {
                messageID = Convert.ToInt32(row["messageID"]),
                senderID = Convert.ToInt32(row["senderID"]),
                receiverID = Convert.ToInt32(row["receiverID"]),
                messageType = Convert.ToChar(row["messageType"]),
                Content = row["Content"].ToString(),
                sendTime = Convert.ToDateTime(row["sendTime"])
            };
        }

        // 将DataTable转换为Messages对象列表
        public static List<Message> ToModelList(DataTable table)
        {
            List<Message> messagesList = new List<Message>();
            foreach (DataRow row in table.Rows)
            {
                messagesList.Add(ToModel(row));
            }
            return messagesList;
        }

        // 插入新的消息记录
        public static bool Insert(Message message, OracleTransaction transaction = null)
        {
            string cmdText = "INSERT INTO Messages (messageID, senderID, receiverID, messageType, Content, sendTime) " +
                             "VALUES (:messageID, :senderID, :receiverID, :messageType, :Content, :sendTime)";

            OracleParameter[] parameters = {
            new OracleParameter("messageID", OracleDbType.Int32) { Value = message.messageID },
            new OracleParameter("senderID", OracleDbType.Int32) { Value = message.senderID },
            new OracleParameter("receiverID", OracleDbType.Int32) { Value = message.receiverID },
            new OracleParameter("messageType", OracleDbType.Char) { Value = message.messageType },
            new OracleParameter("Content", OracleDbType.Varchar2) { Value = message.Content },
            new OracleParameter("sendTime", OracleDbType.Date) { Value = message.sendTime }
        };

            return OracleHelper.ExecuteNonQuery(cmdText, transaction, parameters) > 0;
        }

        // 根据用户ID获取消息列表
        public static List<Message> GetMessageByUserID(int userID)
        {
            string cmdText = "SELECT * FROM Messages WHERE receiverID = :receiverID";

            OracleParameter[] parameters = {
            new OracleParameter("receiverID", OracleDbType.Int32) { Value = userID }
        };

            DataTable table = OracleHelper.ExecuteTable(cmdText, parameters);
            return ToModelList(table);
        }

    }
}
