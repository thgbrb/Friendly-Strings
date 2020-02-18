using Shared;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Shared.Parsers;

namespace Playing
{
    public class v90_Optimized
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly GastoLinha _gastoLinha = new GastoLinha();
        private readonly ArrayPool<char> _charArrayPool = ArrayPool<char>.Shared;

        public void Run()
        {
            using (TextWriter tw = new StreamWriter(path: Configuration.DATABASE_FILE, append: false))
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
            var stringBuilder = new StringBuilder();

            using (var reader = File.OpenRead(Configuration.SOURCE_FILE))
            {
                bool eof = false;

                while (reader.CanRead)
                {
                    stringBuilder.Clear();

                    while (!eof)
                    {
                        var readByte = reader.ReadByte();

                        if (readByte == -1)
                        {
                            eof = true;
                            break;
                        }

                        var character = (char)readByte;

                        if (character == '\r') continue;
                        if (character == '\n') break;

                        stringBuilder.Append(character);
                    }

                    if (eof) break;

                    var buffer = _charArrayPool.Rent(stringBuilder.Length);

                    try
                    {
                        for (int i = 0; i < stringBuilder.Length; i++)
                        {
                            buffer[i] = stringBuilder[i];
                        }

                        var gastoLinha = Parse(buffer);

                        yield return gastoLinha;
                    }
                    finally
                    {
                        _charArrayPool.Return(buffer);
                        _gastoLinha.Clear();
                    }
                }
            }
        }

        public GastoLinha Parse(char[] gastoLinha)
        {
            var separatorPosition = ParseLineWithIntArray(gastoLinha);

            _stringBuilder.Clear();

            for (int i = separatorPosition[2] + 1; i < separatorPosition[3]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.TipoGasto = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[7] + 1; i < separatorPosition[8]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Processo = long.Parse(_stringBuilder.ToString());
            _stringBuilder.Clear();

            for (int i = separatorPosition[9] + 1; i < separatorPosition[10]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Favorecido = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[10] + 1; i < separatorPosition[11]; i++)
                if (char.IsDigit(gastoLinha[i]))
                    _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.CNPJ = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[11] + 1; i < separatorPosition[12]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Poder = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[19] + 1; i < separatorPosition[20]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Categoria = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[27] + 1; i < separatorPosition[28]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Rubrica = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[31] + 1; i < separatorPosition[32]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Funcao = _stringBuilder.ToString();
            _stringBuilder.Clear();

            for (int i = separatorPosition[43] + 1; i < separatorPosition[44]; i++)
                _stringBuilder.Append(gastoLinha[i]);
            _gastoLinha.Valor = double.Parse(_stringBuilder.ToString());
            _stringBuilder.Clear();

            return _gastoLinha;
        }
    }
}