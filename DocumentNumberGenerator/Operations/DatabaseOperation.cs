using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace DocumentNumberGenerator.Operations
{
    /// <summary>
    /// Local SqLite database operation
    /// </summary>
    public class DatabaseOperation : IDatabaseOperation
    {
        /// <summary>
        /// Function returns opened DataBase Connection
        /// </summary>
        readonly Func<SQLiteConnection> getConnection =
            () =>
            {
                try
                {
                    var connection =
                        new SQLiteConnection(@"data source=SqliteDatabase.db");
                    connection.Open();
                    return connection;
                }
                catch (SQLiteException ex)
                {
                    throw new Exception($@"Some SQLiteException error: \n {ex.Message}%");

                }
                catch (SqlException ex)
                {
                    throw new Exception($@"Some SqlException error: \n {ex.Message}%");

                }
                catch (Exception ex)
                {
                    throw new Exception($@"Some Exception error: \n {ex.Message}%");

                }
            };
        /// <summary>
        /// Constructor, create if not exist local database
        /// </summary>
        public DatabaseOperation()
        {
            try
            {
                using (var connection = getConnection())
                {

                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = @"CREATE TABLE  IF NOT EXISTS [TestTable] (
                                                     [Id] INTEGER NOT NULL
                                                    , CONSTRAINT [sqlite_master_PK_TestTable] PRIMARY KEY ([Id]))";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception($@"Some SQLiteException error: \n {ex.Message}%");

            }
            catch (SqlException ex)
            {
                throw new Exception($@"Some SqlException error: \n {ex.Message}%");

            }
            catch (Exception ex)
            {
                throw new Exception($@"Some Exception error: \n {ex.Message}%");

            }
        }

        /// <inheritdoc />
        public int GetDataBaseCount()
        {
            try
            {
                using (var connection = getConnection())
                {
                    using (SQLiteCommand createTableCommand = new SQLiteCommand(connection))
                    {
                        createTableCommand.CommandText = @"SELECT COUNT(*) FROM TestTable";
                        var countAfterInsert = createTableCommand.ExecuteScalar();
                        return Convert.ToInt32(countAfterInsert);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception($@"Some SQLiteException error: \n {ex.Message}%");

            }
            catch (SqlException ex)
            {
                throw new Exception($@"Some SqlException error: \n {ex.Message}%");

            }
            catch (Exception ex)
            {
                throw new Exception($@"Some Exception error: \n {ex.Message}%");

            }
        }

        /// <inheritdoc />
        public int GetDataBaseCount(int fromValue, int toValue)
        {
            try
            {
                using (var connection = getConnection())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = @"SELECT COUNT(*) FROM TestTable where Id between @FromValue and @ToValue";
                        cmd.Parameters?.Add(new SQLiteParameter("@FromValue", fromValue));
                        cmd.Parameters?.Add(new SQLiteParameter("@ToValue", toValue));
                        var countAfterInsert = cmd.ExecuteScalar();
                        return Convert.ToInt32(countAfterInsert);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception($@"Some SQLiteException error: \n {ex.Message}%");

            }
            catch (SqlException ex)
            {
                throw new Exception($@"Some SqlException error: \n {ex.Message}%");

            }
            catch (Exception ex)
            {
                throw new Exception($@"Some Exception error: \n {ex.Message}%");

            }

        }


        /// <inheritdoc />
        public bool GetDataBaseExistElement(int id)
        {
            try
            {
                using (var connection = getConnection())
                {
                    using (var cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = @"SELECT * FROM TestTable where Id=@IdValue LIMIT 1";
                        cmd.Parameters.Add(new SQLiteParameter("@IdValue", id));
                        return cmd.ExecuteReader().HasRows;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception($@"Some SQLiteException error: \n {ex.Message}%");

            }
            catch (SqlException ex)
            {
                throw new Exception($@"Some SqlException error: \n {ex.Message}%");

            }
            catch (Exception ex)
            {
                throw new Exception($@"Some Exception error: \n {ex.Message}%");

            }
        }


        /// <inheritdoc />
        public virtual int FillDataTable(List<int> numbers)
        {
            try
            {
                using (var connection = getConnection())
                {
                    using (var cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = @"INSERT OR IGNORE INTO TestTable(Id) VALUES " + "(" +
                                          string.Join("),(", numbers.ToArray()) + ")";
                        return cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new Exception($@"Some SQLiteException error: \n {ex.Message}%");
               
            }
            catch (SqlException ex)
            {
                throw new Exception($@"Some SqlException error: \n {ex.Message}%");

            }
            catch (Exception ex)
            {
                throw new Exception($@"Some Exception error: \n {ex.Message}%");

            }

        }

    }
}