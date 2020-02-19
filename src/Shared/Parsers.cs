using System.Buffers;
using System.Collections.Generic;

namespace Shared
{
    public static class Parsers
    {
        static ArrayPool<short> _separatorsPosition = ArrayPool<short>.Shared;

        // Parsing with List of Int
        public static IList<int> ParseLineWithList(string line)
        {
            var separatorPosition = new List<int>();

            for (int i = 0; i < line.Length; i++)
                if (line[i] == ';')
                    separatorPosition.Add(i);

            return separatorPosition;
        }

        // Parsing string with short array
        public static short[] ParseLineWithIntArray(string line)
        {
            var separatorPosition = new short[Configuration.NUMBER_OF_COLUMS];
            var counter = 0;

            for (short i = 0; i < line.Length; i++)
                if (line[i] == Configuration.SEPARATOR)
                    separatorPosition[counter++] = i;

            return separatorPosition;
        }

        // Parsing string with short array
        public static short[] ParseLineWithIntArray(char[] line)
        {
            var separatorPosition = _separatorsPosition.Rent(line.Length);

            try
            {
                var counter = 0;

                for (short i = 0; i < line.Length; i++)
                    if (line[i] == Configuration.SEPARATOR)
                        separatorPosition[counter++] = i;

                return separatorPosition;
            }
            finally
            {
                _separatorsPosition.Return(separatorPosition);
            }

        }
    }
}
