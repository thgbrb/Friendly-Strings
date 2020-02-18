using System.Collections.Generic;

namespace Shared
{
    public static class Parsers
    {
        // Parsing with List of Int
        public static IList<int> ParseLineWithList(string line)
        {
            var separatorPosition = new List<int>();

            for (int i = 0; i < line.Length; i++)
                if (line[i] == ';')
                    separatorPosition.Add(i);

            return separatorPosition;
        }

        // Parsing with short array
        public static short[] ParseLineWithIntArray(string line)
        {
            var separatorPosition = new short[Configuration.NUMBER_OF_COLUMS];
            var counter = 0;

            for (short i = 0; i < line.Length; i++)
                if (line[i] == Configuration.SEPARATOR)
                    separatorPosition[counter++] = i;

            return separatorPosition;
        }
    }
}
