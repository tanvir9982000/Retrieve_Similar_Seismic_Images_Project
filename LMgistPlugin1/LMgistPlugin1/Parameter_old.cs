using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMgistPlugin1
{
    public class Parameter
    {
        private int imageWidth;
        private int imageHeight;
        private int[] orientationsPerScale = new int[4];
        private int numberBlocks;
        private int fc_prefilt;
        private int boundaryExtension;
        private double[][][] g;
        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; }
        }
        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }
        public int[] OrientationsPerScale
        {
            get { return orientationsPerScale; }
            set { orientationsPerScale = value; }
        }
        public int NumberBlocks
        {
            get { return numberBlocks; }
            set { numberBlocks = value; }
        }
        public int Fc_prefilt
        {
            get { return fc_prefilt; }
            set { fc_prefilt = value; }
        }
        public int BoundaryExtension
        {
            get { return boundaryExtension; }
            set { boundaryExtension = value; }
        }
        public double[][][] G
        {
            get { return g; }
            set { g = value; }
        }
        public Parameter()
        {
            this.ImageWidth = 256;
            this.ImageHeight = 256;
            int[] ops = new int[4];
            for (int i = 0; i < 4; i++)
                ops[i] = 8;
            this.OrientationsPerScale = ops;
            this.NumberBlocks = 4;
            this.Fc_prefilt = 4;
            this.BoundaryExtension = 32;
            double[][][] gval = new double[this.ImageHeight + 2 * this.BoundaryExtension][][];
            for (int i = 0; i < this.imageHeight; i++)
            {
                gval[i] = new double[this.imageWidth][];
                for (int j = 0; j < this.imageWidth; j++)
                {
                    gval[i][j] = new double[OrientationsPerScale.Sum()];
                }
            }
            this.G = gval;
        }

    };
}
