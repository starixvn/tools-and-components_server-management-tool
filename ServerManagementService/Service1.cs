using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

namespace ServerManagementService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.CreateXML);
            timer.Start();
        }

        protected override void OnStop()
        {

        }

        protected void CreateXML(object sender, System.Timers.ElapsedEventArgs args)
        {
            var timeZoneId = "SE Asia Standard Time";
            XmlDocument doc = new XmlDocument();
            var nowDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), timeZoneId);
            var path = ConfigurationManager.AppSettings["DataPath"].ToString() + nowDate.ToString("MM-dd-yyyy") + ".xml";

            if (!File.Exists(path))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(path))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("servers");

                    //foreach (var site in jsonResult)
                    //{
                    //    // Console.WriteLine(site.System);

                    //    xmlWriter.WriteStartElement("system");
                    //    xmlWriter.WriteAttributeString("name", site.System);

                    //    foreach (var server in site.Server)
                    //    {
                    //        var isChecked = PingHost(server);

                    //        xmlWriter.WriteStartElement("server");
                    //        xmlWriter.WriteAttributeString("ip", server);

                    //        xmlWriter.WriteStartElement("reply");
                    //        xmlWriter.WriteAttributeString("time", nowDate.ToShortTimeString());
                    //        xmlWriter.WriteString(isChecked ? "ok" : "fail");
                    //        xmlWriter.WriteEndElement();

                    //        xmlWriter.WriteEndElement();
                    //    }

                    //    xmlWriter.WriteEndElement();
                    //}

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                }

            }
            //else
            //{
            //    doc.Load(path);

            //    XmlNode serversNode = doc.SelectNodes("servers").Item(0);
            //    XmlNodeList systemNodeList = serversNode.SelectNodes("system");

            //    foreach (XmlNode systemNode in systemNodeList)
            //    {
            //        XmlNodeList serverNodeList = systemNode.SelectNodes("server");

            //        foreach (XmlNode serverNode in serverNodeList)
            //        {
            //            var ipValue = serverNode.Attributes["ip"].Value;
            //            var isChecked = PingHost(ipValue);
            //            XmlNode replyNode = doc.CreateElement("reply");
            //            XmlAttribute timeAttribute = doc.CreateAttribute("time");
            //            timeAttribute.Value = nowDate.ToShortTimeString();
            //            replyNode.Attributes.Append(timeAttribute);
            //            replyNode.InnerText = isChecked ? "ok" : "fail";
            //            serverNode.AppendChild(replyNode);
            //        }
            //    }

            //    doc.Save(path);
            //}
        }

    }
}
