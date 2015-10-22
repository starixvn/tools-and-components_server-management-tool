using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class TaskService : BaseService
    {
        public List<TaskModel> GetTasks()
        {
            var sql = "select * from tasks";
            var tasks = new List<TaskModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    tasks.Add(new TaskModel
                    {
                        TaskId = Convert.ToInt32(reader["Id"]),
                        TaskName = reader["Name"].ToString(),
                        Action = reader["Action"].ToString()
                    });
                }
            }

            return tasks;
        }

        public Result Insert(TaskModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into Tasks('name','action') values('" + criteria.TaskName + "','" + criteria.Action + "')";
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

        public Result Update(TaskModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "update tasks set name='" + criteria.TaskName + "', action='" + criteria.Action + "' where id=" + criteria.TaskId;
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