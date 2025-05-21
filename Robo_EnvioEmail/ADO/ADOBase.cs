using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Robo_EnvioEmail.DataAcess
{
    public class ADOBase
    {
        public DataTable RealizaPesquisaSQL(string strSQL)
        {
            try
            {
                using (var objConexao = new SqlConnection(RetornaConexao()))
                using (var objComando = new SqlCommand(strSQL, objConexao))
                {
                    objComando.CommandTimeout = 99999;
                    var objAdaptador = new SqlDataAdapter(objComando);
                    var objTabela = new DataTable();
                    objAdaptador.Fill(objTabela);
                    return objTabela;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExecutaComando(string strSQL)
        {
            using (var objConexao = new SqlConnection(RetornaConexao()))
            using (var objComando = new SqlCommand(strSQL, objConexao))
            {
                objConexao.Open();
                objComando.CommandTimeout = 99999;
                objComando.CommandType = CommandType.Text;
                
                return objComando.ExecuteNonQuery() > 0;
            }
        }

        private string RetornaConexao()
        {

            string sConexao = ConfigurationManager.ConnectionStrings["ConexaoDB"].ConnectionString;

            return sConexao;

        }


    }
}
