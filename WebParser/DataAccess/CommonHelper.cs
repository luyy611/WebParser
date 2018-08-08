using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.DataAccess
{
    /*
     * 公共数据库操作类
     */
    public class CommonHelper
    {
        /// <summary>
        /// 查询url是否已存在
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool QueryByUrl(string url, string tableName)
        {
            try
            {
                string sql = string.Format("select count(*) from {0} where url=@url", tableName);
                using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
                {
                    cmd.Parameters.Add(DBAccessManager.SqlDBAccess.CreateParameter("@url", url, ParameterDirection.Input));
                    int count = (int)DBAccessManager.SqlDBAccess.ExecuteScalar(cmd);
                    //如果记录已存在则return true
                    if (count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (DbException ex)
            {
                ExceptionManager.Publish(ex);
            }
            return false;
        }
        /// <summary>
        /// 将抓取到的数据存入数据库相应的表中
        /// 第一种数据库表模板（气象，水位）
        /// </summary>
        public static bool SaveToOneTable(string title, string articlefrom, string updatetime, string url, string content, string tableName)
        {
            DataTable dt = new DataTable();
            //设置与数据库中一致的列
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("article", typeof(string));
            dt.Columns.Add("updatetime", typeof(string));
            dt.Columns.Add("url", typeof(string));
            dt.Columns.Add("content", typeof(string));
            dt.Rows.Add(DBNull.Value, title, articlefrom, updatetime, url, content);//将获取的值传入dt
            using (var transResult = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    using (SqlBulkCopy opBulk = new SqlBulkCopy((SqlConnection)transResult.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transResult.Transaction))
                    {
                        opBulk.DestinationTableName = tableName;//数据库中的表名

                        opBulk.WriteToServer(dt);//由dt写入数据库
                    }
                    transResult.CommitTransaction();

                    return true;
                }
                catch (DbException ex)
                {
                    transResult.RollbackTransaction();
                    ExceptionManager.Publish(ex);
                    return false;
                }

            }
            
        }
        /// <summary>
        /// 第二种数据库表模板（航道信息，航行通告）
        /// </summary>  
        public static bool SaveToTwoTable(string title, string articlefrom, string updatetime, string url, string content, string tableName)
        {
            DataTable dt = new DataTable();
            //设置与数据库中一致的列
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("article", typeof(string));
            dt.Columns.Add("updatetime", typeof(string));
            dt.Columns.Add("url", typeof(string));
            dt.Columns.Add("content", typeof(string));
            dt.Rows.Add(DBNull.Value, title, articlefrom, updatetime, url, content);//将获取的值传入dt
            using (var transResult = DBAccessManager.SqlDBAccess.BeginTransaction())
            {
                try
                {
                    using (SqlBulkCopy opBulk = new SqlBulkCopy((SqlConnection)transResult.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transResult.Transaction))
                    {
                        opBulk.DestinationTableName = tableName;//数据库中的表名
                        opBulk.WriteToServer(dt);//由dt写入数据库
                    }
                    transResult.CommitTransaction();
                    return true;
                }
                catch (DbException ex)
                {
                    transResult.RollbackTransaction();
                    ExceptionManager.Publish(ex);
                    return false;
                }

            }
           
        }

    }
}
