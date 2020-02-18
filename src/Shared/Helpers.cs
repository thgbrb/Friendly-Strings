using System;
using System.Diagnostics;

namespace Shared
{
    public static class SharedHelpers
    {
        public static void Summary()
        {
            Console.WriteLine($"Processor Time: {Process.GetCurrentProcess().TotalProcessorTime}");
            Console.WriteLine($"Peak Private Bytes KB: {Process.GetCurrentProcess().PeakWorkingSet64 / 1024:N0}");
            Console.WriteLine($"Private Memory KB: {Process.GetCurrentProcess().PrivateMemorySize64 / 1024:N0}");

            for (int i = 0; i < 3; i++)
                Console.WriteLine($"gen{i}: {GC.CollectionCount(i)}");

            Console.ReadLine();
        }
    }

    public struct GastoLinha
    {
        public string TipoGasto { get; set; } // 3
        public long Processo { get; set; } // 8
        public string Favorecido { get; set; }  //10
        public string CNPJ { get; set; }  //11
        public string Poder { get; set; } // 12
        public string Categoria { get; set; } // 20
        public string Rubrica { get; set; } // 28
        public string Funcao { get; set; } // 32
        public double Valor { get; set; } // 44

        public GastoLinha(string tipoGasto, long processo, string favorecido, string cNPJ, string poder, string categoria, string rubrica, string funcao, double valor)
        {
            TipoGasto = tipoGasto;
            Processo = processo;
            Favorecido = favorecido;
            CNPJ = cNPJ;
            Poder = poder;
            Categoria = categoria;
            Rubrica = rubrica;
            Funcao = funcao;
            Valor = valor;
        }
    }
}
