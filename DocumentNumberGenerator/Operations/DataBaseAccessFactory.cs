using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentNumberGenerator.Operations
{
    /// <summary>
    /// Factory pattern
    /// </summary>
    public static class DataBaseAccessFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>DataBase Operation Class Instance</returns>
        public static IDatabaseOperation GetDataBaseOperationClass()
        {
            return new DatabaseOperation();
        }

    }
}
