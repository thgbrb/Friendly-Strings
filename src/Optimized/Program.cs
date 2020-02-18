// c# read csv :: https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
// Question like: 302
// Anwsered like: 393
// c# save list to file :: https://stackoverflow.com/questions/15300572/saving-lists-to-txt-file
// Question like: 38
// Anwsered like: 61

using Shared;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Shared.Parsers;

namespace Playing
{
    class Program
    {
        private const string SOURCE_FILE = "D:\\Working\\dados\\gastos121950.csv";
        private const string DATABASE_FILE = "D:\\Working\\dados\\database.csv";
        static StringBuilder sb = new StringBuilder();
        static void Main(string[] args)
        {
            using (TextWriter tw = new StreamWriter(path: DATABASE_FILE, append: true))
            {
                foreach (var despesa in CarregarLinhas())
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
            }

            SharedHelpers.Summary();
        }

        public static IEnumerable<GastoLinha> CarregarLinhas()
        {
            using (var reader = new StreamReader(SOURCE_FILE))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();

                    var despesa = Parse(linha);

                    yield return despesa;
                }
            }
        }

        public static GastoLinha Parse(string linha)
        {
            var t = ParseLine(linha);

            for (int i = t[2] + 1; i < t[3]; i++)
                sb.Append(linha[i]);
            var tipoGasto = sb.ToString();
            sb.Clear();


            for (int i = t[7] + 1; i < t[8]; i++)
                sb.Append(linha[i]);
            var processo = long.Parse(sb.ToString());
            sb.Clear();

            for (int i = t[9] + 1; i < t[10]; i++)
                sb.Append(linha[i]);
            var favorecido = sb.ToString();
            sb.Clear();

            for (int i = t[10] + 1; i < t[11]; i++)
                if (char.IsDigit(linha[i]))
                    sb.Append(linha[i]);
            var cnpj = sb.ToString();
            sb.Clear();

            for (int i = t[11] + 1; i < t[12]; i++)
                sb.Append(linha[i]);
            var poder = sb.ToString();
            sb.Clear();

            for (int i = t[19] + 1; i < t[20]; i++)
                sb.Append(linha[i]);
            var categoria = sb.ToString();
            sb.Clear();

            for (int i = t[27] + 1; i < t[28]; i++)
                sb.Append(linha[i]);
            var rubrica = sb.ToString();
            sb.Clear();

            for (int i = t[31] + 1; i < t[32]; i++)
                sb.Append(linha[i]);
            var funcao = sb.ToString();
            sb.Clear();

            for (int i = t[43] + 1; i < t[44]; i++)
                sb.Append(linha[i]);
            var valorMonetario = double.Parse(sb.ToString());
            sb.Clear();

            return new GastoLinha(
                tipoGasto: tipoGasto,
                processo: processo,
                favorecido: favorecido,
                cNPJ: cnpj,
                poder: poder,
                categoria: categoria,
                rubrica: rubrica,
                funcao: funcao,
                valor: valorMonetario);
        }
    }
}