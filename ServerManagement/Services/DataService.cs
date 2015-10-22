using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace ServerManagement.Services
{
    public class BaseService
    {
        private SQLiteConnection _sqLiteConnection = new SQLiteConnection();

        public BaseService()
        {
            var dataPath = ConfigurationManager.AppSettings["DatabasePath"];
            _sqLiteConnection = ConnectionString(dataPath);
        }

        public SQLiteDataReader QueryData(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, _sqLiteConnection);

            return command.ExecuteReader();
        }

        public object QueryDataScalar(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, _sqLiteConnection);

            return command.ExecuteScalar();
        }

        public DataSet QueryDataSet(string sql)
        {
            var dataSet = new DataSet();
            var dataAdapter = new SQLiteDataAdapter(sql, _sqLiteConnection);
            dataAdapter.Fill(dataSet);

            return dataSet;
        }

        public int UpdateData(string sql)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, _sqLiteConnection);

                return command.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
        }

        private SQLiteConnection ConnectionString(string dataSourcePath)
        {
            var sqliteConn = new SQLiteConnection("Data Source=" + dataSourcePath + ";Version=3;PRAGMA journal_mode=WAL;");

            if (sqliteConn.State == ConnectionState.Closed)
            {
                sqliteConn.Open();
            }

            return sqliteConn;
        }
    }
}