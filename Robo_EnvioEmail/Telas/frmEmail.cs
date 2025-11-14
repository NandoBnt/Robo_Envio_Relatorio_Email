using Robo_EnvioEmail.DataAcess;
using Robo_EnvioEmail.Negocio;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace Robo_EnvioEmail
{
    public partial class frmEmail : Form
    {
        DateTime dtProximaAtualizacao;
        DateTime dtProximaAtualizacaoArquivos;
        string sHost = string.Empty;
        int iPorta = 0;
        bool bUtilizaSSL = false;
        string sEmailRemetente = string.Empty;
        string sSenhaEmail = string.Empty;
        string sEmailRemetenteEDI = string.Empty;
        string sSenhaEmailEDI = string.Empty;
        string sEmailRemetentePreAlert = string.Empty;
        string sSenhaEmailPreAlert = string.Empty;
        string sUserAuth = string.Empty;
        string sPasswordAuth = string.Empty;
        string sDiretorioArquivo = string.Empty;
        string sDiretorioXML = string.Empty;
        bool bTrace = false;

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

            if (settings["Email_Agente"] == "")
            {
                MessageBox.Show("Necessário configurar o email de envio de PréAlert no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settings["Senha_Agente"] == "")
            {
                MessageBox.Show("Necessário configurar a senha do email de PréAlert no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settings["DiretorioArquivoEDI"] == "")
            {
                MessageBox.Show("Necessário configurar o diretório dos arquivos de EDI no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (settings["DiretorioXMLCTe"] == "")
            {
                MessageBox.Show("Necessário configurar o diretório dos arquivos XML no arquivo de configurações.", "Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            sHost = ConfigurationManager.AppSettings["Host"].ToString();
            iPorta = Convert.ToInt32(ConfigurationManager.AppSettings["Porta"].ToString());
            bUtilizaSSL = (ConfigurationManager.AppSettings["UtilizaSSL"] == "S" ? true : false);
            sEmailRemetente = ConfigurationManager.AppSettings["Email"].ToString();
            sSenhaEmail = ConfigurationManager.AppSettings["Senha"].ToString();
            sEmailRemetenteEDI = ConfigurationManager.AppSettings["Email_EDI"].ToString();
            sSenhaEmailEDI = ConfigurationManager.AppSettings["Senha_EDI"].ToString();
            sEmailRemetentePreAlert = ConfigurationManager.AppSettings["Email_Agente"].ToString();
            sSenhaEmailPreAlert = ConfigurationManager.AppSettings["Senha_Agente"].ToString();
            sDiretorioArquivo = ConfigurationManager.AppSettings["DiretorioArquivoEDI"].ToString();
            sDiretorioXML = ConfigurationManager.AppSettings["DiretorioXMLCTe"].ToString();

            if (settings["AuthenticationUser"] != null)
                sUserAuth = ConfigurationManager.AppSettings["AuthenticationUser"].ToString();

            if (settings["AuthenticationPassword"] != null)
                sPasswordAuth = ConfigurationManager.AppSettings["AuthenticationPassword"].ToString();

            if(settings["HabilitaTrace"] != null)
                bTrace = ConfigurationManager.AppSettings["HabilitaTrace"].ToString().Equals("S");

            btnParar.Enabled = true;
            btnEnviar.Enabled = false;
            btnLimpar.Enabled = false;

            txtStatus.Text += DateTime.Now.ToString("f") + " - Aplicação iniciada." + Environment.NewLine;
            this.Refresh();

            ProximaAtualizacao(1);
            ProximaAtualizacao(2);
            timer1.Enabled = true;
        }

        private void ProximaAtualizacao(int iAtualizacao)
        {
            if (iAtualizacao == 1)
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
            else
            {
                dtProximaAtualizacaoArquivos = DateTime.Now.AddMinutes(Convert.ToInt16(txtIntervalo.Text));

                lblProximaAtualizacaoArquivos.Text = dtProximaAtualizacaoArquivos.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now >= dtProximaAtualizacao)
                {
                    string sStatusEnvio = string.Empty;
                    EnvioEmail objEnvioEmail = new EnvioEmail(sHost, iPorta, bUtilizaSSL, sEmailRemetente, sSenhaEmail, sUserAuth, sPasswordAuth);
                    DataTable dt = new DataTable();
                    ADOBase objBase = new ADOBase();
                    StringBuilder sSQL = new StringBuilder();
                    Relatorio objRelatorio = new Relatorio();
                    DataTable dtRelatorio = new DataTable();
                    string sSQLAtualizar = string.Empty;
                    string sQuery = string.Empty;

                    timer1.Enabled = false;

                    #region Envio de Relatorio FollowUp Excel

                    txtStatus.Text += DateTime.Now.ToString("f") + " - [Relatório FollowUp] - Verificando emails pendentes de envio." + Environment.NewLine;
                    this.Refresh();

                    
                    sQuery = "Select mov.dt_ImpressaoConhecimento as Emissao, movNF.cd_notaFiscal as NF," +
                            //" rTrim(Isnull(mov.nr_Minuta, '')) as Minuta," +
                            " rTrim(Isnull(mov.nr_Conhecimento, '')) as CTe," +
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
                            "   And Mov.dt_Recepcao is null" +
                            //" And Not Exists(" +
                            //"   Select 1 from tbdOcorrenciaNota x " +
                            //"   Inner join tbdOcorrenciaManifesto y on x.id_Ocorrencia = y.id_OcorrenciaManifesto" +
                            //"   Where x.id_Movimento = MovNF.id_Movimento " +
                            //"       And x.nr_NotaFiscal = MovNF.cd_NotaFiscal " +
                            //"       And y.tp_BaixaAutomatica = 'S'" +
                            //"   )" +
                            " Order by MovNF.id_Movimento, MovNF.cd_NotaFiscal";

                    sSQL.Clear();

                    sSQL.Append("Select").Append(Environment.NewLine);
                    sSQL.Append("Distinct a.id_Cliente, b.ds_Pessoa, a.ds_EmailDestino, a.ds_AssuntoEmail, a.ds_CorpoEmail").Append(Environment.NewLine);
                    sSQL.Append("From tbdExtraClienteEmail a (Nolock) ").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPessoa b (Nolock) on a.id_Cliente = b.id_Pessoa").Append(Environment.NewLine);
                    sSQL.Append("Where a.tp_RelatorioFollowUp = 'S'");

                    if (bTrace)
                        Log.gravaLog("Query: " + sSQL.ToString(), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                    dt = objBase.RealizaPesquisaSQL(sSQL.ToString());

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        txtStatus.Text += DateTime.Now.ToString("f") + " - [Relatório FollowUp] - Nenhum cliente cadastrado para envio de relatório." + Environment.NewLine;
                        this.Refresh();
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (bTrace)
                                Log.gravaLog("Query: " + String.Format(sQuery, dr["id_Cliente"].ToString(), dtpDataCorte.Value.ToString("yyyy-MM-dd")), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                            dtRelatorio = objBase.RealizaPesquisaSQL(String.Format(sQuery, dr["id_Cliente"].ToString(), dtpDataCorte.Value.ToString("yyyy-MM-dd")));

                            if (dtRelatorio == null || dtRelatorio.Rows.Count == 0)
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - [Relatório FollowUp] - Não há emissões hoje para o cliente." + dr["ds_Pessoa"] + Environment.NewLine;
                                this.Refresh();
                                continue;
                            }

                            string sArquivoGerado = objRelatorio.GerarExcel(dtRelatorio, "CRONOGRAMA DE INFORMAÇÕES", "O", AppDomain.CurrentDomain.BaseDirectory + "Relatorios\\", "Relatorio_FollowUp");

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

                    #region Envio de Relatorio PrazoEntrega Excel

                    txtStatus.Text += DateTime.Now.ToString("f") + " - [Relatório Prazo Entrega] - Verificando emails pendentes de envio." + Environment.NewLine;
                    this.Refresh();


                    sQuery = "Select mov.dt_ImpressaoConhecimento as Emissao, Isnull(Agend.dt_Agendamento, Mov.dt_PrazoEntrega) as Prazo_Entrega, movNF.cd_notaFiscal as NF," +
                            " rTrim(Isnull(mov.nr_Conhecimento, '')) as CTe," +
                            " rTrim(rem.ds_Pessoa) as Remetente, rTrim(cidrem.ds_Cidade) Cidade_Origem, rTrim(estrem.cd_Estado) UF_Origem," +
                            " rTrim(fat.ds_Pessoa) as Faturado, rTrim(mov.ds_Cliente) as Destinatario, rTrim(ciddest.ds_Cidade) Cidade_Destinatario, " +
                            " rTrim(estdest.cd_Estado) UF_Destinatario, " +
                            " movNF.vl_NotaFiscal Valor_NF, movNF.qt_Volume as Volume," +
                            " movNF.kg_Mercadoria as Peso, mov.vl_Frete Valor_Frete" +
                            " From tbdMovimento mov (Nolock)" +
                            " Inner join tbdMovimentoNotaFiscal MovNF (Nolock) on MovNF.id_Movimento = mov.id_Movimento" +
                            " Left  join tbd_Site_MovimentoAgendamento Agend on MovNF.id_Movimento = Agend.id_Movimento And MovNF.cd_NotaFiscal = Agend.cd_NotaFiscal" +
                            " Inner join tbdPessoa fat (Nolock) on mov.id_ClienteFaturamento = fat.id_Pessoa" +
                            " Inner join tbdPessoa rem (Nolock) on mov.id_Remetente = rem.id_Pessoa" +
                            " Inner join tbdCidade cidrem (Nolock) on mov.id_CidadeOrigem = cidrem.id_Cidade" +
                            " Inner join tbdEstado estrem (Nolock) on cidrem.id_Estado = estrem.id_Estado" +
                            " Inner join tbdCidade ciddest (Nolock) on mov.id_Cidade = ciddest.id_Cidade" +
                            " Inner join tbdEstado estdest (Nolock) on ciddest.id_Estado = estdest.id_Estado" +
                            " Where id_ClienteFaturamento = {0} And mov.dt_ImpressaoConhecimento >= '{1}'" +
                            "   And Mov.dt_Recepcao is null" +
                            //" And Not Exists(" +
                            //"   Select 1 from tbdOcorrenciaNota x " +
                            //"   Inner join tbdOcorrenciaManifesto y on x.id_Ocorrencia = y.id_OcorrenciaManifesto" +
                            //"   Where x.id_Movimento = MovNF.id_Movimento " +
                            //"       And x.nr_NotaFiscal = MovNF.cd_NotaFiscal " +
                            //"       And y.tp_BaixaAutomatica = 'S'" +
                            //"   )" +
                            " Order by MovNF.id_Movimento, MovNF.cd_NotaFiscal";

                    sSQL.Clear();

                    sSQL.Append("Select").Append(Environment.NewLine);
                    sSQL.Append("Distinct a.id_Cliente, b.ds_Pessoa, a.ds_EmailDestino, a.ds_AssuntoEmail, a.ds_CorpoEmail").Append(Environment.NewLine);
                    sSQL.Append("From tbdExtraClienteEmail a (Nolock) ").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPessoa b (Nolock) on a.id_Cliente = b.id_Pessoa").Append(Environment.NewLine);
                    sSQL.Append("Where a.tp_RelatorioPrazoEntrega = 'S'");

                    if (bTrace)
                        Log.gravaLog("Query: " + sSQL.ToString(), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                    dt = objBase.RealizaPesquisaSQL(sSQL.ToString());

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        txtStatus.Text += DateTime.Now.ToString("f") + " - [Relatório Prazo Entrega] - Nenhum cliente cadastrado para envio de relatório." + Environment.NewLine;
                        this.Refresh();
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (bTrace)
                                Log.gravaLog("Query: " + String.Format(sQuery, dr["id_Cliente"].ToString(), dtpDataCorte.Value.ToString("yyyy-MM-dd")), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                            dtRelatorio = objBase.RealizaPesquisaSQL(String.Format(sQuery, dr["id_Cliente"].ToString(), dtpDataCorte.Value.ToString("yyyy-MM-dd")));

                            if (dtRelatorio == null || dtRelatorio.Rows.Count == 0)
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - [Relatório Prazo Entrega] - Não há emissões hoje para o cliente." + dr["ds_Pessoa"] + Environment.NewLine;
                                this.Refresh();
                                continue;
                            }

                            string sArquivoGerado = objRelatorio.GerarExcel(dtRelatorio, "CRONOGRAMA DE ENTREGAS", "L", AppDomain.CurrentDomain.BaseDirectory + "Relatorios\\", "Relatorio_PrazoEntrega");

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
                    ProximaAtualizacao(1);

                }

                if (DateTime.Now >= dtProximaAtualizacaoArquivos)
                {
                    string sStatusEnvio = string.Empty;
                    EnvioEmail objEnvioEmail;
                    DataTable dt = new DataTable();
                    ADOBase objBase = new ADOBase();
                    StringBuilder sSQL = new StringBuilder();
                    Relatorio objRelatorio = new Relatorio();
                    DataTable dtRelatorio = new DataTable();
                    string sSQLAtualizar = string.Empty;
                    string sQuery = string.Empty;
                    string sTemplateUpdate = string.Empty;
                    bool bRet = false;
                    string sArquivoGerado = string.Empty;
                    string sPDFGerado = string.Empty;
                    string listaXML = string.Empty;

                    timer1.Enabled = false;

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

                    if (dt != null && dt.Rows.Count != 0)
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

                            if (sArquivos.Length > 0)
                            {
                                sStatusEnvio = objEnvioEmail.EnviarMensagemEmail(dr["ds_Email"].ToString(),
                                                                    "", dr["ds_AssuntoEmail"].ToString(), dr["ds_TextoEmail"].ToString(), sArquivos.Substring(0, sArquivos.Length - 1));

                                if (sStatusEnvio.ToUpper() == "OK")
                                {
                                    foreach (FileInfo fi in d.GetFiles())
                                    {
                                        txtStatus.Text += DateTime.Now.ToString("f") + " - Email enviado o cliente: " +
                                        dr["ds_Pessoa"].ToString().Trim() + " - Arquivo: " + fi.Name + Environment.NewLine;

                                        if (File.Exists(diretorioCliente + "\\Transferidos\\" + fi.Name))
                                        {
                                            fi.Delete();
                                        }
                                        else
                                        {
                                            fi.MoveTo(diretorioCliente + "\\Transferidos\\" + fi.Name);
                                        }

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

                    #region Envio de PreAlert Aéreo

                    txtStatus.Text += DateTime.Now.ToString("f") + " - Verificando Pré-Alert Aéreo pendentes de envio." + Environment.NewLine;
                    this.Refresh();

                    sQuery = "Select rTrim(Cia.ds_Pessoa) as Cia_Aerea, fechto.nr_AWB as AWB, Ag.ds_Pessoa as Agente_Entrega," +
                            " rTrim(Isnull(mov.nr_Conhecimento, '')) as CTe, 'CTe' + Convert(varchar, mov.id_Movimento) + '_Auto.xml' as Arquivo," +
                            " Mov.dt_ImpressaoConhecimento as Emissao, rTrim(rem.ds_Pessoa) as Remetente, rTrim(cidrem.ds_Cidade) Cidade_Origem, rTrim(estrem.cd_Estado) UF_Origem," +
                            " rTrim(mov.ds_Cliente) as Destinatario, rTrim(ciddest.ds_Cidade) Cidade_Destinatario, rTrim(estdest.cd_Estado) UF_Destinatario, " +
                            " mov.vl_NotaFiscal Valor_NF, mov.qt_Volume as Volume, mov.kg_Mercadoria as Peso" +
                            " From tbdFechamentoCiaAerea fechto (Nolock)" +
                            " Inner join tbdFechamentoCiaAereaMovimento fechtoMov (Nolock) on fechto.id_FechamentoCiaAerea = fechtoMov.id_FechamentoCiaAerea" +
                            " Inner join tbdMovimento Mov (Nolock) on fechtoMov.id_Movimento = Mov.id_Movimento" +
                            " Inner join tbdPessoa Cia (Nolock) on fechto.id_CiaAerea = Cia.id_Pessoa" +
                            " Inner join tbdPessoa Rem (Nolock) on Mov.id_Remetente = Rem.id_Pessoa" +
                            " Inner join tbdCidade CidRem (Nolock) on Rem.id_Cidade = CidRem.id_Cidade" +
                            " Inner join tbdEstado EstRem (Nolock) on CidRem.id_Estado = EstRem.id_Estado" +
                            " Inner join tbdCidade CidDest (Nolock) on Mov.id_CidadeDestinatario = CidDest.id_Cidade" +
                            " Inner join tbdEstado EstDest (Nolock) on CidDest.id_Estado = EstDest.id_Estado" +
                            " Inner join tbdPessoa Ag (Nolock) on fechto.id_Destinatario = Ag.id_Pessoa" +
                            " Where fechto.id_Destinatario = {0} " +
                            "   And fechto.tp_GeradoPreAlert = 'S' And Isnull(fechto.tp_EnviadoPreAlert, '') <> 'S'" +
                            " Order by fechto.nr_AWB, Rem.ds_Pessoa, Mov.nr_Conhecimento";

                    sSQL.Clear();

                    sSQL.Append("Select").Append(Environment.NewLine);
                    sSQL.Append("Distinct fechto.id_Destinatario as id_Agente, fechto.nr_PreAlertEmail, Ag.ds_Pessoa as ds_Agente, Ag.cd_Email").Append(Environment.NewLine);
                    sSQL.Append("From tbdFechamentoCiaAerea fechto (Nolock) ").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPessoa Ag (Nolock) on fechto.id_Destinatario = Ag.id_Pessoa").Append(Environment.NewLine);
                    sSQL.Append("Where fechto.tp_GeradoPreAlert = 'S' And Isnull(fechto.tp_EnviadoPreAlert, '') <> 'S'");

                    sTemplateUpdate = "Update tbdFechamentoCiaAerea set tp_EnviadoPreAlert = 'S'" + Environment.NewLine +
                        "Where id_Destinatario = {0}" + Environment.NewLine +
                        "   And tp_GeradoPreAlert = 'S'" + Environment.NewLine +
                        "   And nr_PreAlertEmail = {1}";

                    if (bTrace)
                        Log.gravaLog("Query: " + sSQL.ToString(), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                    dt = objBase.RealizaPesquisaSQL(sSQL.ToString());

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        txtStatus.Text += DateTime.Now.ToString("f") + " - Nenhum Pré-Alert Aéreo pendente de envio." + Environment.NewLine;
                        this.Refresh();
                    }
                    else
                    {
                        objEnvioEmail = null;

                        objEnvioEmail = new EnvioEmail(sHost, iPorta, bUtilizaSSL, sEmailRemetentePreAlert, sSenhaEmailPreAlert, sUserAuth, sPasswordAuth);

                        foreach (DataRow dr in dt.Rows)
                        {
                            dtRelatorio = objBase.RealizaPesquisaSQL(String.Format(sQuery, dr["id_Agente"].ToString()));

                            sArquivoGerado = ""; sPDFGerado = ""; listaXML = "";

                            sArquivoGerado = objRelatorio.GerarExcel(dtRelatorio, "PRE ALERT", "M", AppDomain.CurrentDomain.BaseDirectory + "Relatorios\\", "PreAlert");

                            // Arquivo PDF do Relatório
                            sPDFGerado = objRelatorio.ExportarExcelParaPDF(sArquivoGerado, sArquivoGerado.Replace("xlsx", "pdf"));

                            if (sPDFGerado != string.Empty)
                            {
                                sArquivoGerado += ";" + sPDFGerado;
                            }

                            // Lista de XML dos CTes
                            listaXML = objRelatorio.RetornaArquivosXML(sDiretorioXML, dtRelatorio);

                            if (listaXML != string.Empty)
                            {
                                sArquivoGerado += ";" + listaXML;
                            }

                            sStatusEnvio = objEnvioEmail.EnviarMensagemEmail(
                                dr["cd_Email"].ToString(),
                                "",
                                "Pré-Alert",
                                "Segue anexo relação de cargas a entregar.",
                                sArquivoGerado
                                );

                            if (sStatusEnvio.ToUpper() == "OK")
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - Email enviado para: " + dr["cd_Email"] + Environment.NewLine;
                                this.Refresh();

                                bRet = objBase.ExecutaComando(string.Format(sTemplateUpdate, dr["id_Agente"], dr["nr_PreAlertEmail"]));

                                if (bRet)
                                {
                                    txtStatus.Text += DateTime.Now.ToString("f") +
                                    " - PréAlert: " + dr["nr_PreAlertEmail"].ToString().Trim() +
                                    " - Status atualizado para enviado." + Environment.NewLine;
                                    this.Refresh();

                                }
                                else
                                {
                                    txtStatus.Text += DateTime.Now.ToString("g") + " - Ocorreu erro atualizar o status do PréAlert." + Environment.NewLine;
                                    this.Refresh();
                                }
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

                    #endregion Envio de PreAlert

                    #region Envio de PreAlert Terrestre

                    txtStatus.Text += DateTime.Now.ToString("f") + " - Verificando Pré-Alert Terrestre pendentes de envio." + Environment.NewLine;
                    this.Refresh();

                    sQuery = "Select Pre.id_PreManifesto as Pre_Manifesto, rtrim(Pre.cm_PreManifesto) as Observação, rtrim(mov.nr_ConhecimentoTransportadora) as CTRC3, " +
                            " rTrim(Ag.ds_Pessoa) as Agente_Entrega," +
                            " rTrim(Isnull(mov.nr_Conhecimento, '')) as CTe, 'CTe' + Convert(varchar, mov.id_Movimento) + '_Auto.xml' as Arquivo, " +
                            " Mov.dt_ImpressaoConhecimento as Emissao, " +
                            " rTrim(rem.ds_Pessoa) as Remetente, rTrim(cidrem.ds_Cidade) Cidade_Origem, rTrim(estrem.cd_Estado) UF_Origem," +
                            " rTrim(mov.ds_Cliente) as Destinatario, rTrim(ciddest.ds_Cidade) Cidade_Destinatario, rTrim(estdest.cd_Estado) UF_Destinatario, " +
                            " mov.vl_NotaFiscal Valor_NF, mov.qt_Volume as Volume, mov.kg_Mercadoria as Peso" +
                            " From tbdPreManifesto Pre (Nolock)" +
                            " Inner join tbdPreManifestoItem Item (Nolock) on Pre.id_PreManifesto = Item.id_PreManifesto" +
                            " Inner join tbdMovimento Mov (Nolock) on Item.id_Movimento = Mov.id_Movimento" +
                            " Inner join tbdPessoa Rem (Nolock) on Mov.id_Remetente = Rem.id_Pessoa" +
                            " Inner join tbdCidade CidRem (Nolock) on Rem.id_Cidade = CidRem.id_Cidade" +
                            " Inner join tbdEstado EstRem (Nolock) on CidRem.id_Estado = EstRem.id_Estado" +
                            " Inner join tbdCidade CidDest (Nolock) on Mov.id_CidadeDestinatario = CidDest.id_Cidade" +
                            " Inner join tbdEstado EstDest (Nolock) on CidDest.id_Estado = EstDest.id_Estado" +
                            " Inner join tbdPessoa Ag (Nolock) on mov.id_Agente = Ag.id_Pessoa" +
                            " Where Mov.id_Agente = {0} " +
                            "   And Pre.tp_GeradoPreAlert = 'S' And Isnull(Pre.tp_EnviadoPreAlert, '') <> 'S'" +
                            " Order by Ag.ds_Pessoa, Rem.ds_Pessoa, Mov.nr_Conhecimento";

                    sSQL.Clear();

                    sSQL.Append("Select").Append(Environment.NewLine);
                    sSQL.Append("Distinct Pre.id_PreManifesto, Mov.id_Agente as id_Agente, Pre.nr_PreAlertEmail, Ag.ds_Pessoa as ds_Agente, Ag.cd_Email").Append(Environment.NewLine);
                    sSQL.Append("From tbdPreManifesto Pre (Nolock) ").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPreManifestoItem Item (Nolock) on Pre.id_PreManifesto = Item.id_PreManifesto").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdMovimento Mov(Nolock) on Item.id_Movimento = Mov.id_Movimento").Append(Environment.NewLine);
                    sSQL.Append("Inner join tbdPessoa Ag (Nolock) on Mov.id_Agente = Ag.id_Pessoa").Append(Environment.NewLine);
                    sSQL.Append("Where Pre.tp_GeradoPreAlert = 'S' And Isnull(Pre.tp_EnviadoPreAlert, '') <> 'S'");

                    sTemplateUpdate = "Update tbdPreManifesto set tp_EnviadoPreAlert = 'S'" + Environment.NewLine +
                        "Where id_PreManifesto = {0}" + Environment.NewLine +
                        "   And tp_GeradoPreAlert = 'S'" + Environment.NewLine +
                        "   And nr_PreAlertEmail = {1}";

                    if(bTrace)
                        Log.gravaLog("Query: " + sSQL.ToString(), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                    dt = objBase.RealizaPesquisaSQL(sSQL.ToString());

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        txtStatus.Text += DateTime.Now.ToString("f") + " - Nenhum Pré-Alert Terrestre pendente de envio." + Environment.NewLine;
                        this.Refresh();
                    }
                    else
                    {
                        objEnvioEmail = null;

                        objEnvioEmail = new EnvioEmail(sHost, iPorta, bUtilizaSSL, sEmailRemetentePreAlert, sSenhaEmailPreAlert, sUserAuth, sPasswordAuth);

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (bTrace)
                                Log.gravaLog("Query: " + String.Format(sQuery, dr["id_Agente"].ToString()), AppDomain.CurrentDomain.BaseDirectory + "Log\\");

                            dtRelatorio = objBase.RealizaPesquisaSQL(String.Format(sQuery, dr["id_Agente"].ToString()));

                            sArquivoGerado = ""; sPDFGerado = ""; listaXML = "";

                            sArquivoGerado = objRelatorio.GerarExcel(dtRelatorio, "PRE ALERT", "N", AppDomain.CurrentDomain.BaseDirectory + "Relatorios\\", "PreAlert");

                            // Arquivo PDF do Relatório
                            sPDFGerado = objRelatorio.ExportarExcelParaPDF(sArquivoGerado, sArquivoGerado.Replace("xlsx", "pdf"));

                            if (sPDFGerado != string.Empty)
                            {
                                sArquivoGerado += ";" + sPDFGerado;
                            }

                            // Lista de XML dos CTes
                            listaXML = objRelatorio.RetornaArquivosXML(sDiretorioXML, dtRelatorio);

                            if (listaXML != string.Empty)
                            {
                                sArquivoGerado += ";" + listaXML;
                            }

                            sStatusEnvio = objEnvioEmail.EnviarMensagemEmail(
                                dr["cd_Email"].ToString(),
                                "",
                                "Pré-Alert",
                                "Segue anexo relação de cargas a entregar.",
                                sArquivoGerado
                                );

                            if (sStatusEnvio.ToUpper() == "OK")
                            {
                                txtStatus.Text += DateTime.Now.ToString("f") + " - Email enviado para: " + dr["cd_Email"] + Environment.NewLine;
                                this.Refresh();

                                bRet = objBase.ExecutaComando(string.Format(sTemplateUpdate, dr["id_PreManifesto"], dr["nr_PreAlertEmail"]));

                                if (bRet)
                                {
                                    txtStatus.Text += DateTime.Now.ToString("f") +
                                    " - PréAlert: " + dr["nr_PreAlertEmail"].ToString().Trim() +
                                    " - Status atualizado para enviado." + Environment.NewLine;
                                    this.Refresh();

                                }
                                else
                                {
                                    txtStatus.Text += DateTime.Now.ToString("g") + " - Ocorreu erro atualizar o status do PréAlert." + Environment.NewLine;
                                    this.Refresh();
                                }
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

                    #endregion Envio de PreAlert Terrestre

                    ProximaAtualizacao(2);
                }

                if (timer1.Enabled == false)
                {
                    timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text += DateTime.Now.ToString("f") + " - Ocorreu erro durante o processo de envio: " + Environment.NewLine
                    + ex.Message.ToString() + Environment.NewLine
                    + "Tracc: " + ex.StackTrace + Environment.NewLine;
                this.Refresh();

                ProximaAtualizacao(1);
                ProximaAtualizacao(2);
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

        private void frmEmail_Load(object sender, EventArgs e)
        {
            lblVersao.Text = "Versão: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();  
        }
    }
}
