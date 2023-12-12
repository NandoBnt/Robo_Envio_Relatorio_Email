using Robo_EnvioEmail.Negocio;
using System;
using System.Configuration;
using System.Data;
using System.IO;
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
        string sEmailRemetenteEDI = string.Empty;
        string sSenhaEmailEDI = string.Empty;
        string sUserAuth = string.Empty;
        string sPasswordAuth = string.Empty;
        string sDiretorioArquivo = string.Empty;
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

            if (settings["Email_EDI"] == "")
            {
                MessageBox.Show("Necessário configurar o email de envio de EDI no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settings["Senha_EDI"] == "")
            {
                MessageBox.Show("Necessário configurar a senha do email de EDI no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settings["DiretorioArquivoEDI"] == "")
            {
                MessageBox.Show("Necessário configurar o diretório dos arquivos de EDI no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sHost = ConfigurationManager.AppSettings["Host"].ToString();
            iPorta = Convert.ToInt32(ConfigurationManager.AppSettings["Porta"].ToString());
            bUtilizaSSL = (ConfigurationManager.AppSettings["UtilizaSSL"] == "S" ? true : false);
            sEmailRemetente = ConfigurationManager.AppSettings["Email"].ToString();
            sSenhaEmail = ConfigurationManager.AppSettings["Senha"].ToString();
            sEmailRemetenteEDI = ConfigurationManager.AppSettings["Email_EDI"].ToString();
            sSenhaEmailEDI = ConfigurationManager.AppSettings["Senha_EDI"].ToString();
            sDiretorioArquivo = ConfigurationManager.AppSettings["DiretorioArquivoEDI"].ToString();

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
            else if (dtSegundoEnvio > DateTime.Now)
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

                    #region Envio de Relatorio Excel

                    Relatorio objRelatorio = new Relatorio();

                    txtStatus.Text += DateTime.Now.ToString("f") + " - Verificando emails pendentes de envio." + Environment.NewLine;
                    this.Refresh();

                    timer1.Enabled = false;

                    var objBase = new DataAcess.ADOBase();
                    DataTable dt = new DataTable();
                    DataTable dtRelatorio = new DataTable();
                    StringBuilder sSQL = new StringBuilder();

                    string sSQLAtualizar = string.Empty;
                    string sQuery = string.Empty;

                    sQuery = "Select mov.dt_ImpressaoConhecimento as Emissao, movNF.cd_notaFiscal as NF," +
                            " rTrim(Isnull(mov.nr_Minuta, '')) as Minuta, rTrim(Isnull(mov.nr_Conhecimento, '')) as CTe," +
                            " rTrim(rem.ds_Pessoa) as Remetente, rTrim(cidrem.ds_Cidade) Cidade_Origem, rTrim(estrem.cd_Estado) UF_Origem," +
                            " rTrim(fat.ds_Pessoa) as Faturado, rTrim(mov.ds_Cliente) as Destinatario, rTrim(ciddest.ds_Cidade) Cidade_Destinatario, " +
                            " rTrim(estdest.cd_Estado) UF_Destinatario, rTrim(oco.ds_Ocorrencia) as Ultima_Ocorrencia, " +
                            " ocoNF.dt_PrazoFechamento as Data, ocoNF.hr_PrazoFechamento as Hora, rTrim(ocoNF.ds_Ocorrencia) as Complemento, " +
                            " movNF.vl_NotaFiscal Valor_NF, movNF.qt_Volume as Volume," +
                            " movNF.kg_Mercadoria as Peso, mov.vl_Frete Valor_Frete" +
                            " From tbdMovimento mov (Nolock)" +
                            " Inner join tbdExtraGrupoTipoMovimentoItem grupoTipo (Nolock) on mov.id_TipoMovimento = grupoTipo.id_TipoMovimento And grupoTipo.id_GrupoTipoMovimento = 2" +
                            " Inner join tbdMovimentoNotaFiscal MovNF (Nolock) on MovNF.id_Movimento = mov.id_Movimento" +
                            " Inner join v_DadosMovimento v (Nolock) on MovNF.id_Movimento = v.id_Movimento And MovNF.cd_NotaFiscal = v.cd_NotaFiscal" +
                            " Inner join tbdOcorrenciaNota ocoNF (Nolock) on v.id_OcorrenciaNota = ocoNF.id_OcorrenciaNota" +
                            " Inner join tbdOcorrencia oco (Nolock) on ocoNF.id_Ocorrencia = oco.id_Ocorrencia" +
                            " Inner join tbdPessoa fat (Nolock) on mov.id_ClienteFaturamento = fat.id_Pessoa" +
                            " Inner join tbdPessoa rem (Nolock) on mov.id_Remetente = rem.id_Pessoa" +
                            " Inner join tbdCidade cidrem (Nolock) on mov.id_CidadeOrigem = cidrem.id_Cidade" +
                            " Inner join tbdEstado estrem (Nolock) on cidrem.id_Estado = estrem.id_Estado" +
                            " Inner join tbdCidade ciddest (Nolock) on mov.id_Cidade = ciddest.id_Cidade" +
                            " Inner join tbdEstado estdest (Nolock) on ciddest.id_Estado = estdest.id_Estado" +
                            " Left  join tbdParametrizacaoPrazoOcorrencia param (Nolock) on ocoNF.id_Ocorrencia = param.id_Ocorrencia" +
                            " Where id_ClienteFaturamento = {0} And ocoNF.dt_PrazoFechamento >= '{1}'" +
                            " And Isnull(param.tp_NaoEnviarRelatorio, '') <> 'S'" +
                            " And Not Exists(" +
                            "   Select 1 from tbdOcorrenciaNota x " +
                            "   Inner join tbdOcorrenciaManifesto y on x.id_Ocorrencia = y.id_OcorrenciaManifesto" +
                            "   Where x.id_Movimento = MovNF.id_Movimento " +
                            "       And x.nr_NotaFiscal = MovNF.cd_NotaFiscal " +
                            "       And y.tp_BaixaAutomatica = 'S'" +
                            "   )" +
                            " Order by MovNF.id_Movimento, MovNF.cd_NotaFiscal";


                    sSQL.Append("Select").Append(Environment.NewLine);
                    sSQL.Append("Distinct a.id_Cliente, b.ds_Pessoa, a.ds_EmailDestino, a.ds_AssuntoEmail, a.ds_CorpoEmail").Append(Environment.NewLine);
                    sSQL.Append("From tbdExtraClienteEmail a (Nolock) ").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPessoa b (Nolock) on a.id_Cliente = b.id_Pessoa").Append(Environment.NewLine);

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
                            dtRelatorio = objBase.RealizaPesquisaSQL(String.Format(sQuery, dr["id_Cliente"].ToString(), dtpDataCorte.Value.ToString("yyyy-MM-dd")));

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

                    #endregion

                    #region Envio de Arquivos EDI

                    sQuery =
                        "Select config.ds_Email, config.ds_AssuntoEmail, config.ds_TextoEmail, b.ds_Pessoa, e.ds_Layout" + Environment.NewLine +
                        "From tbdConfiguracaoFTPCliente config " + Environment.NewLine +
                        "Inner join tbdConfiguracaoEDICliente a on config.id_Cliente = a.id_Cliente" + Environment.NewLine +
                        "Inner join tbdPessoa b on a.id_Cliente = b.id_Pessoa" + Environment.NewLine +
                        "Inner join tbdConfiguracaoEDI d on a.id_EDI = d.id_EDI" + Environment.NewLine +
                        "Inner join tbdLayoutEDI e on d.id_LayoutEDI = e.id_Layout" + Environment.NewLine +
                        "Where config.tp_Envio = 'E' and config.tp_Robo = 'E'";

                    dt = objBase.RealizaPesquisaSQL(sQuery);

                    if (dt == null || dt.Rows.Count != 0)
                    {
                        objEnvioEmail = null;

                        objEnvioEmail = new EnvioEmail(sHost, iPorta, bUtilizaSSL, sEmailRemetenteEDI, sSenhaEmailEDI, sUserAuth, sPasswordAuth);

                        foreach (DataRow dr in dt.Rows)
                        {
                            var diretorioCliente = sDiretorioArquivo +
                                (!sDiretorioArquivo.EndsWith("\\") ? "\\" : "") + dr["ds_Pessoa"].ToString().Trim() + "\\" + dr["ds_Layout"].ToString().Trim();

                            DirectoryInfo d = new DirectoryInfo(diretorioCliente);

                            d.CreateSubdirectory("Transferidos");

                            string sArquivos = string.Empty;

                            foreach (FileInfo fi in d.GetFiles())
                            {
                                sArquivos += fi.FullName + ";";
                            }

                            if(sArquivos.Length > 0)
                            {
                                sStatusEnvio = objEnvioEmail.EnviarMensagemEmail(dr["ds_Email"].ToString(),
                                                                    "", dr["ds_AssuntoEmail"].ToString(), dr["ds_TextoEmail"].ToString(), sArquivos.Substring(0, sArquivos.Length -1));

                                if (sStatusEnvio.ToUpper() == "OK")
                                {
                                    foreach (FileInfo fi in d.GetFiles())
                                    {
                                        txtStatus.Text += DateTime.Now.ToString("f") + " - Email enviado o cliente: " +
                                        dr["ds_Pessoa"].ToString().Trim() + " - Arquivo: " + fi.Name + Environment.NewLine;

                                        fi.MoveTo(diretorioCliente + "\\Transferidos\\" + fi.Name);

                                        this.Refresh();
                                    }
                                }
                                else
                                {
                                    txtStatus.Text += DateTime.Now.ToString("f") + " - Ocorreu uma falha no envio do email para o cliente[ " +
                                        dr["ds_Pessoa"] + "]: " + sStatusEnvio + Environment.NewLine;
                                    this.Refresh();
                                }
                            }
                            
                            txtStatus.Text += DateTime.Now.ToString("f") + " - Processo concluído." + Environment.NewLine;
                            this.Refresh();
                        }
                    }

                    #endregion

                    ProximaAtualizacao();
                    timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text += DateTime.Now.ToString("f") + " - Ocorreu erro durante o processo de envio: : " + Environment.NewLine + ex.Message.ToString() + Environment.NewLine;
                this.Refresh();

                ProximaAtualizacao();
                timer1.Enabled = true;
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
