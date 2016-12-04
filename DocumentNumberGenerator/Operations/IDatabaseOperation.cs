using System.Collections.Generic;
using System.Data.SQLite;

namespace DocumentNumberGenerator.Operations
{
    /// <summary>
    /// DataBase Operation Interfaces. 
    /// </summary>
    public interface IDatabaseOperation
    {
        /// <summary>
        /// Count operation
        /// </summary>
        /// <returns>Size/number of elements in DataBase Table</returns>
        int GetDataBaseCount();

        /// <summary>
        /// Count operation by range
        /// </summary>
        /// <param name="fromValue">Start range of document number</param>
        /// <param name="toValue">End range of document number</param>
        /// <returns></returns>
        int GetDataBaseCount(int fromValue, int toValue);

        /// <summary>
        /// Operation try to fill 
        /// </summary>
        /// <param name="numbers"></param>
        int FillDataTable(List<int> numbers);
        bool GetDataBaseExistElement(int id);

    }
}