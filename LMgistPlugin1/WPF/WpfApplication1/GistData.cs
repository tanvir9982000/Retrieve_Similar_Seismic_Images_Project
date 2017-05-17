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
        public string metadata;
        private int isSimilar = 0;  // 0 = no comment, 1  =  like, -1 = dislike 

        public int IsSimilar
        {
            get { return isSimilar; }
            set { isSimilar = value; }
        }
        public GistData() { }

        public GistData(string md)
        {
            metadata = md;
        }
        public GistData(double d, string fn) 
        {
            distance = d;
            filename = fn;
        }

        public GistData(double d, string fn, string md)
        {
            distance = d;
            filename = fn;
            metadata = md;
        }

        public GistData(string fn, double[] g)
        {
            filename = fn;
            for (int i = 0; i < 512; i++ )
                gistvalue.Add(g[i]);
        }
    }
}
