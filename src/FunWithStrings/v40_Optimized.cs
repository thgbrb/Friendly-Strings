using Shared;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Shared.Parsers;

namespace Playing
{
    public class v40_Optimized
    {
        private readonly GastoLinha _gastoLinha = new GastoLinha();

        public void Run()
        {
            using (TextWriter tw = new StreamWriter(path: Configuration.DATABASE_FILE, append: false))
            {
                foreach (var despesa in LoadLines())
                {
                    var stringBuilder = new StringBuilder();

                    stringBuilder
                        .Append(despesa.TipoGasto)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Processo)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Favorecido)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.CNPJ)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Poder)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Categoria)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Rubrica)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Funcao)
                        .Append(Configuration.SEPARATOR)
                        .Append(despesa.Valor)
                        .Append(Configuration.SEPARATOR)
                        .Append("\n");

                    tw.Write(stringBuilder.ToString());
                }
            }

            SharedHelpers.Summary();
        }

        public IList<GastoLinha> LoadLines()
        {
            var gastoLinhas = new List<GastoLinha>();

            using (var reader = new StreamReader(Configuration.SOURCE_FILE))
            {
                while (!reader.EndOfStream)
                {
                    var linhas = reader.ReadLine();

                    var gastoLinha = Parse(linhas);

                    gastoLinhas.Add(item: gastoLinha);
                }
            }

            return gastoLinhas;
        }

        public GastoLinha Parse(string gastoLinha)
        {
            var separatorPosition = ParseLineWithIntArray(gastoLinha);
            var stringBuilder = new StringBuilder();

            for (int i = separatorPosition[2] + 1; i < separatorPosition[3]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var tipoGasto = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[7] + 1; i < separatorPosition[8]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var processo = long.Parse(stringBuilder.ToString());
            stringBuilder.Clear();

            for (int i = separatorPosition[9] + 1; i < separatorPosition[10]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var favorecido = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[10] + 1; i < separatorPosition[11]; i++)
                if (char.IsDigit(gastoLinha[i]))
                    stringBuilder.Append(gastoLinha[i]);
            var cnpj = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[11] + 1; i < separatorPosition[12]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var poder = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[19] + 1; i < separatorPosition[20]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var categoria = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[27] + 1; i < separatorPosition[28]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var rubrica = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[31] + 1; i < separatorPosition[32]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var funcao = stringBuilder.ToString();
            stringBuilder.Clear();

            for (int i = separatorPosition[43] + 1; i < separatorPosition[44]; i++)
                stringBuilder.Append(gastoLinha[i]);
            var valorMonetario = double.Parse(stringBuilder.ToString());
            stringBuilder.Clear();

            return _gastoLinha.SetValues(
                tipoGasto: tipoGasto,
                processo: processo,
                favorecido: favorecido,
                cnpj: cnpj,
                poder: poder,
                categoria: categoria,
                rubrica: rubrica,
                funcao: funcao,
                valor: valorMonetario);
        }
    }
}