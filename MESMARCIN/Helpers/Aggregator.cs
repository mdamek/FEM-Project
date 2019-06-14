namespace MesMarcin
{
    public static class Aggregator
    {
        public static Grid AggregateToGlobalMatrix(Grid grid)
        {
            foreach (var actualElement in grid.Elements)
            {
                for (var i = 0; i < 4; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        grid.HG[actualElement.Id[i], actualElement.Id[j]] += actualElement.HL[i, j];
                        grid.CG[actualElement.Id[i], actualElement.Id[j]] += actualElement.CL[i, j];
                    }
                    grid.PG[actualElement.Id[i]] += actualElement.PL[i];
                }
            }
            return grid;
        }
    }
}
