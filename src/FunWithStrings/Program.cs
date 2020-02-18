using Shared;

namespace Playing
{
    class Program
    {
        static void Main(string[] args)
        {
            new v01_Stackoverflow().Run();
            //new v50_Optimized().Run();
            //new v90_Optimized().Run();

            SharedHelpers.Summary();
        }
    }
}