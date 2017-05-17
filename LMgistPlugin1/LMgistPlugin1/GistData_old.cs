using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class GistData
    {
        public string filename;
        public List<double> gistvalue = new List<double>();
        public double distance;
        public GistData() { }
        public GistData(double d, string fn) 
        {
            distance = d;
            filename = fn;
        }
        public GistData(string fn, double[] g)
        {
            filename = fn;
            for (int i = 0; i < 512; i++ )
                gistvalue.Add(g[i]);
        }
    }
}
