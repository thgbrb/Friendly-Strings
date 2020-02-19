using Shared;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Shared.Parsers;

namespace Playing
{
    public class v99_Optimized
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly ArrayPool<char> _charArrayPool = ArrayPool<char>.Shared;

        public void Run()
        {
            using (TextWriter tw = new StreamWriter(path: Configuration.DATABASE_FILE, append: false))
            {
                foreach (var despesa in LoadLines())
                    tw.Write(despesa);
            }

            SharedHelpers.Summary();
        }

        public IEnumerable<char[]> LoadLines()
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

                        yield return gastoLinha.GenerateLine();
                    }
                    finally
                    {
                        _charArrayPool.Return(buffer);
                    }
                }
            }
        }

        public GastoLinhaSpan Parse(char[] gastoLinha)
        {
            var separatorPosition = ParseLineWithIntArray(gastoLinha);
            var gastoLinhaSpan = new GastoLinhaSpan();
            var lineSpan = new Span<char>(array: gastoLinha);

            gastoLinhaSpan.TipoGasto = lineSpan
                .Slice(
                    start: separatorPosition[2] + 1,
                    length: separatorPosition[3] - separatorPosition[2] + 1);

            gastoLinhaSpan.Processo = lineSpan
                .Slice(
                    start: separatorPosition[7] + 1,
                    length: separatorPosition[8] - separatorPosition[7] + 1);

            gastoLinhaSpan.Favorecido = lineSpan
                .Slice(
                    start: separatorPosition[9] + 1,
                    length: separatorPosition[10] - separatorPosition[9] + 1);

            gastoLinhaSpan.CNPJ = lineSpan
                .Slice(
                    start: separatorPosition[10] + 1,
                    length: separatorPosition[11] - separatorPosition[10] + 1);

            gastoLinhaSpan.Poder = lineSpan
                .Slice(
                    start: separatorPosition[11] + 1,
                    length: separatorPosition[12] - separatorPosition[11] + 1);

            gastoLinhaSpan.Categoria = lineSpan
                .Slice(
                    start: separatorPosition[19] + 1,
                    length: separatorPosition[20] - separatorPosition[19] + 1);

            gastoLinhaSpan.Rubrica = lineSpan
                .Slice(
                    start: separatorPosition[27] + 1,
                    length: separatorPosition[28] - separatorPosition[27] + 1);

            gastoLinhaSpan.Funcao = lineSpan
                .Slice(
                    start: separatorPosition[31] + 1,
                    length: separatorPosition[32] - separatorPosition[31] + 1);

            gastoLinhaSpan.Funcao = lineSpan
                .Slice(
                    start: separatorPosition[43] + 1,
                    length: separatorPosition[44] - separatorPosition[43] + 1);

            return gastoLinhaSpan;
        }
    }

    public ref struct GastoLinhaSpan
    {
        public Span<char> TipoGasto { get; set; } // 3
        public Span<char> Processo { get; set; } // 8
        public Span<char> Favorecido { get; set; }  //10
        public Span<char> CNPJ { get; set; }  //11
        public Span<char> Poder { get; set; } // 12
        public Span<char> Categoria { get; set; } // 20
        public Span<char> Rubrica { get; set; } // 28
        public Span<char> Funcao { get; set; } // 32
        public Span<char> Valor { get; set; } // 44

        public GastoLinhaSpan(Span<char> tipoGasto, Span<char> processo, Span<char> favorecido, Span<char> cnpj, Span<char> poder, Span<char> categoria, Span<char> rubrica, Span<char> funcao, Span<char> valor)
        {
            TipoGasto = tipoGasto;
            Processo = processo;
            Favorecido = favorecido;
            CNPJ = cnpj;
            Poder = poder;
            Categoria = categoria;
            Rubrica = rubrica;
            Funcao = funcao;
            Valor = valor;
        }

        public char[] GenerateLine()
        {
            var bufferSize = TipoGasto.Length + Processo.Length + Favorecido.Length + CNPJ.Length + Poder.Length + Categoria.Length + Rubrica.Length + Funcao.Length + Valor.Length + 10;
            
            //preencher array
            var result = new char[bufferSize];

            for (int lastPosition = 0; lastPosition < bufferSize - 1;)
            {
                for (int j = 0; j < TipoGasto.Length; j++)
                {
                    result[lastPosition] = TipoGasto[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;

                for (int j = 0; j < Processo.Length; j++)
                {
                    result[lastPosition] = Processo[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;

                for (int j = 0; j < Favorecido.Length; j++)
                {
                    result[lastPosition] = Favorecido[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;

                for (int j = 0; j < CNPJ.Length; j++)
                {
                    result[lastPosition] = CNPJ[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;

                for (int j = 0; j < Poder.Length; j++)
                {
                    result[lastPosition] = Poder[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;

                for (int j = 0; j < Categoria.Length; j++)
                {
                    result[lastPosition] = Categoria[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;


                for (int j = 0; j < Rubrica.Length; j++)
                {
                    result[lastPosition] = Rubrica[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;


                for (int j = 0; j < Funcao.Length; j++)
                {
                    result[lastPosition] = Funcao[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;

                for (int j = 0; j < Valor.Length; j++)
                {
                    result[lastPosition] = Valor[j];
                    lastPosition++;
                }

                result[lastPosition] = ';';
                lastPosition++;
            }

            result[result.Length - 1] = '\n';

            return result;
        }
    }
}