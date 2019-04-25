using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace QBA.Qutilize.WebApp.DAL
{
    public class SqlHelper : IDisposable
    {
        public SqlHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string connection()
        {
            ConnectionStringSettingsCollection connectionStrings = WebConfigurationManager.ConnectionStrings as ConnectionStringSettingsCollection;
            return Convert.ToString(connectionStrings["QBADBConnetion"]);
        }

        public DataSet ExecuteDataset(String storedprocedure, params SqlParameter[] arrParam)
        {
            SqlConnection cn = new SqlConnection(connection());
            try
            {
                //initialisation of datatable, Sql connection and sql command
                DataSet dt = new DataSet();

                SqlCommand cmd = new SqlCommand(storedprocedure, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                //opens the db connection
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();

                //if sql param is not null then gets the parameters from that
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                        cmd.Parameters.Add(param);
                }
                cmd.CommandTimeout = 5000;
                //executes the command and fills it into the datatable
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            catch (Exception ee)
            {
                //Errormessage.WriteError(ee.ToString());
                //System.Windows.Forms.MessageBox.Show(ee.Message);
                return null;
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable ExecuteDataTable(String storedprocedure, params SqlParameter[] arrParam)
        {
            SqlConnection cn = new SqlConnection(connection());
            try
            {
                //initialisation of datatable, Sql connection and sql command
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand(storedprocedure, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                //opens the db connection
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();

                //if sql param is not null then gets the parameters from that
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                        cmd.Parameters.Add(param);
                }

                //executes the command and fills it into the datatable
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ee.Message);
                //Errormessage.WriteError(ee.Message);
                throw;
                // return null;

            }
            finally
            {
                cn.Close();
            }
        }

        public DataSet ExecuteDataset(String storedprocedure)
        {
            SqlConnection cn = new SqlConnection(connection());
            try
            {
                //initialisation of datatable, Sql connection and sql command
                DataSet dt = new DataSet();

                SqlCommand cmd = new SqlCommand(storedprocedure, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                //opens the db connection
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();
                cmd.CommandTimeout = 180;
                //executes the command and fills it into the datatable
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ee)
            {
                //Errormessage.WriteError(ee.ToString());
                //System.Windows.Forms.MessageBox.Show(ee.Message);
                return null;
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable ExecuteDataTable(String storedprocedure)
        {
            SqlConnection cn = new SqlConnection(connection());
            try
            {
                //initialisation of datatable, Sql connection and sql command
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand(storedprocedure, cn);
                cmd.CommandType = CommandType.Text;

                //opens the db connection
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();

                //executes the command and fills it into the datatable
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ee.Message);
                //Errormessage.WriteError(ee.Message);
                return null;
            }
            finally
            {
                cn.Close();
            }
        }

        public int CreateNewMasters(String storedprocedure, params SqlParameter[] arrParam)
        {
            int modified = 1;
            SqlConnection cn = new SqlConnection(connection());
            try
            {
                //initialisation of datatable, Sql connection and sql command
                SqlCommand cmd = new SqlCommand(storedprocedure, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                //opens the db connection
                if (cn.State == ConnectionState.Closed || cn.State == ConnectionState.Broken)
                    cn.Open();

                //if sql param is not null then gets the parameters from that
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                        cmd.Parameters.Add(param);
                }
                //executes the command and fills it into the datatable
                modified = cmd.ExecuteNonQuery();
                return modified;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ee.Message);
                //Errormessage.WriteError(ee.Message);
                return -1;
            }
            finally
            {
                if (cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
        }

        public Boolean SaveFile(SqlCommand cmd)
        {
            SqlConnection cn = new SqlConnection(connection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                return false;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        //Testing 
        public DataTable GetData(string query)
        {
            SqlConnection cn = new SqlConnection(connection());
            SqlCommand cmd = new SqlCommand(query, cn);
            DataTable table = new DataTable();
            try
            {
                cn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                // Fill the DataTable.
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                return table;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}