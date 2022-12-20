using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robo_FTP.DataAcess
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
                throw;
            }
        }

        private string RetornaConexao()
        {

            string sConexao = ConfigurationManager.ConnectionStrings["ConexaoDB"].ConnectionString;

            return sConexao;

        }


    }
}
