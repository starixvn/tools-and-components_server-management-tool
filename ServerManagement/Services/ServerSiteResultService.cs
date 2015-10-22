using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class ServerSiteResultService : BaseService
    {
        public List<ReplyModel> Replies()
        {
            var sql = "select * from serverstatus";
            var replies = new List<ReplyModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    replies.Add(new ReplyModel
                    {
                        ServerSiteId = Convert.ToInt32(reader["serversiteid"]),
                        ReplyDate = reader["replydate"].ToString(),
                        Id = Convert.ToInt32(reader["id"]),
                        IsResult = reader["isresult"].ToString() == "True"
                    });
                }
            }

            return replies;
        }

       
    }
}