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
