using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrajectoryAnalyzer
{
    class Utility
    {
        public static List<double> GetDataFromFile(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            string[] lines = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var data = new List<double>(lines.Length);
            foreach (var line in lines)
            {
                data.Add(double.Parse(line));
            }
            return data;
        }

        public static List<double> Correlation(List<double> first, List<double> second)
        {
            int length = Math.Min(first.Count, second.Count);
            List<double> correlation = new List<double>(length);
            List<double> x = first.GetRange(0, length);
            List<double> y = second.GetRange(0, length);

            double xMean = x.Average();
            double yMean = y.Average();

            for (int i = 0; i < length; i++)
            {
                double n = 0;
                double d1 = 0;
                double d2 = 0;

                for (int j = 0; j < length; j++)
                {
                    double xim = (j + i >= x.Count ? 0 : x[j + i] - xMean);
                    double yim = y[j] - yMean;
                    n += xim * yim;
                    d1 += xim * xim;
                    d2 += yim * yim;
                }

                correlation.Add(n / (Math.Sqrt(d1 * d2)));
            }
            return correlation;
        }
    }
}
