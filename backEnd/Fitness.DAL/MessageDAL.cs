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
                messageType = Convert.ToString(row["messageType"]),
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
            string cmdText = "INSERT INTO \"Messages\" (\"senderID\", \"receiverID\", \"messageType\", \"Content\", \"sendTime\",\"isRead\") " +
                             "VALUES ( :senderID, :receiverID, :messageType, :Content, :sendTime,:isRead)";

            OracleParameter[] parameters = {
            new OracleParameter("senderID", OracleDbType.Int32) { Value = message.senderID },
            new OracleParameter("receiverID", OracleDbType.Int32) { Value = message.receiverID },
            new OracleParameter("messageType", OracleDbType.NVarchar2) { Value = message.messageType },
            new OracleParameter("Content", OracleDbType.NVarchar2) { Value = message.Content },
            new OracleParameter("sendTime", OracleDbType.Date) { Value = message.sendTime },
            new OracleParameter("isRead",OracleDbType.Int32){ Value =message.isRead}
        };
            return OracleHelper.ExecuteNonQuery(cmdText, transaction, parameters) > 0;
        }

        public static List<Message> GetUnreadMessages(int userId)
        {
            string query = @"SELECT * FROM Messages
                         WHERE receiverID = :receiverID AND isRead = 0
                         ORDER BY sendTime DESC";

            OracleParameter[] parameters = {
            new OracleParameter("receiverID", userId)
        };
            DataTable dataTable = OracleHelper.ExecuteTable(query, parameters);
            List<Message> messages = ToModelList(dataTable);
            return messages;
        }

        public static void MarkMessagesAsRead(List<int> messageIds)
        {
            if (messageIds == null || messageIds.Count == 0) return;

            string query = "UPDATE \"Messages\" SET \"isRead\" = 1 WHERE \"messageID\" IN (";

            for (int i = 0; i < messageIds.Count; i++)
            {
                query += "messageID" + i;
                if (i < messageIds.Count - 1)
                    query += ", ";
            }
            query += ")";

            List<OracleParameter> parameters = new List<OracleParameter>();
            for (int i = 0; i < messageIds.Count; i++)
            {
                parameters.Add(new OracleParameter("messageID" + i, messageIds[i]));
            }

            OracleHelper.ExecuteNonQuery(query, null,parameters.ToArray());
        }



        // 根据用户ID获取消息列表
        public static List<Message> GetMessageByUserID(int userID)
        {
            string cmdText = "SELECT * FROM \"Messages\" WHERE \"receiverID\" = :receiverID UNION SELECT * FROM \"Messages\" WHERE \"senderID\" = :receiverID";

            OracleParameter[] parameters = {
            new OracleParameter("receiverID", OracleDbType.Int32) { Value = userID }
        };

            DataTable table = OracleHelper.ExecuteTable(cmdText, parameters);
            return ToModelList(table);
        }

    }
}
