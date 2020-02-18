// c# read csv :: https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
// Question like: 302
// Anwsered like: 393
// c# save list to file :: https://stackoverflow.com/questions/15300572/saving-lists-to-txt-file
// Question like: 38
// Anwsered like: 61

using Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Playing
{
    class Program
    {
        private const string SOURCE_FILE = "D:\\repos\\FriendlyStrings\\dados\\gastos121950.csv";
        private const string DATABASE_FILE = "D:\\repos\\FriendlyStrings\\dados\\database.csv";

        static void Main(string[] args)
        {
            var despesas = new List<GastoLinha>();
            using (var reader = new StreamReader(SOURCE_FILE))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    var valor = linha.Split(';');

                    var veiculo = new GastoLinha(
                        tipoGasto: valor[3],
                        processo: long.Parse(valor[8]),
                        favorecido: valor[10],
                        cNPJ: valor[11]
                            .Replace(".", "")
                            .Replace("/", "")
                            .Replace("-", ""),
                        poder: valor[12],
                        categoria: valor[20],
                        rubrica: valor[28],
                        funcao: valor[32],
                        valor: double.Parse(valor[44]));

                    despesas.Add(item: veiculo);
                }
            }

            using (TextWriter tw = new StreamWriter(path: DATABASE_FILE, append: true))
            {
                foreach (var despesa in despesas)
                {
                    tw.Write(despesa.TipoGasto + ";");
                    tw.Write(despesa.Processo + ";");
                    tw.Write(despesa.Favorecido + ";");
                    tw.Write(despesa.CNPJ + ";");
                    tw.Write(despesa.Poder + ";");
                    tw.Write(despesa.Categoria + ";");
                    tw.Write(despesa.Rubrica + ";");
                    tw.Write(despesa.Funcao + ";");
                    tw.Write(despesa.Valor + "\n");
                }

                var totalGastos = despesas.Sum(item => item.Valor);

                tw.Write("Total de Gastos:" + totalGastos + "\n");
            }

            SharedHelpers.Summary();
        }
    }
}