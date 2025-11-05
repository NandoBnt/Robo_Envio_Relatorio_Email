using System;
using System.IO;

namespace Robo_EnvioEmail
{
    public static class Log
    {
        public  static void gravaLog(Exception ex, string dirLog)
        {
            string fileName = "Log_Robo_Email_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!dirLog.EndsWith("\\"))
                dirLog += "\\";

            if (!Directory.Exists(dirLog))
            {
                // Cria o diretório se não existir
                Directory.CreateDirectory(dirLog);
            }

            StreamWriter sw = new StreamWriter(dirLog + fileName, true);

            sw.WriteLine("------INÍCIO DO TRATAMENTO DE ERRO------");
            sw.WriteLine(DateTime.Now.ToShortTimeString());
            sw.WriteLine(ex.Message);
            sw.WriteLine(ex.StackTrace);
            sw.WriteLine(ex.Source);
            sw.WriteLine("------FINAL DO TRATAMENTO DE ERRO------");
            sw.WriteLine("");

            sw.Flush();
            sw.Close();
            sw.Dispose();

        }

        public static void gravaLog(string mensagem, string dirLog)
        {
            string fileName = "Log_Robo_Email_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!dirLog.EndsWith("\\"))
                dirLog += "\\";

            if (!Directory.Exists(dirLog))
            {
                // Cria o diretório se não existir
                Directory.CreateDirectory(dirLog);
            }

            StreamWriter sw = new StreamWriter(dirLog + fileName, true);

            sw.WriteLine("------INÍCIO DA MENSAGEM------");
            sw.WriteLine(DateTime.Now.ToShortTimeString());
            sw.WriteLine(mensagem);
            sw.WriteLine("------FINAL DA MENSAGEM------");
            sw.WriteLine("");

            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
