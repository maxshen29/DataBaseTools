using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataBaseTools.Common;
using DataBaseTools.Model;
namespace DataBaseTools.SQLMethod
{
    public  class SQLContext:DbContext
    {
       
        private string  _sqlconnectstr;
 
        public SQLContext()
        {
            _sqlconnectstr = CommHelper.GetSQLConnectStr();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlServer(_sqlconnectstr);
    }
}
