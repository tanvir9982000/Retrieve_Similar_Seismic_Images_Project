using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class KNN
    {
        // testSamples and trainSamples consists of about 20k vectors each with 25 dimensions
        // trainClasses contains 0 or 1 signifying the corresponding class for each sample in trainSamples
        public int[] TestKnnCase(List<double[]> trainSamples, List<double[]> testSamples, int[] trainClasses, int K, double alpha)
        {
            Console.WriteLine("Performing KNN with K = " + K);

            var testResults = new int[testSamples.Count()];

            var testNumber = testSamples.Count();
            var trainNumber = trainSamples.Count();
            // Declaring these here so that I don't have to 'new' them over and over again in the main loop, 
            // just to save some overhead
            var distances = new double[trainNumber][];
            for (var i = 0; i < trainNumber; i++)
            {
                distances[i] = new double[2]; // Will store both distance and index in here
            }

            // Performing KNN ...
            for (var tst = 0; tst < testNumber; tst++)
            {
                // For every test sample, calculate distance from every training sample
                Parallel.For(0, trainNumber, trn =>
                {
                    var dist = GetDistance(testSamples[tst], trainSamples[trn]);  
                    // Storing distance as well as index 
                    distances[trn][0] = dist;
                    distances[trn][1] = trn;
                });

                // Sort distances and take top K (?What happens in case of multiple points at the same distance?)
                var votingDistances = distances.AsParallel().OrderBy(t => t[0]).Take(K);

                // Do a 'majority vote' to classify test sample
                var yea = 0.0;
                var nay = 0.0;

                foreach (var voter in votingDistances)
                {

                    if (trainClasses[(int)voter[1]] == 1) 
                        yea++;
                    else
                        nay++;
                }
                if (yea > nay)
                    testResults[tst] = 1;
                else
                    testResults[tst] = 0;

            }

            return testResults;
        }


        public List<GistData> TestKnnCase(List<GistData> trainSamples, List<GistData> testSamples, int K, double alpha)
        {
            Console.WriteLine("Performing KNN with K = " + K);

            //var testResults = new int[testSamples.Count()];

            var testNumber = testSamples.Count();
            var trainNumber = trainSamples.Count();
            // Declaring these here so that I don't have to 'new' them over and over again in the main loop, 
            // just to save some overhead
            var distances = new double[trainNumber][];
            for (var i = 0; i < trainNumber; i++)
            {
                distances[i] = new double[2]; // Will store both distance and index in here
            }

            // Performing KNN ...
            for (var tst = 0; tst < testNumber; tst++)
            {
                // For every test sample, calculate distance from every training sample
                Parallel.For(0, trainNumber, trn =>
                {
                    var dist = GetDistance(testSamples[tst], trainSamples[trn], alpha);
                    // Storing distance as well as index 
                    distances[trn][0] = dist;
                    distances[trn][1] = trn;
                });

                // Sort distances and take top K (?What happens in case of multiple points at the same distance?)
                var votingDistances = distances.AsParallel().OrderBy(t => t[0]).Take(K);

                // Do a 'majority vote' to classify test sample
                var yea = 0.0;
                var nay = 0.0;

                foreach (var voter in votingDistances)
                {
                    if (trainSamples[(int)voter[1]].IsSimilar == 1)
                        yea++;
                    else
                        nay++;
                }
                if (yea > nay)
                    testSamples[tst].IsSimilar = 1;
                else
                    testSamples[tst].IsSimilar = -1;

            }

            return testSamples;
        }

        // Calculates and returns square of Euclidean distance between two vectors
        static double GetDistance(IList<double> sample1, IList<double> sample2)
        {
            var distance = 0.0;
            // assume sample1 and sample2 are valid i.e. same length 

            for (var i = 0; i < sample1.Count; i++)
            {
                var temp = sample1[i] - sample2[i];
                distance += temp * temp;
            }
            return distance;
        }

        public double GetDistance(GistData sample1, GistData sample2, double alpha)
        {
            var distance = 0.0;
            // assume sample1 and sample2 are valid i.e. same length 

            //for (var i = 0; i < sample1.Count; i++)
            {
                var tempD = sample1.distance - sample2.distance;
                var tempH = GetEditDistance(sample1.metadata, sample2.metadata);
                distance += alpha * tempD * tempD + (1 - alpha) * tempH;

                Console.WriteLine(sample1.filename + "-" + sample2.filename + " tempD: " + tempD + " tempH: " + tempH + " distance :" + distance);
            }
            return distance;
        }


        public double GetEditDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            int maxLength = Math.Max(n, m);
            double normalizedEditDistance = 0;
            
            // Step 1
            if (n == 0 && m == 0)
            {
                normalizedEditDistance = 0;
                Console.WriteLine(s + " " + t + " maxLength: " + maxLength + " NED: " + normalizedEditDistance);
                return normalizedEditDistance;
            }
            
            if (n == 0)
            {
                normalizedEditDistance = (double)m / maxLength;
                Console.WriteLine(s + " " + t + " maxLength: " + maxLength + " NED: " + normalizedEditDistance);
                return normalizedEditDistance;
            }

            if (m == 0)
            {
                normalizedEditDistance =  (double)n / maxLength;
                Console.WriteLine(s + " " + t + " maxLength: " + maxLength + " NED: " + normalizedEditDistance);
                return normalizedEditDistance;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7

            normalizedEditDistance = (double)d[n, m]/maxLength;
            Console.WriteLine(s + " : " + n + " : "+ t + m + " maxLength: " + maxLength + " NED: " + normalizedEditDistance);
            return normalizedEditDistance;
        }




    }
}
