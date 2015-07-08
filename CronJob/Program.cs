using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using System.IO;
using System.Xml;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;

namespace CronJob
{
    class Program
    {
        static SQLiteConnection sqliteConnection = null;

        static void Main(string[] args)
        {
            var rangeDate = Convert.ToInt32(ConfigurationManager.AppSettings["RangeData"].ToString());
            var path = ConfigurationManager.AppSettings["DataPath"].ToString();
            sqliteConnection = ConnectionString(path);                 

            var localDate = DateTime.Now;            
            var serverSites = GetServerSite();

            foreach (var serverSite in serverSites)
            {
                var isResult = PingHost(serverSite.ServerIP);

                UpdateReply(serverSite.Id, localDate, isResult, rangeDate);
            }
        }

        public static bool PingHost(string nameOrAddress)
        {
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);

                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        private static SQLiteConnection ConnectionString(string dataSourcePath)
        {
            var sqliteConn = new SQLiteConnection("Data Source=" + dataSourcePath + ";Version=3;");


            if (sqliteConn.State == ConnectionState.Closed)
            {
                sqliteConn.Open();

            }

            return sqliteConn;
        }

        private static string UpdateServerIp(string ip)
        {
            string sql = "select count(*) from server where ip='" + ip + "'";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            var result = Convert.ToInt32(command.ExecuteScalar());

            if (result == 0)
            {
                sql = "insert into server('ip','name') values ('" + ip + "','" + ip + "')";
                command = new SQLiteCommand(sql, sqliteConnection);
                command.ExecuteNonQuery();
            }

            return ip;
        }

        private static string UpdateSite(string siteName)
        {
            string sql = "select count(*) from site where name='" + siteName + "'";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            var result = Convert.ToInt32(command.ExecuteScalar());

            if (result == 0)
            {
                sql = "insert into site('name') values ('" + siteName + "')";
                command = new SQLiteCommand(sql, sqliteConnection);
                command.ExecuteNonQuery();
            }

            return siteName;
        }

        private static void UpdateReply(int serverSiteId, DateTime localDate, bool isResult, int rangeDate)
        {
            string sql = "select count(*) from serversiteresult where serversiteid =" + serverSiteId + " and ReplyDate BETWEEN '" + localDate.ToString("MM/dd/yyyy") + "' AND '" + localDate.AddDays(1).ToString("MM/dd/yyyy") + "'";

            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);
            var result = Convert.ToInt32(command.ExecuteScalar());

            if (result < rangeDate)
            {
                sql = "insert into serversiteresult('serversiteId','replyDate','isresult') values (" + serverSiteId + ",'" + localDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "','" + isResult + "')";
                
            }
            else
            {
                sql = "select id from serversiteresult where ServerSiteId = " + serverSiteId + " order by replyDate asc limit 1";
                command = new SQLiteCommand(sql, sqliteConnection);
                var resultID = Convert.ToInt32(command.ExecuteScalar());

                if (resultID != 0)
                {                    
                    sql = "update serversiteresult set replyDate='" + localDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "', isresult='" + isResult + "' where id=" + resultID;
                }
            }

            command = new SQLiteCommand(sql, sqliteConnection);
            command.ExecuteNonQuery();
        }

        private static List<SiteModel> GetAllSite()
        {
            var sites = new List<SiteModel>();

            string sql = "select * from site";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);

            using (var execReader = command.ExecuteReader())
            {
                while (execReader.Read())
                {
                    sites.Add(new SiteModel
                    {
                        SiteId = Convert.ToInt32(execReader["id"]),
                        SiteName = execReader["name"].ToString()
                    });
                }
            }

            return sites;
        }

        private static List<ServerModel> GetAllServer()
        {
            var servers = new List<ServerModel>();

            string sql = "select * from server";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);

            using (var execReader = command.ExecuteReader())
            {
                while (execReader.Read())
                {
                    servers.Add(new ServerModel
                    {
                        ServerIp = execReader["ip"].ToString(),
                        ServerName = execReader["name"].ToString()
                    });
                }
            }

            return servers;
        }

        private static List<ServerSiteModel> GetServerSite()
        {
            var serverSite = new List<ServerSiteModel>();

            string sql = "select ss.ServerId, ss.SiteId, ss.Id, sv.Ip, sv.Name as servername, s.Name as sitename, s.id as siteid from serversite ss inner join server sv on ss.serverid = sv.id inner join site s on ss.siteid = s.id;";
            SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection);

            using (var execReader = command.ExecuteReader())
            {
                while (execReader.Read())
                {
                    serverSite.Add(new ServerSiteModel
                    {
                        Id = Convert.ToInt32(execReader["id"]),
                        ServerIP = execReader["ip"].ToString(),
                        SiteId = Convert.ToInt32(execReader["SiteId"]),
                        SiteName = execReader["sitename"].ToString()
                    });
                }
            }

            return serverSite;
        }

    }
}
