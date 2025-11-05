using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;

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
            InternetAddressList listaDestino = new InternetAddressList();
            InternetAddressList listaCC = new InternetAddressList();

            try
            {
                message.From.Add(new MailboxAddress(_email, _email));
                var listaEmails = _destinatario.Split(';');

                foreach (string sDestino in listaEmails)
                {
                    if (EmailValido(sDestino))
                    {
                        listaDestino.Add(new MailboxAddress(sDestino.TrimStart().TrimEnd(), sDestino.TrimStart().TrimEnd()));
                    }
                    else 
                    {
                        return "Email inválido: " + sDestino;
                    }
                }

                message.To.AddRange(listaDestino);

                //List de emails CC
                if (_cc != null && _cc != String.Empty)
                {
                    var listaEmailsCC = _cc.Split(';');

                    foreach (string sCC in listaEmailsCC)
                    {
                        listaCC.Add(new MailboxAddress(sCC.TrimStart().TrimEnd(), sCC.TrimStart().TrimEnd()));
                    }

                    message.Cc.AddRange(listaCC);
                }

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

                if (_porta == 587)
                {
                    smtpClient.Connect(_host, _porta, SecureSocketOptions.StartTls);
                }
                else
                {
                    smtpClient.Connect(_host, _porta, (_UtilizaSSL ? true : false));
                }

                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");

                if (_userAuth != string.Empty && _passwordAuth != string.Empty)
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

        public bool EmailValido(string email)
        {
            try
            {
                var endereco = new MailboxAddress(email, email);
                return endereco.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
