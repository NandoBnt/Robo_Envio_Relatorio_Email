using System;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;

namespace Robo_EnvioEmail
{
    public class EnvioEmail
    {
        string _host;
        int _porta;
        bool _UtilizaSSL;
        string _email;
        string _senha;
        string _userAuth;
        string _passwordAuth;

        public EnvioEmail(string host, int porta, bool utilizaSSL, string email, string senha, string userAuth, string passwordAuth)
        {
            _host = host;
            _porta = porta;
            _UtilizaSSL = utilizaSSL;
            _email = email;
            _senha = senha;
            _userAuth = userAuth;
            _passwordAuth = passwordAuth;
        }

        public string EnviarMensagemEmail(string _destinatario, string _cc, string _assunto, string _mensagem, string _anexos)
        {
            SmtpClient smtpClient = new SmtpClient();
            MimeMessage message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(_email, _email));
                message.To.Add(new MailboxAddress(_destinatario, _destinatario));

                if (_cc != null && _cc != String.Empty)
                    message.Cc.Add(new MailboxAddress(_cc, _cc));

                message.Subject = _assunto.Trim();

                var builder = new BodyBuilder();
                builder.TextBody = _mensagem.Trim();

                if (_anexos != null && _anexos != String.Empty)
                {
                    var listaAnexos = _anexos.Split(';');

                    foreach (string sAnexo in listaAnexos)
                    {
                        FileInfo file = new FileInfo(sAnexo);

                        if (file.Exists)
                        {
                            if (sAnexo != null && _anexos != String.Empty)
                            {
                                builder.Attachments.Add(sAnexo.Trim());
                            }
                        }
                    }
                }

                message.Body = builder.ToMessageBody();
                
                smtpClient.Connect(_host, _porta, (_UtilizaSSL ? true : false));
                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");

                if(_userAuth != string.Empty && _passwordAuth != string.Empty)
                {
                    smtpClient.Authenticate(_userAuth, _passwordAuth);
                }
                else
                {
                    smtpClient.Authenticate(_email, _senha);
                }

                try
                {
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);
                    smtpClient.Dispose();
                }
                catch (SmtpCommandException ex)
                {
                    return "Ocorreu erro inesperado: " + ex.Message;
                }
                catch (SmtpProtocolException ex)
                {
                    return "Ocorreu erro inesperado: " + ex.Message;
                }

            }
            catch (Exception ex)
            {
                return "Erro inesperado: " + ex.Message;
            }

            return "OK";
        }
    }
}
