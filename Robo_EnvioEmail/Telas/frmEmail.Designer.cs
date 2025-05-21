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
            this.dtpDataCorte = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpSegundoEnvio = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpPrimeiroEnvio = new System.Windows.Forms.DateTimePicker();
            this.lblProximaAtualizacao = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIntervalo = new System.Windows.Forms.TextBox();
            this.lblProximaAtualizacaoArquivos = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnParar = new System.Windows.Forms.Button();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpDataCorte);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpSegundoEnvio);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpPrimeiroEnvio);
            this.groupBox1.Controls.Add(this.lblProximaAtualizacao);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(614, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuração do Envio de Emails";
            // 
            // dtpDataCorte
            // 
            this.dtpDataCorte.CustomFormat = "HH:mm";
            this.dtpDataCorte.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataCorte.Location = new System.Drawing.Point(179, 37);
            this.dtpDataCorte.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpDataCorte.Name = "dtpDataCorte";
            this.dtpDataCorte.Size = new System.Drawing.Size(91, 20);
            this.dtpDataCorte.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 22);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Data de Corte";
            // 
            // dtpSegundoEnvio
            // 
            this.dtpSegundoEnvio.CustomFormat = "HH:mm";
            this.dtpSegundoEnvio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSegundoEnvio.Location = new System.Drawing.Point(93, 38);
            this.dtpSegundoEnvio.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpSegundoEnvio.Name = "dtpSegundoEnvio";
            this.dtpSegundoEnvio.Size = new System.Drawing.Size(75, 20);
            this.dtpSegundoEnvio.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Segundo Envio";
            // 
            // dtpPrimeiroEnvio
            // 
            this.dtpPrimeiroEnvio.CustomFormat = "HH:mm";
            this.dtpPrimeiroEnvio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPrimeiroEnvio.Location = new System.Drawing.Point(13, 37);
            this.dtpPrimeiroEnvio.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpPrimeiroEnvio.Name = "dtpPrimeiroEnvio";
            this.dtpPrimeiroEnvio.Size = new System.Drawing.Size(71, 20);
            this.dtpPrimeiroEnvio.TabIndex = 6;
            // 
            // lblProximaAtualizacao
            // 
            this.lblProximaAtualizacao.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProximaAtualizacao.Location = new System.Drawing.Point(284, 38);
            this.lblProximaAtualizacao.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProximaAtualizacao.Name = "lblProximaAtualizacao";
            this.lblProximaAtualizacao.Size = new System.Drawing.Size(120, 18);
            this.lblProximaAtualizacao.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Próxima Atualização";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Primeiro Envio";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnParar);
            this.groupBox2.Controls.Add(this.btnEnviar);
            this.groupBox2.Controls.Add(this.btnLimpar);
            this.groupBox2.Controls.Add(this.txtStatus);
            this.groupBox2.Location = new System.Drawing.Point(8, 175);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(614, 294);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // btnLimpar
            // 
            this.btnLimpar.Location = new System.Drawing.Point(513, 12);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(97, 21);
            this.btnLimpar.TabIndex = 7;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(4, 38);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(607, 247);
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
            this.groupBox3.Location = new System.Drawing.Point(8, 97);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(614, 73);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Arquivos EDI / Pré Alert";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Intervalo (Min)";
            // 
            // txtIntervalo
            // 
            this.txtIntervalo.Location = new System.Drawing.Point(13, 36);
            this.txtIntervalo.Name = "txtIntervalo";
            this.txtIntervalo.Size = new System.Drawing.Size(70, 20);
            this.txtIntervalo.TabIndex = 13;
            // 
            // lblProximaAtualizacaoArquivos
            // 
            this.lblProximaAtualizacaoArquivos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProximaAtualizacaoArquivos.Location = new System.Drawing.Point(95, 38);
            this.lblProximaAtualizacaoArquivos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProximaAtualizacaoArquivos.Name = "lblProximaAtualizacaoArquivos";
            this.lblProximaAtualizacaoArquivos.Size = new System.Drawing.Size(120, 18);
            this.lblProximaAtualizacaoArquivos.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(92, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Próxima Atualização";
            // 
            // btnParar
            // 
            this.btnParar.Location = new System.Drawing.Point(409, 12);
            this.btnParar.Margin = new System.Windows.Forms.Padding(2);
            this.btnParar.Name = "btnParar";
            this.btnParar.Size = new System.Drawing.Size(97, 21);
            this.btnParar.TabIndex = 9;
            this.btnParar.Text = "Parar";
            this.btnParar.UseVisualStyleBackColor = true;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(307, 12);
            this.btnEnviar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(97, 21);
            this.btnEnviar.TabIndex = 8;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // frmEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 474);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmail";
            this.Text = "Envio Automático de Emails";
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
    }
}