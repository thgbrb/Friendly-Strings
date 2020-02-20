// c# read csv :: https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
// Question like: 302
// Anwsered like: 393
// c# save list to file :: https://stackoverflow.com/questions/15300572/saving-lists-to-txt-file
// Question like: 38
// Anwsered like: 61

using Shared;
using System.Collections.Generic;
using System.IO;

namespace Playing
{
    public class v01_Stackoverflow
    {
        public void Run()
        {
            var gastoLinhas = new List<GastoLinha>();

            using (var reader = new StreamReader(Configuration.SOURCE_FILE))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    var valor = linha.Split(';');

                    var gastoLinha = new GastoLinha(
                        tipoGasto: valor[3],
                        processo: long.Parse(valor[8]),
                        favorecido: valor[10],
                        cnpj: valor[11]
                            .Replace(".", "")
                            .Replace("/", "")
                            .Replace("-", ""),
                        poder: valor[12],
                        categoria: valor[20],
                        rubrica: valor[28],
                        funcao: valor[32],
                        valor: double.Parse(valor[44]));

                    gastoLinhas.Add(item: gastoLinha);
                }
            }

            using (TextWriter tw = new StreamWriter(path: Configuration.DATABASE_FILE, append: false))
            {
                foreach (var gastoLinha in gastoLinhas)
                {
                    tw.Write(gastoLinha.TipoGasto + ";");
                    tw.Write(gastoLinha.Processo + ";");
                    tw.Write(gastoLinha.Favorecido + ";");
                    tw.Write(gastoLinha.CNPJ + ";");
                    tw.Write(gastoLinha.Poder + ";");
                    tw.Write(gastoLinha.Categoria + ";");
                    tw.Write(gastoLinha.Rubrica + ";");
                    tw.Write(gastoLinha.Funcao + ";");
                    tw.Write(gastoLinha.Valor + "\n");
                }
            }

            SharedHelpers.Summary();
        }
    }
}