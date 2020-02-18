using Shared;

namespace Playing
{
    class Program
    {
        static void Main(string[] args)
        {
            new Stackoverflow().Run();
            //new Optimized().Run();

            SharedHelpers.Summary();
        }
    }
}