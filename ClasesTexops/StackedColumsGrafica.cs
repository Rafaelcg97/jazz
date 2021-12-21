using LiveCharts;
using LiveCharts.Wpf;

namespace ClasesTexops
{
    public class StackedColumsGrafica
    {

        public SeriesCollection StackedChart(string sn1, string sn2, string sn3, double[] v1, double[] v2, double[] v3)
        {
            return
            new SeriesCollection()
            {
                new StackedColumnSeries
                {
                    Title = sn1,
                    Values = new ChartValues<double>(v1),
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },

                new StackedColumnSeries
                {
                    Title= sn2,
                    Values = new ChartValues<double>(v2),
                    StackMode = StackMode.Values,
                    DataLabels = true
                },

                new StackedColumnSeries
                {
                    Title= sn3,
                    Values = new ChartValues<double>(v3),
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };
        }
    }
}
