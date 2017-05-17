using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AForge.Math;
using System.Numerics;

namespace Tester1
{
    public class Padarray
    {

        public void swap(ref double[] a, ref double[] b, int size)
        {
            double[] temp = new double[size];
            a.CopyTo(temp, 0);
            a = b;
            b = temp;

        }
        public double[][] ProcessPadArray(double[][] A, int heightA, int widthA, int padsize = 5)
        {


            int paddedWidthB = widthA + 2 * padsize;
            int paddedHeightB = heightA + 2 * padsize;

            int[,] dimNumWidth = new int[heightA, 2 * widthA];
            int[] dimNumHeight = new int[2 * heightA];
            double[][] b = new double[paddedHeightB][];

            for (int i = 0; i < paddedHeightB; i++)
            {
                b[i] = new double[paddedWidthB];
            }

            //Initialize dimNumHeight
            for (int i = 0; i < heightA; i++)
            {
                dimNumHeight[i] = i+1;
            }
            for (int i = heightA; i < 2 * heightA; i++)
            {
                dimNumHeight[i] = 2*heightA - i;
            }

            //Initialize dimNumWidth
            for (int row = 0; row < heightA; row++)
            {
                for (int i = 0; i < widthA; i++)
                {
                    dimNumWidth[row, i] = i + 1;
                }
                for (int i = widthA; i < 2 * widthA; i++)
                {
                    dimNumWidth[row, i] = 2 * widthA - i ;
                }
            }

            
            Console.WriteLine("Printing dimNumHeight :");
            for (int i = 0; i < 2 * heightA; i++)
            {
                Console.Write(dimNumHeight[i] + "\t");
            }
            Console.WriteLine();

            Console.WriteLine("Printing dimNumWidth :");
            //this.Print2DArray(dimNumWidth, heightA, 2 * widthA);
            Console.WriteLine();


            Console.WriteLine("Printing array a :");
            this.Print2DArray(A, heightA, widthA);
            

            //Saving roworder for a single row
            int[,] roworder = new int[2, paddedWidthB];


            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    int index = 0;
                    for (int k = -padsize; k <= (heightA + padsize - 1); k++)
                    {
                        int idx = ((k + (2 * heightA)) % (2 * heightA));
                        roworder[0, index++] = dimNumHeight[idx] - 1;
                        //Console.Write(roworder[0, index - 1] + " ");
                        if (index > paddedHeightB) break;
                    }
                    //Console.WriteLine();
                }

                if (i == 1)
                {
                    int index = 0;
                    for (int k = -padsize; k <= (widthA + padsize - 1); k++)
                    {
                        int idx = ((k + (2 * widthA)) % (2 * widthA));
                        roworder[1, index++] = dimNumWidth[0, idx] - 1;
                        //Console.Write(roworder[1, index - 1] + " ");
                        if (index > paddedWidthB) break;
                    }
                    //Console.WriteLine();
                }


            }


            //Console.WriteLine("Saving padarray: ");
            for (int i = 0; i < paddedHeightB; i++)
            {
                for (int j = 0; j < paddedWidthB; j++)
                {
                    b[i][j] = A[roworder[0, i]][roworder[1, j]];
                }
            }

            return b;

        }
        public double[][] ProcessPadArray(double[][] A, int heightA, int widthA, int padsizeH, int padsizeW)
        {


            int paddedWidthB = widthA + 2 * padsizeW;
            int paddedHeightB = heightA + 2 * padsizeH;

            int[,] dimNumWidth = new int[heightA, 2 * widthA];
            int[] dimNumHeight = new int[2 * heightA];
            double[][] b = new double[paddedHeightB][];

            for (int i = 0; i < paddedHeightB; i++)
            {
                b[i] = new double[paddedWidthB];
            }

            //Initialize dimNumHeight
            for (int i = 0; i < heightA; i++)
            {
                dimNumHeight[i] = i + 1;
            }
            for (int i = heightA; i < 2 * heightA; i++)
            {
                dimNumHeight[i] = 2 * heightA - i;
            }

            //Initialize dimNumWidth
            for (int row = 0; row < heightA; row++)
            {
                for (int i = 0; i < widthA; i++)
                {
                    dimNumWidth[row, i] = i + 1;
                }
                for (int i = widthA; i < 2 * widthA; i++)
                {
                    dimNumWidth[row, i] = 2 * widthA - i;
                }
            }


            Console.WriteLine("Printing dimNumHeight :");
            for (int i = 0; i < 2 * heightA; i++)
            {
                Console.Write(dimNumHeight[i] + "\t");
            }
            Console.WriteLine();

            Console.WriteLine("Printing dimNumWidth :");
            //this.Print2DArray(dimNumWidth, heightA, 2 * widthA);
            Console.WriteLine();


            Console.WriteLine("Printing array a :");
            this.Print2DArray(A, heightA, widthA);


            //Saving roworder for a single row
            int[,] roworder = new int[2, paddedWidthB];


            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    int index = 0;
                    for (int k = -padsizeH; k <= (heightA + padsizeH - 1); k++)
                    {
                        int idx = ((k + (2 * heightA)) % (2 * heightA));
                        roworder[0, index++] = dimNumHeight[idx] - 1;
                        //Console.Write(roworder[0, index - 1] + " ");
                        if (index > paddedHeightB) break;
                    }
                    //Console.WriteLine();
                }

                if (i == 1)
                {
                    int index = 0;
                    for (int k = -padsizeW; k <= (widthA + padsizeW - 1); k++)
                    {
                        int idx = ((k + (2 * widthA)) % (2 * widthA));
                        roworder[1, index++] = dimNumWidth[0, idx] - 1;
                        //Console.Write(roworder[1, index - 1] + " ");
                        if (index > paddedWidthB) break;
                    }
                    //Console.WriteLine();
                }


            }


            //Console.WriteLine("Saving padarray: ");
            for (int i = 0; i < paddedHeightB; i++)
            {
                for (int j = 0; j < paddedWidthB; j++)
                {
                    b[i][j] = A[roworder[0, i]][roworder[1, j]];
                }
            }

            return b;

        }
        public double[][] ProcessPadArray(double[][] A, int heightA, int widthA, int padsizeH, int padsizeW, string direction = "both")
        {


            int paddedWidthB = widthA + 2 * padsizeW;
            int paddedHeightB = heightA + 2 * padsizeH;

            int[,] dimNumWidth = new int[heightA, 2 * widthA];
            int[] dimNumHeight = new int[2 * heightA];
            double[][] b = new double[paddedHeightB][];

            for (int i = 0; i < paddedHeightB; i++)
            {
                b[i] = new double[paddedWidthB];
            }

            //Initialize dimNumHeight
            for (int i = 0; i < heightA; i++)
            {
                dimNumHeight[i] = i + 1;
            }
            for (int i = heightA; i < 2 * heightA; i++)
            {
                dimNumHeight[i] = 2 * heightA - i;
            }

            //Initialize dimNumWidth
            for (int row = 0; row < heightA; row++)
            {
                for (int i = 0; i < widthA; i++)
                {
                    dimNumWidth[row, i] = i + 1;
                }
                for (int i = widthA; i < 2 * widthA; i++)
                {
                    dimNumWidth[row, i] = 2 * widthA - i;
                }
            }

            /*
            Console.WriteLine("Printing dimNumHeight :");
            for (int i = 0; i < 2 * heightA; i++)
            {
                Console.Write(dimNumHeight[i] + "\t");
            }
            Console.WriteLine();

            Console.WriteLine("Printing dimNumWidth :");
            //this.Print2DArray(dimNumWidth, heightA, 2 * widthA);
            Console.WriteLine();


            Console.WriteLine("Printing array a :");
            this.Print2DArray(A, heightA, widthA);
            */

            //Saving roworder for a single row
            int[,] roworder = new int[2, paddedWidthB];


            if (direction.Equals("pre", StringComparison.Ordinal))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        int index = 0;
                        for (int k = -padsizeH; k <= (heightA - 1); k++)
                        {
                            int idx = ((k + (2 * heightA)) % (2 * heightA));
                            roworder[0, index++] = dimNumHeight[idx] - 1;
                            //Console.Write(roworder[0, index - 1] + " ");
                            if (index > paddedHeightB) break;
                        }
                        //Console.WriteLine();
                    }

                    if (i == 1)
                    {
                        int index = 0;
                        for (int k = -padsizeW; k <= (widthA - 1); k++)
                        {
                            int idx = ((k + (2 * widthA)) % (2 * widthA));
                            roworder[1, index++] = dimNumWidth[0, idx] - 1;
                            //Console.Write(roworder[1, index - 1] + " ");
                            if (index > paddedWidthB) break;
                        }
                        //Console.WriteLine();
                    }


                }
            }



            else if (direction.Equals("post", StringComparison.Ordinal))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        int index = 0;
                        for (int k = 0; k <= (heightA + padsizeH - 1); k++)
                        {
                            int idx = ((k + (2 * heightA)) % (2 * heightA));
                            roworder[0, index++] = dimNumHeight[idx] - 1;
                            //Console.Write(roworder[0, index - 1] + " ");
                            if (index > paddedHeightB) break;
                        }
                        //Console.WriteLine();
                    }

                    if (i == 1)
                    {
                        int index = 0;
                        for (int k = 0; k <= (widthA + padsizeW - 1); k++)
                        {
                            int idx = ((k + (2 * widthA)) % (2 * widthA));
                            roworder[1, index++] = dimNumWidth[0, idx] - 1;
                            //Console.Write(roworder[1, index - 1] + " ");
                            if (index > paddedWidthB) break;
                        }
                        //Console.WriteLine();
                    }


                }
            }
            else if (direction.Equals("both", StringComparison.Ordinal))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        int index = 0;
                        for (int k = -padsizeH; k <= (heightA + padsizeH - 1); k++)
                        {
                            int idx = ((k + (2 * heightA)) % (2 * heightA));
                            roworder[0, index++] = dimNumHeight[idx] - 1;
                            //Console.Write(roworder[0, index - 1] + " ");
                            if (index > paddedHeightB) break;
                        }
                        //Console.WriteLine();
                    }

                    if (i == 1)
                    {
                        int index = 0;
                        for (int k = -padsizeW; k <= (widthA + padsizeW - 1); k++)
                        {
                            int idx = ((k + (2 * widthA)) % (2 * widthA));
                            roworder[1, index++] = dimNumWidth[0, idx] - 1;
                            //Console.Write(roworder[1, index - 1] + " ");
                            if (index > paddedWidthB) break;
                        }
                        //Console.WriteLine();
                    }


                }

            }

            //Console.WriteLine("Saving padarray: ");
            for (int i = 0; i < paddedHeightB; i++)
            {
                for (int j = 0; j < paddedWidthB; j++)
                {
                    b[i][j] = A[roworder[0, i]][roworder[1, j]];
                }
            }

            /*
            Console.Write("Printing padded array:\n");
            Print2DArray(b, paddedHeightB, paddedWidthB);
            Console.Write("\n");*/

            return b;

        }
        public void Print2DArray(double[][] B, int heightB, int widthB)
        {
            for (int i = 0; i < heightB; i++)
            {
                for (int j = 0; j < widthB; j++)
                {
                    //Console.Write(B[i][j].ToString() + "\t");
                    Console.Write("{0:F4}\t", B[i][j]);
                }
                Console.WriteLine();
            }
        }
        public void Print2DArray(int[][] B, int heightB, int widthB)
        {
            for (int i = 0; i < heightB; i++)
            {
                for (int j = 0; j < widthB; j++)
                {
                    Console.Write(B[i][j].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }
        public void Print2DArray(Complex[,] B, int heightB, int widthB)
        {
            for (int i = 0; i < heightB; i++)
            {
                for (int j = 0; j < widthB; j++)
                {
                    //Console.Write(B[i][j].ToString() + "\t");
                    Console.Write("{0:F4}\t", B[i,j]);
                }
                Console.WriteLine();
            }
        }
        public Complex[,] fft2(double[][] img, int height, int width)
        {
            Complex[,] data = new Complex[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex tmp = new Complex(img[i][j], 0);
                    data[i, j] = tmp;
                }
            }

            // process columns
            Complex[] col = new Complex[height];

            for (int j = 0; j < width; j++)
            {
                // copy column
                for (int i = 0; i < height; i++)
                    col[i] = data[i, j];
                // transform it
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(col);
                // copy back
                for (int i = 0; i < height; i++)
                    data[i, j] = col[i];
            }


            // process rows
            Complex[] row = new Complex[width];

            for (int i = 0; i < height; i++)
            {
                // copy row
                for (int j = 0; j < width; j++)
                    row[j] = data[i, j];
                // transform it
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(row);

                //FourierTransform.FFT( row, direction );
                // copy back
                for (int j = 0; j < width; j++)
                    data[i, j] = row[j];
            }

            //Console.Write("Printing after img\n");
            //Print2DArray(img, height, width);

            //Console.Write("Printing after fft2(img)\n");
            //Print2DArray(data, height, width);

            return data;

 
        }
        public Complex[,] ifft2(double[][] img, int height, int width)
        {

            Complex[,] data = new Complex[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex tmp = new Complex(img[i][j], 0);
                    data[i, j] = tmp;
                }
            }


            // process columns
            Complex[] colb = new Complex[height];

            for (int j = 0; j < width; j++)
            {
                // copy column
                for (int i = 0; i < height; i++)
                    colb[i] = data[i, j];
                // transform it
                MathNet.Numerics.IntegralTransforms.Fourier.Inverse(colb);
                // copy back
                for (int i = 0; i < height; i++)
                    data[i, j] = colb[i];
            }

            // process rows
            Complex[] rowb = new Complex[width];

            for (int i = 0; i < height; i++)
            {
                // copy row
                for (int j = 0; j < width; j++)
                    rowb[j] = data[i, j];
                // transform it
                MathNet.Numerics.IntegralTransforms.Fourier.Inverse(rowb);

                //FourierTransform.FFT( row, direction );
                // copy back
                for (int j = 0; j < width; j++)
                    data[i, j] = rowb[j];
            }


            return data;
        }
        public Complex[,] ifft2(Complex[,] data, int height, int width)
        {

            // process columns
            Complex[] colb = new Complex[height];

            for (int j = 0; j < width; j++)
            {
                // copy column
                for (int i = 0; i < height; i++)
                    colb[i] = data[i, j];
                // transform it
                MathNet.Numerics.IntegralTransforms.Fourier.Inverse(colb);
                // copy back
                for (int i = 0; i < height; i++)
                    data[i, j] = colb[i];
            }

            // process rows
            Complex[] rowb = new Complex[width];

            for (int i = 0; i < height; i++)
            {
                // copy row
                for (int j = 0; j < width; j++)
                    rowb[j] = data[i, j];
                // transform it
                MathNet.Numerics.IntegralTransforms.Fourier.Inverse(rowb);

                //FourierTransform.FFT( row, direction );
                // copy back
                for (int j = 0; j < width; j++)
                    data[i, j] = rowb[j];
            }


            return data;
        }
        public double[][] TestPrefilt()
        {
            int bitmapImageWidth = 2;
            int bitmapImageHeight = 2;
            double[][] a = new double[bitmapImageHeight][];
            a[0] = new double[] { .1, .2};
            a[1] = new double[] { .3, .4};
            //a[2] = new double[] { .9, 1.0, 1.1, 1.2 };
            //a[3] = new double[] { 1.3, 1.4, 1.5, 1.6 };

            int w = 1;
            int fc = 4;
            double s1 = fc / Math.Sqrt(Math.Log(2.0));


            for (int i = 0; i < bitmapImageWidth; i++)
            {
                for (int j = 0; j < bitmapImageHeight; j++)
                {
                    a[i][j] = Math.Log(a[i][j] + 1);
                }
            }

            int sw = bitmapImageWidth + 2 * w;
            int sh = bitmapImageHeight + 2 * w;
            double[][] b = new double[sh][];
            for (int i = 0; i < sh; i++)
            {
                b[i] = new double[sw];
            }
            b = ProcessPadArray(a, bitmapImageHeight, bitmapImageWidth, w, w, "both");

            Console.Write("Printing b after first pad\n");
            Print2DArray(b, sw, sh);
            Console.WriteLine();

            int c = 1;
            int N = 1;
            int n = Math.Max(sw, sh);
            n = n + (n % 2);

            double[][] img = new double[sh + n - sw][];
            for (int i = 0; i < sh; i++)
            {
                img[i] = new double[sw + n - sh];
            }

            img = ProcessPadArray(b, sh, sw, n - sw, n - sh, "post");



            MeshGridContainer cont = new MeshGridContainer(); 
            int meshgridstart = -(n / 2);
            int meshgridend = (n / 2) - 1;
            cont = meshgrid(meshgridstart, meshgridend);

            int meshgridheight = meshgridend - meshgridstart + 1;
            int meshgridwidth = meshgridend - meshgridstart + 1;

            double[][] gf = new double[meshgridheight][];
            for (int i = 0; i < meshgridheight; i++)
            {
                gf[i] = new double[meshgridwidth];
            }
            gf = fftshiftPrefilt(cont.container1_row, cont.container1_col, cont.container1, cont.container2, s1);
            Print2DArray(gf, meshgridheight, meshgridwidth);

            //skipping repmat()


            Complex[,] data = new Complex[sh, sw];

            data = fft2(img, sh, sw);
            
            //data = ifft2(data, sm, sn);

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    data[i, j] = data[i, j] * gf[i][j];
                }
            }
            data = ifft2(data, sh, sw);

            double[][] output = new double[sh][];
            for (int i = 0; i < sh; i++)
            {
                output[i] = new double[sw];
            }

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    output[i][j] = img[i][j] - data[i, j].Real;
                }
            }

            Console.Write("Priniting after whitening\n");
            Print2DArray(output, sh, sw);

            //end of whitening

             //Local contrast normalization

            double[][] outputSquare = new double[sh][];
            for (int i = 0; i < sh; i++)
            {
                outputSquare[i] = new double[sw];
            }

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    outputSquare[i][j] = Math.Pow(output[i][j], 2);
                }
            }

            data = fft2(outputSquare, sh, sw);
            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    data[i,j] = data[i,j] * gf[i][j];
                }
            }

            data = ifft2(data, sh, sw);

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    outputSquare[i][j] = Math.Sqrt(Math.Abs(data[i, j].Real));
                    outputSquare[i][j] = .2 + outputSquare[i][j];
                    output[i][j] = output[i][j] / outputSquare[i][j];
                }
            }

            Console.Write("Priniting after Local contrast normalization\n");
            Print2DArray(output, sh, sw);

            //Crop output to have same size than the input

            double[][] prefiltOut = new double[bitmapImageHeight][];
            for (int i = 0; i < bitmapImageHeight; i++)
            {
                prefiltOut[i] = new double[bitmapImageWidth];
            }

            for (int i = 0; i < bitmapImageHeight; i++)
            {
                for (int j = 0; j < bitmapImageWidth; j++)
                {
                    prefiltOut[i][j]= output[i+w][j+w];
                }
            }

            Console.Write("Priniting after croping\n");
            Print2DArray(prefiltOut, bitmapImageHeight, bitmapImageWidth);

            return prefiltOut;


        }
        public MeshGridContainer meshgrid(int start, int end)
        {
            int meshgridheight = end - start + 1;
            int meshgridwidth = end - start + 1;
            int[][] fx = new int[meshgridheight][];
            int[][] fy = new int[meshgridheight][];
            for (int i = 0; i < meshgridheight; i++)
            {
                fx[i] = new int[meshgridwidth];
                fy[i] = new int[meshgridwidth];
            }

            int row, col ;
            row = start;
            for (int i = 0; i < meshgridheight; i++, row++)
            {
                col = start;
                for (int j = 0; j < meshgridwidth ; j++, col++)
                {
                    fx[i][j] = col;
                    fy[i][j] = row;
                }
            }
            MeshGridContainer cont = new MeshGridContainer(end - start + 1, end - start + 1, end - start + 1, end - start + 1);
            cont.container1 = fx;
            cont.container2 = fy;

            return cont;
        }
        public void circshift( ref double[][] src, int xdim, int ydim, int yshift, int xshift)
        {
            double[][] temp = new double[ydim][];
            for (int i = 0; i < ydim; i++)
            {
                temp[i] = new double[xdim];
                temp[i] = (double[])src[i].Clone(); // 1D Clone is deep
            }
            //temp = (double[][])src.Clone();  // 2D Clone is shallow

            for (int i = 0; i < ydim ; i++)
            {
                src[(i + yshift) % ydim] = (double[])temp[i].Clone();  // 1D Clone is deep
            }

            for (int i = 0; i < ydim; i++)
            {
                temp[i] = (double[])src[i].Clone();
            }

            //temp = (double[][])src.Clone();  // 2D Clone is shallow

            for (int col = 0; col < xdim; col++)
            {
                for (int row = 0; row < ydim; row++)
                {
                    //Array.Copy(temp[row], col, src[row], (col + xshift) % xdim, 1);
                    src[row][(col + xshift) % xdim] = temp[row][col];
                }
            }
        }
        public double[][] fftshiftPrefilt(int row, int col, int[][] fx, int[][] fy, double scalar)
        {
            double[][] out1 = new double[row][];
            for (int i = 0; i < row; i++)
            {
                out1[i] = new double[col];
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    out1[i][j] = Math.Exp(-((Math.Pow(fx[i][j], 2) + Math.Pow(fy[i][j], 2)) / Math.Pow(scalar, 2)));
                }
            }

            int yshift = (int)Math.Floor(row/2.0);
            int xshift = (int)Math.Floor(col/2.0);
            circshift(ref out1, col, row, yshift , xshift);

            return out1;
        }
        public double[][] fftshiftCreateGabor(int row, int col, int[][] fx, int[][] fy)
        {
            double[][] out1 = new double[row][];
            for (int i = 0; i < row; i++)
            {
                out1[i] = new double[col];
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    out1[i][j] = Math.Sqrt((Math.Pow(fx[i][j], 2) + Math.Pow(fy[i][j], 2)));
                }
            }



            int yshift = (int)Math.Floor(row / 2.0);
            int xshift = (int)Math.Floor(col / 2.0);
            circshift(ref out1, col, row, yshift, xshift);

            return out1;
        }
        public double[][] fftshiftCreateGaborAngle(int row, int col, int[][] fx, int[][] fy)
        {
            double[][] out1 = new double[row][];
            for (int i = 0; i < row; i++)
            {
                out1[i] = new double[col];
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    out1[i][j] = Math.Atan2(fy[i][j], fx[i][j]);
                }
            }

            int yshift = (int)Math.Floor(row / 2.0);
            int xshift = (int)Math.Floor(col / 2.0);
            circshift(ref out1, col, row, yshift, xshift);

            return out1;
        }
        public int isGreaterThanPI(double a)
        {
            if (a > Math.PI) return 1;
            else return 0;
        }
        public int isSmallerThanNPI(double a)
        {
            if (a < -Math.PI) return 1;
            else return 0;
        }
        public double[][][] createGabor(int[] or, int nn)
        {
            //or=param.OrientationsPerScale, n=param.ImageSize[0] + 2 * param.BoundaryExtension
            int Nscales = or.Length;    //Nscales = 4
            int Nfilters = or.Sum();    //Nfilters = 32
            int[] n = new int[2];
            n[0] = nn;
            n[1] = nn;

            double[][] param = new double[Nfilters][];
            for(int i=0;i<Nfilters;i++)
            {
                param[i]= new double[Nscales];
            }

            int l=0;
            for (int i = 0; i < Nscales; i++)       //Nscales = 4
            {
                for(int j = 0; j < or[i]; j++)
                {
                    
                    param[l][0] = .35;
                    param[l][1] = .3/Math.Pow(1.85,(i));
                    param[l][2] = 16 * Math.Pow(or[i],2)/(32*32);
                    param[l][3]= Math.PI/(or[i])*(j);
                    l = l + 1;
                }

            }

            //frequencies
            MeshGridContainer cont = new MeshGridContainer(); 
            int meshgridstart = -(n[0] / 2);
            int meshgridend = (n[0] / 2) - 1;
            cont = meshgrid(meshgridstart, meshgridend);

            int meshgridheight = meshgridend - meshgridstart + 1;
            int meshgridwidth = meshgridend - meshgridstart + 1;

            double[][] fr = new double[meshgridheight][];
            for (int i = 0; i < meshgridheight; i++)
            {
                fr[i] = new double[meshgridwidth];
            }

            fr = fftshiftCreateGabor(meshgridwidth,meshgridheight, cont.container1, cont.container2);

            double[][] t = new double[meshgridheight][];
            for (int i = 0; i < meshgridheight; i++)
            {
                t[i] = new double[meshgridwidth];
            }

            t = fftshiftCreateGaborAngle(meshgridwidth, meshgridheight, cont.container1, cont.container2);


            // Transfer functions:

            //Allocate G();
            double[][][] G = new double[n[0]][][];
            for (int i = 0; i < n[0]; i++)
            {
                G[i] = new double[n[0]][];
                for (int j = 0; j < n[0]; j++)
                {
                    G[i][j] = new double[Nfilters];
                }
            }

            //Init G with 0
            for (int i = 0; i < n[0]; i++)
            {
                for (int j = 0; j < n[0]; j++)
                {
                    for (int k = 0; k < Nfilters; k++)
                    {
                        G[i][j][k] = 0;
                    }
                }
            }
            

            double[][] tr = new double[meshgridheight][];
            for (int i = 0; i < meshgridheight; i++)
            {
                tr[i] = new double[meshgridwidth];
            }

            for (int i = 0; i < Nfilters; i++)
            {
                ArrayAddParami( ref tr, t, param[i][3], meshgridheight, meshgridwidth);
                ArrayAddtr(ref tr, meshgridheight, meshgridwidth);
                ArrayAddG(ref G, tr, fr, param, i, n[1], meshgridheight, meshgridwidth);
            }
            
            //for(int j=0;j<meshgridheight;j++)
            //{
            //    for(int k=0;k<meshgridwidth;k++)
            //    {
            //        for (int i=0; i<Nfilters;i++)
            //        {
            //            tr[j][k] = t[j][k] + param[i][3];
            //            //tr[j][k] = tr[j][k] + 2 * Math.PI *isSmallerThanNPI(tr[j][k]) - 2 * Math.PI * isGreaterThanPI(tr[j][k]);

            //            //G[j][k][i] = Math.Exp(-10 * param[i][0] * Math.Pow((fr[j][k]/n[1]/param[i][1]-1),2) - 2 * param[i][2] * Math.PI * Math.Pow(tr[j][k],2)    );
            //        }
            //    }
            //}

            //Console.Write("{0} ", G[191][191][31]);

            return G;

        }
        public void ArrayAddParami(ref double[][] tr, double[][] t, double parami, int meshgridheight, int meshgridwidth)
        {
            for (int j = 0; j < meshgridheight; j++)
            {
                for (int k = 0; k < meshgridwidth; k++)
                {
                    tr[j][k] = t[j][k] + parami;
                }
            }
        }
        public void ArrayAddtr(ref double[][] tr, int meshgridheight, int meshgridwidth)
        {
            for (int j = 0; j < meshgridheight; j++)
            {
                for (int k = 0; k < meshgridwidth; k++)
                {
                    tr[j][k] = tr[j][k] + 2 * Math.PI * isSmallerThanNPI(tr[j][k]) - 2 * Math.PI * isGreaterThanPI(tr[j][k]);
                }
            }
        }
        public void ArrayAddG(ref double[][][] G, double[][] tr, double[][] fr, double[][] param, int i,int n_1 , int meshgridheight, int meshgridwidth)
        {

            for (int j = 0; j < meshgridheight; j++)
            {
                for (int k = 0; k < meshgridwidth; k++)
                {
                    G[j][k][i] = Math.Exp(-10 * param[i][0] * Math.Pow((fr[j][k] / n_1 / param[i][1] - 1), 2) - 2 * param[i][2] * Math.PI * Math.Pow(tr[j][k], 2));
                }
            }
        }
        public void gistGabor(double[][] imgsrc, Parameter param)
        {
            //img = single(img);
            int w = param.NumberBlocks;
            int be = param.BoundaryExtension;
            int ny = param.ImageSize[0] + 2 * param.BoundaryExtension;
            int nx = param.ImageSize[1] + 2 * param.BoundaryExtension;
            int Nfilters = param.OrientationsPerScale.Sum();  // Nfilters=32
            double[][][] G = new double[ny][][];
            

            for (int i = 0; i < ny; i++)
            {
                G[i] = new double[nx][];
                for (int j = 0; j < nx; j++)
                {
                    G[i][j] = new double[Nfilters];
                }
            }

            
            int c = 1; 
            int N = 1;
            int nrows = param.ImageSize[0];
            int ncols = param.ImageSize[1];

            int W = w * w;

            double[][] g = new double[W * Nfilters][];
            for (int i = 0; i < W * Nfilters; i++)
            {
                g[i]= new double[N];
            }

            //init g with zeros
            for (int i = 0; i < W * Nfilters; i++)
            {
                for (int j = 0; j < N; j++)
                    g[i][j] = 0;
            }

            //pad image

            double[][] img = new double[ny][];
            for(int i=0;i<ny;i++)
            {
                img[i]= new double[nx];
            }

            img = ProcessPadArray(imgsrc, nrows, ncols, be, be, "both");  // img is as large as G now

            int yshift = (int)Math.Floor(nrows / 2.0);
            int xshift = (int)Math.Floor(ncols / 2.0);
            circshift(ref img, ncols, nrows, yshift, xshift);



            double[][] ig = new double[nrows][];
            for(int i=0;i<nrows;i++)
            {
                ig[i]= new double[ncols];
            }

            Complex[,] data = new Complex[nrows, ncols];



            int k=0;
            for (int n = 1;n<=Nfilters;n++)
            {
                imgMultiplyG(ref img, G, nrows, ncols, n);
                data = ifft2(img,nrows, ncols); 
                
                for(int i=0;i<nrows; i++)
                {
                    for(int j=0;j<ncols;j++)
                    {
                        ig[i][j] = data[i,j].Magnitude; 
                    }
                }



                //Crop output to have same size than the input

                double[][] igOut = new double[param.ImageSize[0]][];
                for (int i = 0; i < param.ImageSize[0]; i++)
                {
                    igOut[i] = new double[param.ImageSize[1]];
                }

                for (int i = 0; i < param.ImageSize[0]; i++)
                {
                    for (int j = 0; j < param.ImageSize[1]; j++)
                    {
                        igOut[i][j]= ig[i+be][j+be];
                    }
                }

                double[][] v = new double[w][];
                for(int i=0;i<w;i++)
                {
                    v[i] = new double[w];
                }

                v = downN(igOut, param.ImageSize[0],param.ImageSize[1],w);
                
                double[][] afterResize = new double[W][];
                for(int i=0;i<W;i++)
                {
                    afterResize[i] = new double[N];
                }
                afterResize = ResizeArray(v, W, N);

                for (int i = 0; i < W; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        g[k+i][j] =afterResize[i][j];
 
                    }
                }

                k = k + W;

            }


        }
        public double[][] ResizeArray(double[][] original, int x, int y)
        {
            double[][] newArray = new double[x][];
            for(int i=0;i<x;i++)
                newArray[i]= new double[y];

            int minX = Math.Min(original.GetLength(0), newArray.GetLength(0));
            int minY = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minY; ++i)
                Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);

            return newArray;
        }
        public double[][] ResizeArrayColwise(double[][] original, int orows, int ocols, int drows, int dcols)
        {
            if (drows*dcols != orows * ocols)
            {
                Console.Write("dim dont match\n");
            }
            
            //create a transpose matrix
            double[][] TArray = new double[ocols][];
            for (int i = 0; i < ocols; i++)
                TArray[i] = new double[orows];

            
            

            //save original into transpose form in TArray
            for (int i = 0; i < orows; i++)
                for (int j = 0; j < ocols; j++)
                    TArray[j][i] = original[i][j];
                    //TArray[j][i] = original[j][i];

            //create a list
            List<double> tlist = new List<double>();

            //copy TArrays in to tlist
            for (int i = 0; i < ocols; i++)
            {
                for (int j = 0; j < orows; j++)
                {
                    tlist.Add(TArray[i][j]);
                }
            }


            //create reshaped newArray
            double[][] newArray = new double[drows][];
            for (int i = 0; i < drows; i++)
                newArray[i] = new double[dcols];

            //copy from tlist to newArray
            for (int i = 0; i < dcols; i++)
            {
                for (int j = 0; j < drows; j++)
                {
                    newArray[j][i] = tlist.First();
                    tlist.RemoveAt(0);
                }
            }

            return newArray;
        }
        public int[] LinSpace_Fix(int d1, int d2, int n = 100)
        {
            double[] nx = new double[n];
            int[] nx_int = new int[n];
            int n1 = n-1;

            nx[0] = d1;

            for (int i = 1; i < n; i++)
            {
                nx[i] = nx[i - 1] + (d2 - d1) / n1;
            }

            for (int i = 0; i < n; i++)
            {
                if (nx[i] > 0)
                    nx_int[i] = (int)Math.Floor(nx[i]);
                else
                    nx_int[i] = (int)Math.Ceiling(nx[i]);
            }

            return nx_int;
        }
        public double[][] downN(double[][] x, int height, int width, int N)
        {
            int[] nx = new int[N + 1];
            int[] ny = new int[N + 1];
            nx = LinSpace_Fix(0, width, N + 1);
            ny = LinSpace_Fix(0, height, N + 1);

            double[][] y = new double[N][];
            for (int i = 0; i < N; i++)
            {
                y[i] = new double[N];
            }

            // init y with 0
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    y[i][j] = 0;
                }
            }

            for(int xx=0;xx<N;xx++)
            {
                for (int yy=0;yy<N;yy++)
                {
                    double sum = 0;
                    int count = 0;

                    for(int k = ny[yy]+1 ; k<= ny[yy+1]; k++)
                    {
                        for(int l = nx[xx]+1 ; l<= nx[xx+1] || k<=ny[yy+1] ;k++, l++)
                        {
                            sum = sum + x[k][l];
                            count++;
                        }
                    }
                    y[xx][yy] =  sum/count;

                }
            }

            return y;



 
        }
        public void imgMultiplyG(ref double[][] img, double[][][] G, int nrows, int ncols, int n)
        {
            

            for (int j = 0; j < nrows; j++)
            {
                for (int k = 0; k < ncols; k++)
                {
                    img[j][k] = img[j][k] * G[j][k][n];
                }
            }

 
        }
        public void TestResizeArrayColwise()
        {
            double[][] original = new double[4][];
            for (int i = 0; i < 3; i++)
            {
                original[i] = new double[3];
            }

            //init
            original[0] = new double[3] { .1, .2, .3 };
            original[1] = new double[3] { .4, .5, .6 };
            original[2] = new double[3] { .7, .8, .9 };
            original[3] = new double[3] { 1.0, 1.1, 1.2 };

            Print2DArray(original, 4, 3);
            Console.Write("after reshape\n");

            Print2DArray(ResizeArrayColwise(original, 4, 3, 3, 4), 3, 4);
            

 
        }
        public static void MainPadarray()
        {

            Padarray padarray = new Padarray();
            int bitmapImageHeight = 128;
            int bitmapImageWidth = 128;
            //double[][] prefiltOut = new double[bitmapImageHeight][];
            //for (int i = 0; i < bitmapImageHeight; i++)
            //{
            //    prefiltOut[i] = new double[bitmapImageWidth];
            //}
            //prefiltOut = padarray.TestPrefilt();

            Parameter param = new Parameter();
            param.NumberBlocks = 4;
            param.Fc_prefilt = 4;
            param.ImageSize[0] = bitmapImageWidth;
            param.ImageSize[1] = bitmapImageHeight;
            param.BoundaryExtension = 32;
            param.G = padarray.createGabor(param.OrientationsPerScale, param.ImageSize[0] + 2 * param.BoundaryExtension);

            //padarray.gistGabor(prefiltOut, param);
            //padarray.TestResizeArrayColwise();

            Console.WriteLine( param.OrientationsPerScale.Sum() * param.NumberBlocks * param.NumberBlocks);

            //System.IO.File.WriteAllLines(@"D:\Codes\Petrel 2015 Plugin\LMgistPlugin1\Tester1\WriteLines.txt", param.G);
            Console.ReadKey();
          
        }

    }
}
