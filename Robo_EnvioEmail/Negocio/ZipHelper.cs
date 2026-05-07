using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Robo_EnvioEmail.Negocio
{
    public static class ZipHelper
    {
        public static string CriarZipArquivos(List<string> lista, string DiretorioZip)
        {
            try
            {
                string nomeZip = string.Empty;

                // Nome do ZIP
                nomeZip = DiretorioZip + "\\" + $"Anexos_{DateTime.Now:yyyyMMdd_HHmmss}.zip";

                // Caminho temporário
                string caminhoZip = Path.Combine(Path.GetTempPath(), nomeZip);

                // Remove zip antigo se existir
                if (File.Exists(caminhoZip))
                    File.Delete(caminhoZip);

                // Cria o ZIP
                using (var zip = ZipFile.Open(caminhoZip, ZipArchiveMode.Create))
                {
                    foreach (var arquivo in lista)
                    {
                        if (!File.Exists(arquivo))
                            continue;

                        // Nome do arquivo dentro do ZIP
                        string nomeArquivo = Path.GetFileName(arquivo);

                        zip.CreateEntryFromFile(
                            arquivo,
                            nomeArquivo,
                            CompressionLevel.Optimal
                        );
                    }
                }

                return caminhoZip;
            }
            catch (Exception ex) 
            { 
                return string.Empty;
            }

            
        }
    }
}



