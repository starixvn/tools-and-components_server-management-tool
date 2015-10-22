using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class SiteTaskService : BaseService
    {
        public List<SiteTaskModel> GetSiteTasks()
        {
            var sql = "select * from sitetask";
            var tasks = new List<SiteTaskModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    tasks.Add(new SiteTaskModel
                    {
                        TaskId = Convert.ToInt32(reader["TaskId"]),
                        SiteId = Convert.ToInt32(reader["SiteId"]),
                        Id = Convert.ToInt32(reader["Id"]),
                        IsAssigned = Convert.ToBoolean(reader["Assigned"])
                    });
                }
            }

            return tasks;
        }

        public bool Existing(SiteTaskModel criteria)
        {
            var sql = "select * from sitetask where id = " + criteria.Id;

            var result = QueryDataScalar(sql);

            return result != null;
        }

        public Result Insert(SiteTaskModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into SiteTask('siteid','taskid','assigned') values(" + criteria.SiteId + "," + criteria.TaskId + "," + criteria.IsAssigned + ")";
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ex.Message
                };
            }

            return new Result
            {
                ErrorCode = errorCode,
                ErrorMessage = string.Empty
            };
        }

        public Result delete(SiteTaskModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "delete sitetask where id=" + criteria.Id;
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ex.Message
                };
            }

            return new Result
            {
                ErrorCode = errorCode,
                ErrorMessage = string.Empty
            };
        }
    }
}