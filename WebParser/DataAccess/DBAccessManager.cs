using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.DataAccess
{
    /*
     * 线程安全的数据库操作对象
     */
    public class DBAccessManager
    {
        private static Lazy<DBAccess> db = new Lazy<DBAccess>(() => DBAccess.CreateDB("SQL"), true);
        public static DBAccess SqlDBAccess
        {
            get{return db.Value; }
        }
    }
}
