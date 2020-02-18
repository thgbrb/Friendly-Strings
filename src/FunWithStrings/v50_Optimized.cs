using Shared;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Shared.Parsers;

namespace Playing
{
    public class v50_Optimized
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly GastoLinha _gastoLinha = new GastoLinha();

        public void Run()
        {
            using (TextWriter tw = new StreamWriter(path: Configuration.DATABASE_FILE, append: true))
            {
                foreach (var despesa in LoadLines())
                {
                    _stringBuilder
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

                    tw.Write(_stringBuilder.ToString());

                    _stringBuilder.Clear();
                }
            }

            SharedHelpers.Summary();
        }

        public IEnumerable<GastoLinha> LoadLines()
        {
            using (var reader = new StreamReader(Configuration.SOURCE_FILE))
            {
                while (!reader.EndOfStream)
                {
                    var gastoLinhas = reader.ReadLine();

                    var gastoLinha = Parse(gastoLinhas);

                    yield return gastoLinha;
                }
            }
        }

        public GastoLinha Parse(string gastoLinha)
        {
            var separatorPosition = ParseLineWithIntArray(gastoLinha);

            for (int i = separatorPosition[2] + 1; i < separatorPosition[3]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var tipoGasto = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[7] + 1; i < separatorPosition[8]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var processo = long.Parse(_stringBuilder.ToString());
            _stringBuilder.Clear();

            for (int i = separatorPosition[9] + 1; i < separatorPosition[10]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var favorecido = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[10] + 1; i < separatorPosition[11]; i++)
                if (char.IsDigit(gastoLinha[i]))
                    _stringBuilder.Append(gastoLinha[i]);
            var cnpj = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[11] + 1; i < separatorPosition[12]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var poder = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[19] + 1; i < separatorPosition[20]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var categoria = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[27] + 1; i < separatorPosition[28]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var rubrica = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[31] + 1; i < separatorPosition[32]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var funcao = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[43] + 1; i < separatorPosition[44]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            var valorMonetario = double.Parse(_stringBuilder.ToString());
            _stringBuilder.Clear();

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