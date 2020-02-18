using System.Buffers;
using System.Collections.Generic;

namespace Shared
{
    public static class Parsers
    {
        public static IList<int> ParseLine(string line)
        {
            var splittersPosition = new List<int>();

            for (int i = 0; i < line.Length; i++)
                if (line[i] == ';')
                    splittersPosition.Add(i);

            return splittersPosition;
        }
    }
}
