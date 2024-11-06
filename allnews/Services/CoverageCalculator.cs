namespace allnews.Services
{
    public static class CoverageCalculator
    {
        public static (int OppCoverage, int CenterCoverage, int GovCoverage) CalculateCoverage(int oppCount, int centerCount, int govCount)
        {
            int totalCount = oppCount + centerCount + govCount;
            if (totalCount == 0) return (0, 0, 0);

            int oppCoverage = (int)Math.Round((double)oppCount / totalCount * 100);
            int centerCoverage = (int)Math.Round((double)centerCount / totalCount * 100);
            int govCoverage = (int)Math.Round((double)govCount / totalCount * 100);

            return (oppCoverage, centerCoverage, govCoverage);
        }
    }
}
