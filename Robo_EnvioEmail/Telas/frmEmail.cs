using Robo_EnvioEmail.Negocio;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Robo_EnvioEmail
{
    public partial class frmEmail : Form
    {
        DateTime dtProximaAtualizacao;
        string sHost = string.Empty;
        int iPorta = 0;
        bool bUtilizaSSL = false;
        string sEmailRemetente = string.Empty;
        string sSenhaEmail = string.Empty;
        string sUserAuth = string.Empty;
        string sPasswordAuth = string.Empty;
        int iEnvio = 1;

        public frmEmail()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            var settings = ConfigurationManager.AppSettings;

            if (settings["Host"] == "")
            {
                MessageBox.Show("Necessário configurar o Host no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            if (settings["Porta"] == "")
            {
                MessageBox.Show("Necessário configurar a Porta no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            if (settings["UtilizaSSL"] == "")
            {
                MessageBox.Show("É necessário configurar a opção Utiliza SSL no arquivo de configurção.", "Configuração");
                return;
            }
                
            if (settings["Email"] == "")
            {
                MessageBox.Show("Necessário configurar o email de envio no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            if (settings["Senha"] == "")
            {
                MessageBox.Show("Necessário configurar a senha do email no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            sHost = ConfigurationManager.AppSettings["Host"].ToString();
            iPorta = Convert.ToInt32(ConfigurationManager.AppSettings["Porta"].ToString());
            bUtilizaSSL = (ConfigurationManager.AppSettings["UtilizaSSL"] == "S" ? true : false);
            sEmailRemetente = ConfigurationManager.AppSettings["Email"].ToString();
            sSenhaEmail = ConfigurationManager.AppSettings["Senha"].ToString();

            if (settings["AuthenticationUser"] != null)
                sUserAuth = ConfigurationManager.AppSettings["AuthenticationUser"].ToString();

            if (settings["AuthenticationPassword"] != null)
                sPasswordAuth = ConfigurationManager.AppSettings["AuthenticationPassword"].ToString();

            btnParar.Enabled = true;
            btnEnviar.Enabled = false;
            btnLimpar.Enabled = false;

            txtStatus.Text += DateTime.Now.ToString("f") + " - Aplicação iniciada." + Environment.NewLine;
            this.Refresh();

            ProximaAtualizacao();
            timer1.Enabled = true;
        }

        private void ProximaAtualizacao()
        {
            DateTime dtPrimeiroEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, dtpPrimeiroEnvio.Value.Hour, dtpPrimeiroEnvio.Value.Minute, 0);
            DateTime dtSegundoEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, dtpSegundoEnvio.Value.Hour, dtpSegundoEnvio.Value.Minute, 0);

            if (dtPrimeiroEnvio > DateTime.Now)
            {
                dtProximaAtualizacao = dtPrimeiroEnvio;
            }
            else if(dtSegundoEnvio > DateTime.Now)
            {
                dtProximaAtualizacao = dtSegundoEnvio;
            }
            else
            {
                dtProximaAtualizacao = dtPrimeiroEnvio.AddDays(1);
            }
            
            lblProximaAtualizacao.Text = dtProximaAtualizacao.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now >= dtProximaAtualizacao)
                {
                    string sStatusEnvio = string.Empty;
                    EnvioEmail objEnvioEmail = new EnvioEmail(sHost, iPorta, bUtilizaSSL, sEmailRemetente, sSenhaEmail, sUserAuth, sPasswordAuth);
                    Relatorio objRelatorio = new Relatorio();

                    txtStatus.Text += DateTime.Now.ToString("f") + " - Verificando emails pendentes de envio." + Environment.NewLine;
                    this.Refresh();

                    timer1.Enabled = false;

                    var objBase = new DataAcess.ADOBase();
                    DataTable dt = new DataTable();
                    DataTable dtRelatorio = new DataTable();
                    StringBuilder sSQL = new StringBuilder();

                    bool bRet = false;
                    string sSQLAtualizar = string.Empty;

                    string sQuery = "Select mov.dt_ImpressaoConhecimento as Emissao, movNF.cd_notaFiscal as NF," +
                            " rTrim(Isnull(mov.nr_Minuta, '')) as Minuta, rTrim(Isnull(mov.nr_Conhecimento, '')) as CTe," +
                            " rTrim(rem.ds_Pessoa) as Remetente, rTrim(cidrem.ds_Cidade) Cidade_Origem, rTrim(estrem.cd_Estado) UF_Origem," +
                            " rTrim(fat.ds_Pessoa) as Faturado, rTrim(mov.ds_Cliente) as Destinatario, rTrim(ciddest.ds_Cidade) Cidade_Destinatario, " +
                            " rTrim(estdest.cd_Estado) UF_Destinatario, rTrim(oco.ds_Ocorrencia) as Ultima_Ocorrencia, " +
                            " ocoNF.dt_PrazoFechamento as Data, ocoNF.hr_PrazoFechamento as Hora," +
                            " movNF.vl_NotaFiscal Valor_NF, movNF.qt_Volume as Volume," +
                            " movNF.kg_Mercadoria as Peso, mov.vl_Frete Valor_Frete" +
                            " From tbdMovimento mov" +
                            " Inner join tbdMovimentoNotaFiscal MovNF on MovNF.id_Movimento = mov.id_Movimento" +
                            " Inner join v_DadosMovimento v on MovNF.id_Movimento = v.id_Movimento And MovNF.cd_NotaFiscal = v.cd_NotaFiscal" +
                            " Inner join tbdOcorrenciaNota ocoNF on v.id_OcorrenciaNota = ocoNF.id_OcorrenciaNota" +
                            " Inner join tbdOcorrencia oco on ocoNF.id_Ocorrencia = oco.id_Ocorrencia" +
                            " Inner join tbdPessoa fat on mov.id_ClienteFaturamento = fat.id_Pessoa" +
                            " Inner join tbdPessoa rem on mov.id_Remetente = rem.id_Pessoa" +
                            " Inner join tbdCidade cidrem on mov.id_CidadeOrigem = cidrem.id_Cidade" +
                            " Inner join tbdEstado estrem on cidrem.id_Estado = estrem.id_Estado" +
                            " Inner join tbdCidade ciddest on mov.id_Cidade = ciddest.id_Cidade" +
                            " Inner join tbdEstado estdest on ciddest.id_Estado = estdest.id_Estado" +
                            " Left  join tbdParametrizacaoPrazoOcorrencia param on ocoNF.id_Ocorrencia = param.id_Ocorrencia" +
                            " Where id_ClienteFaturamento = {0} And mov.dt_ImpressaoConhecimento = '{1}'" +
                            " And Isnull(param.tp_NaoEnviarRelatorio, '') <> 'S'" +
                            " Order by MovNF.id_Movimento, MovNF.cd_NotaFiscal";


                    sSQL.Append("Select").Append(Environment.NewLine);
                    sSQL.Append("Distinct a.id_Cliente, b.ds_Pessoa, a.ds_EmailDestino, a.ds_AssuntoEmail, a.ds_CorpoEmail").Append(Environment.NewLine);
                    sSQL.Append("From tbdExtraClienteEmail a").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPessoa b on a.id_Cliente = b.id_Pessoa").Append(Environment.NewLine);

                    dt = objBase.RealizaPesquisaSQL(sSQL.ToString());

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        txtStatus.Text += DateTime.Now.ToString("f") + " - Nenhum cliente cadastrado para envio de relatório." + Environment.NewLine;
                        this.Refresh();
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dtRelatorio = objBase.RealizaPesquisaSQL(String.Format(sQuery, dr["id_Cliente"].ToString(), DateTime.Now.ToString("yyyy-MM-dd")));

                            if (dtRelatorio == null || dtRelatorio.Rows.Count == 0)
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - Não há emissões hoje para o cliente." + dr["ds_Pessoa"] + Environment.NewLine;
                                continue;
                            }

                            string sArquivoGerado = objRelatorio.GerarExcel(dtRelatorio, AppDomain.CurrentDomain.BaseDirectory + "Relatorios\\");

                            sStatusEnvio = objEnvioEmail.EnviarMensagemEmail(
                                dr["ds_EmailDestino"].ToString(),
                                "",
                                dr["ds_AssuntoEmail"].ToString(),
                                dr["ds_CorpoEmail"].ToString(),
                                sArquivoGerado
                                );

                            if (sStatusEnvio.ToUpper() == "OK")
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - Email enviado para: " + dr["ds_EmailDestino"] + Environment.NewLine;
                                this.Refresh();
                            }
                            else
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - Ocorreu uma falha no envio do email: " + sStatusEnvio + Environment.NewLine;
                                this.Refresh();
                            }
                        }

                        txtStatus.Text += DateTime.Now.ToString("f") + " - Processo concluído." + Environment.NewLine;
                        this.Refresh();
                    }

                    ProximaAtualizacao();
                    timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu erro durante o processo de envio: " + Environment.NewLine + ex.Message.ToString());
            }
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            btnParar.Enabled = false;

            txtStatus.Text += DateTime.Now.ToString("f") + " - Aplicação paralisada pelo usuário." + Environment.NewLine;
            this.Refresh();

            btnEnviar.Enabled = true;
            btnLimpar.Enabled = true;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "";
            this.Refresh();
        }
    }
}
