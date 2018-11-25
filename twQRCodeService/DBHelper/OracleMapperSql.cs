using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using Dapper;
using System.Windows;

namespace twQRCodeService.DBHelper
{
    public static class OracleMapperSql
    {
        public static OracleConnection GetOpenConnection()
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(OracleBaseRepository.DbUrl);
            }
            catch (OracleException e)
            {
                throw new Exception(e.Message);
            }
            return connection;
        }


        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet getDsQuery(string SQLString)
        {
            DataSet ds = new DataSet();
            using (OracleConnection connection = GetOpenConnection())
            {
                try
                {
                    OracleCommand command = new OracleCommand(SQLString, connection);
                    connection.Open();
                    OracleDataReader dbReader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    ds.Tables.Add(new DataTable());
                    ds.Tables[0].Load(dbReader);
                }
                catch (OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return ds;
        }


        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new()
        {
            int records = 0;
            using (OracleConnection cnn = GetOpenConnection())
            {
                cnn.Open();
                using (var trans = cnn.BeginTransaction())
                {
                    try
                    {
                        records = cnn.Execute(sql, entities, trans, 30, CommandType.Text);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.StackTrace.ToString());
                    }
                    finally
                    {
                        cnn.Close();
                    }
                }
            }
            return records;
        }



        /// <summary>
        /// Delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int DeleteSql(string sql)
        {
            int result = 0;
            using (OracleConnection connection = GetOpenConnection())
            {
                try
                {
                    connection.Open();
                    OracleCommand cmd = new OracleCommand(sql, connection);
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.StackTrace.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionName = null)
        {
            int i = 0;
            using (OracleConnection connection = GetOpenConnection())
            {
                try
                {
                    connection.Open();
                    i = connection.Execute(sql, (object)parms);
                }
                catch (Exception e)
                {
                    throw new Exception(e.StackTrace.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            return i;
        }


        /// <summary>
        /// SQL with params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> SqlWithParams<T>(string sql, dynamic parms, string connectionnName = null)
        {
            List<T> Tlist = new List<T>();
            using (OracleConnection connection = GetOpenConnection())
            {
                try
                {
                    connection.Open();
                    Tlist = connection.Query<T>(sql, (object)parms).ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }

            return Tlist;
        }

    }
}
