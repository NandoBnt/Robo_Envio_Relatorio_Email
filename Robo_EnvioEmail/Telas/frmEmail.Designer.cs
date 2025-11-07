namespace Robo_EnvioEmail
{
    partial class frmEmail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblVersao = new System.Windows.Forms.Label();
            this.dtpDataCorte = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpSegundoEnvio = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpPrimeiroEnvio = new System.Windows.Forms.DateTimePicker();
            this.lblProximaAtualizacao = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnParar = new System.Windows.Forms.Button();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblProximaAtualizacaoArquivos = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIntervalo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblVersao);
            this.groupBox1.Controls.Add(this.dtpDataCorte);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpSegundoEnvio);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpPrimeiroEnvio);
            this.groupBox1.Controls.Add(this.lblProximaAtualizacao);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1091, 129);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Envio de Emails - Relatórios FollowUp || Prazo Entrega";
            // 
            // lblVersao
            // 
            this.lblVersao.AutoSize = true;
            this.lblVersao.Location = new System.Drawing.Point(814, 22);
            this.lblVersao.Name = "lblVersao";
            this.lblVersao.Size = new System.Drawing.Size(60, 20);
            this.lblVersao.TabIndex = 11;
            this.lblVersao.Text = "Versão";
            // 
            // dtpDataCorte
            // 
            this.dtpDataCorte.CustomFormat = "HH:mm";
            this.dtpDataCorte.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataCorte.Location = new System.Drawing.Point(268, 57);
            this.dtpDataCorte.Name = "dtpDataCorte";
            this.dtpDataCorte.Size = new System.Drawing.Size(134, 26);
            this.dtpDataCorte.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Data de Corte";
            // 
            // dtpSegundoEnvio
            // 
            this.dtpSegundoEnvio.CustomFormat = "HH:mm";
            this.dtpSegundoEnvio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSegundoEnvio.Location = new System.Drawing.Point(140, 58);
            this.dtpSegundoEnvio.Name = "dtpSegundoEnvio";
            this.dtpSegundoEnvio.Size = new System.Drawing.Size(110, 26);
            this.dtpSegundoEnvio.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Segundo Envio";
            // 
            // dtpPrimeiroEnvio
            // 
            this.dtpPrimeiroEnvio.CustomFormat = "HH:mm";
            this.dtpPrimeiroEnvio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPrimeiroEnvio.Location = new System.Drawing.Point(20, 57);
            this.dtpPrimeiroEnvio.Name = "dtpPrimeiroEnvio";
            this.dtpPrimeiroEnvio.Size = new System.Drawing.Size(104, 26);
            this.dtpPrimeiroEnvio.TabIndex = 0;
            // 
            // lblProximaAtualizacao
            // 
            this.lblProximaAtualizacao.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProximaAtualizacao.Location = new System.Drawing.Point(426, 58);
            this.lblProximaAtualizacao.Name = "lblProximaAtualizacao";
            this.lblProximaAtualizacao.Size = new System.Drawing.Size(180, 28);
            this.lblProximaAtualizacao.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(424, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Próxima Atualização";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Primeiro Envio";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnParar);
            this.groupBox2.Controls.Add(this.btnEnviar);
            this.groupBox2.Controls.Add(this.btnLimpar);
            this.groupBox2.Controls.Add(this.txtStatus);
            this.groupBox2.Location = new System.Drawing.Point(12, 269);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1091, 452);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // btnParar
            // 
            this.btnParar.Location = new System.Drawing.Point(783, 20);
            this.btnParar.Name = "btnParar";
            this.btnParar.Size = new System.Drawing.Size(146, 32);
            this.btnParar.TabIndex = 5;
            this.btnParar.Text = "Parar";
            this.btnParar.UseVisualStyleBackColor = true;
            this.btnParar.Click += new System.EventHandler(this.btnParar_Click);
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(629, 20);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(146, 32);
            this.btnEnviar.TabIndex = 4;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Location = new System.Drawing.Point(939, 20);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(146, 32);
            this.btnLimpar.TabIndex = 6;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(6, 58);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(1079, 378);
            this.txtStatus.TabIndex = 0;
            this.txtStatus.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblProximaAtualizacaoArquivos);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtIntervalo);
            this.groupBox3.Location = new System.Drawing.Point(12, 149);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1091, 112);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Envio de Emails - Arquivos EDI / Pré Alert";
            // 
            // lblProximaAtualizacaoArquivos
            // 
            this.lblProximaAtualizacaoArquivos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProximaAtualizacaoArquivos.Location = new System.Drawing.Point(142, 58);
            this.lblProximaAtualizacaoArquivos.Name = "lblProximaAtualizacaoArquivos";
            this.lblProximaAtualizacaoArquivos.Size = new System.Drawing.Size(180, 28);
            this.lblProximaAtualizacaoArquivos.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(138, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Próxima Atualização";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Intervalo (Min)";
            // 
            // txtIntervalo
            // 
            this.txtIntervalo.Location = new System.Drawing.Point(20, 55);
            this.txtIntervalo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIntervalo.Name = "txtIntervalo";
            this.txtIntervalo.Size = new System.Drawing.Size(103, 26);
            this.txtIntervalo.TabIndex = 3;
            // 
            // frmEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 729);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmail";
            this.Text = "Envio Automático de Emails";
            this.Load += new System.EventHandler(this.frmEmail_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblProximaAtualizacao;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox txtStatus;
        internal System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.DateTimePicker dtpSegundoEnvio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpPrimeiroEnvio;
        private System.Windows.Forms.DateTimePicker dtpDataCorte;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIntervalo;
        private System.Windows.Forms.Button btnParar;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.Label lblProximaAtualizacaoArquivos;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblVersao;
    }
}