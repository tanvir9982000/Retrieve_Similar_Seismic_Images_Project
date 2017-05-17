using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WpfApplication1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tester1;
using Slb.Ocean.Petrel;


namespace Tester1
{
    public class Gist
    {
        private int nscenes;
        private int typeD;
        private int nfeatures;
        private int precomputed;
        private Parameter param;
        private int bitmapImageWidth;
        private int bitmapImageHeight;
        public string defaultOutputDirectory;

        #region Set_Get_Functions
        public int Nscenes
        {
            get { return nscenes; }
            set { nscenes = value; }
        }
        public int TypeD
        {
            get { return typeD; }
            set { typeD = value; }
        }
        public int Nfeatures
        {
            get { return nfeatures; }
            set { nfeatures = value; }
        }
        public int Precomputed
        {
            get { return precomputed; }
            set { precomputed = value; }
        }
        public Parameter Param
        {
            get { return param; }
            set { param = value; }
        }
        public int BitmapImageWidth
        {
            get { return bitmapImageWidth; }
            set { bitmapImageWidth = value; }
        }
        public int BitmapImageHeight
        {
            get { return bitmapImageHeight; }
            set { bitmapImageHeight = value; }
        }
#endregion

        //functions
        public Gist()
        {
            this.Nscenes = 1;
            this.TypeD = 3;
            this.Nfeatures = 1;
            this.Precomputed = 0;
            this.Param = new Parameter();
        }
        public Bitmap GetBitMapMatrix(string bitmapFilePath)
        {
            Bitmap bmp = new Bitmap(bitmapFilePath);
            this.BitmapImageHeight = bmp.Height;
            this.BitmapImageWidth = bmp.Width;
            return bmp;
        }
        public double[,] prefilt(double[,] img_grey, Size newsize, int fc = 4)
        {

            int w = 5;
            double s1 = fc / Math.Sqrt(Math.Log(2));

            //pad images to reduce boundary artifacts

            for (int i = 0; i < newsize.Height; i++)
            {
                for (int j = 0; j < newsize.Width; j++)
                {
                    img_grey[i,j] = Math.Log(img_grey[i,j] + 1); 
                }
            }

            int sw = newsize.Width + 2 * w;
            int sh = newsize.Height + 2 * w;

            double[,] b = new double[sh, sw];
            b = ProcessPadArray(img_grey, newsize.Height, newsize.Width, w, w);
            int n = Math.Max(sw, sh);
            n = n + (n % 2);

            double[,] img = new double[sh + n - sw, sw + n - sh];


            img = ProcessPadArray(b, sh, sw, n-sw, n-sh,"post");


            //filter
            MeshGridContainer cont = new MeshGridContainer();  // may cause an error 
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

            //skipping repmat()

            Complex[,] data = new Complex[sh, sw];

            data = fft2(img, sh, sw);


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
                    output[i][j] = img[i,j] - data[i, j].Real;
                }
            }
            //Console.Write("Priniting after whitening\n");
            
            //Local contrast normalization
            double[,] outputSquare = new double[sh,sw];

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    outputSquare[i,j] = Math.Pow(output[i][j], 2);
                }
            }

            data = fft2(outputSquare, sh, sw);
            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    data[i, j] = data[i, j] * gf[i][j];
                }
            }

            data = ifft2(data, sh, sw);

            for (int i = 0; i < sh; i++)
            {
                for (int j = 0; j < sw; j++)
                {
                    outputSquare[i,j] = Math.Sqrt(Math.Abs(data[i, j].Real));
                    outputSquare[i,j] = .2 + outputSquare[i,j];
                    output[i][j] = output[i][j] / outputSquare[i,j];
                }
            }
            //Console.Write("Priniting after Local contrast normalization\n");

            //Crop output to have same size than the input
            double[,] prefiltOut = new double[newsize.Height, newsize.Width];

            for (int i = 0; i < newsize.Height; i++)
            {
                for (int j = 0; j < newsize.Width; j++)
                {
                    prefiltOut[i,j] = output[i + w][j + w];
                }
            }
            //Console.Write("Priniting after croping\n");

            return prefiltOut;
        }
        public double[,] imresizecrop(Bitmap img, int paramHeight, int paramWidth)
        {
            double scaling = Math.Max((double)paramHeight / img.Height, (double)paramWidth / img.Width);
            int newHeight = (int)Math.Round(img.Height * scaling);
            int newWidth = (int)Math.Round(img.Width * scaling);
            Size size = new Size(newWidth,newHeight);
            Bitmap newimage = (new Bitmap(img, size));
            int nr = newimage.Height;
            int nc = newimage.Width;
            int sr = (int)Math.Floor((nr - paramHeight)/2.0);
            int sc = (int)Math.Floor((nc - paramWidth) / 2.0);

            Size size2 = new Size(paramWidth - sc, paramHeight-sr);   // Size(width, height)
            double[,] imgOut = new double[paramHeight, paramWidth];

            for (int i = 0; i < paramHeight; i++)
            {
                for (int j = 0; j < paramWidth; j++)
                {
                    imgOut[i,j] = (double)(newimage.GetPixel(sc + j, sr + i).R + newimage.GetPixel(sc + j, sr + i).G + newimage.GetPixel(sc + j, sr + i).B)/3.0;  // GetPixel(x,y)
                }
            }

            return imgOut;
        }
        public void LMgist(string filename) 
        {
            try 
            {
                Bitmap D = GetBitMapMatrix(filename);
                if (param.ImageWidth == 0 || param.ImageHeight == 0)  // never using this block
                {
                    this.Param.ImageWidth = D.Width;
                    this.Param.ImageHeight = D.Height;
                }

                param.G = createGabor(Param.OrientationsPerScale, param.ImageWidth + 2 * Param.BoundaryExtension); // width or height does not matter coz Param.Width == param.height
                
                /*Precompute filter transfert functions (only need to do this once, unless image size is changes) */
                Nfeatures = Param.OrientationsPerScale.Sum() * Param.NumberBlocks * Param.NumberBlocks;

                //double[,] gist = new double[Nscenes, Nfeatures];
                double[] gist = new double[Nfeatures];
                // Loop: Compute gist features for all scenes
                //for (int n = 0; n < Nscenes; n++)

                double[,] img = new double[param.ImageHeight, param.ImageWidth];

                //Make it grey scale, resize and crop image to make it square
                img = imresizecrop(D, param.ImageWidth, param.ImageHeight);
                Size newsize = new Size(param.ImageWidth, param.ImageHeight);  // size(width, height)  // swapped it while using ImageHeight

                //scale intensities to be in the range [0 255]
                double min = 10000.0;
                double max = -10000.0;
                for (int i = 0; i < newsize.Height; i++)
                {
                    for (int j = 0; j < newsize.Width; j++)
                    {
                        if (min > img[i,j])
                            min = img[i,j];
                    }
                }

                for (int i = 0; i < newsize.Height; i++)
                {
                    for (int j = 0; j < newsize.Width; j++)
                    {
                        img[i,j] -= min;
                        if (max < img[i,j])
                            max = img[i,j];
                    }
                }

                for (int i = 0; i < newsize.Height; i++)
                {
                    for (int j = 0; j < newsize.Width; j++)
                    {
                        img[i,j] *= (255 / max);
                    }
                }

                double[,] output = prefilt(img, newsize, param.Fc_prefilt);
                gist = gistGabor(output, newsize, param);
                JsonWrite(filename, gist);
                PetrelLogger.InfoOutputWindow("Done writing gist in Json\n");
                //return gist;

            }
            catch (System.IO.FileNotFoundException)
            {
                //PetrelLogger.InfoBox("There was an error opening the image." + "Please check the path.");
                Console.Write("There was an error opening the image." + "Please check the path.");
            }
            
        }
        public void JsonWrite(string filename, double[] gist)
        {
            filename = filename.Replace("\\", "").Replace(":", "").Replace("/", "").Replace(" ","");  // removing special char from filename
            GistData gistdata = new GistData(filename, gist);
            var json = JsonConvert.SerializeObject(gistdata, Formatting.Indented);

            //write string to file
            string jfilename = filename + ".json";
            System.IO.File.WriteAllText(@defaultOutputDirectory+jfilename, json);
        }
        public List<double> JsonRead(string filename)
        {
            JObject jobj = JObject.Parse(File.ReadAllText(@filename));
            JArray a = (JArray)jobj["gistvalue"];
            List<double> gistvalues = a.ToObject<List<double>>();

            return gistvalues;

        }
        public double[,] ProcessPadArray(double[,] A, int heightA, int widthA, int padsizeH, int padsizeW, string direction = "both")
        {


            int paddedWidthB = widthA + 2 * padsizeW;
            int paddedHeightB = heightA + 2 * padsizeH;

            int[,] dimNumWidth = new int[heightA, 2 * widthA];
            int[] dimNumHeight = new int[2 * heightA];
            double[,] b = new double[paddedHeightB, paddedWidthB];

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
            int[,] roworder = new int[2, Math.Max(paddedWidthB, paddedHeightB)];


            if (direction.Equals("pre", StringComparison.Ordinal))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        int index = 0;
                        for (int k = -padsizeH; k <= (heightA - 1); k++)
                        {
                            if (index > paddedWidthB) break;

                            int idx = ((k + (2 * heightA)) % (2 * heightA));
                            roworder[0, index++] = dimNumHeight[idx] - 1;
                            //Console.Write(roworder[0, index - 1] + " ");
                        }
                        //Console.WriteLine();
                    }

                    if (i == 1)
                    {
                        int index = 0;
                        for (int k = -padsizeW; k <= (widthA - 1); k++)
                        {
                            if (index > paddedHeightB) break;

                            int idx = ((k + (2 * widthA)) % (2 * widthA));
                            roworder[1, index++] = dimNumWidth[0, idx] - 1;
                            //Console.Write(roworder[1, index - 1] + " ");
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
                            if (index > paddedWidthB) break;

                            int idx = ((k + (2 * heightA)) % (2 * heightA));
                            roworder[0, index++] = dimNumHeight[idx] - 1;
                            //Console.Write(roworder[0, index - 1] + " ");
                        }
                        //Console.WriteLine();
                    }

                    if (i == 1)
                    {
                        int index = 0;
                        for (int k = 0; k <= (widthA + padsizeW - 1); k++)
                        {
                            if (index >= paddedWidthB) break; 
                            int idx = ((k + (2 * widthA)) % (2 * widthA));
                            roworder[1, index++] = dimNumWidth[0, idx] - 1;
                            //Console.Write(roworder[1, index - 1] + " ");
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
                            if (index >= paddedWidthB) break; 
                            int idx = ((k + (2 * heightA)) % (2 * heightA));
                            roworder[0, index++] = dimNumHeight[idx] - 1;
                            //Console.Write(roworder[0, index - 1] + " ");
                            
                        }
                        //Console.WriteLine();
                    }

                    if (i == 1)
                    {
                        int index = 0;
                        for (int k = -padsizeW; k <= (widthA + padsizeW - 1); k++)
                        {
                            if (index >= paddedWidthB) break; 
                            int idx = ((k + (2 * widthA)) % (2 * widthA));
                            roworder[1, index++] = dimNumWidth[0, idx] - 1;
                            //Console.Write(roworder[1, index - 1] + " ");
                            
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
                    b[i,j] = A[roworder[0, i],roworder[1, j]];
                }
            }

            return b;

        }
        public void Print2DArray(double[][] B, int heightB, int widthB)
        {
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/result.txt", true))
            {
                for (int i = 0; i < heightB; i++)
                {
                    for (int j = 0; j < widthB; j++)
                    {
                        Console.Write(B[i][j].ToString() + "\t");
                        Console.Write("{0:F4}\t", B[i][j]);
                        //file.Write("{0:F4}\t", B[i][j].ToString());
                    }
                    //Console.WriteLine();
                }

                //file.WriteLine();
            }
        }
        public Complex[,] fft2(double[,] img, int height, int width)
        {
            Complex[,] data = new Complex[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex tmp = new Complex(img[i,j], 0);
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
        public Complex[,] ifft2(double[,] img, int height, int width)
        {

            Complex[,] data = new Complex[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex tmp = new Complex(img[i,j], 0);
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

            int row, col;
            row = start;
            for (int i = 0; i < meshgridheight; i++, row++)
            {
                col = start;
                for (int j = 0; j < meshgridwidth; j++, col++)
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
        public void circshift(ref double[][] src, int xdim, int ydim, int yshift, int xshift)
        {
            double[][] temp = new double[ydim][];
            for (int i = 0; i < ydim; i++)
            {
                temp[i] = new double[xdim];
                temp[i] = (double[])src[i].Clone(); // 1D Clone is deep
            }
            //temp = (double[][])src.Clone();  // 2D Clone is shallow

            for (int i = 0; i < ydim; i++)
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

            int yshift = (int)Math.Floor(row / 2.0);
            int xshift = (int)Math.Floor(col / 2.0);
            circshift(ref out1, col, row, yshift, xshift);

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
            for (int i = 0; i < Nfilters; i++)
            {
                param[i] = new double[Nscales];
            }
            
            int l = 0;
            for (int i = 0; i < Nscales; i++)       //Nscales = 4
            {
                for (int j = 0; j < or[i]; j++)
                {
                    param[l][0] = .35;
                    param[l][1] = .3 / Math.Pow(1.85, (i));
                    param[l][2] = 16 * Math.Pow(or[i], 2) / (32 * 32);
                    param[l][3] = Math.PI / (or[i]) * (j);
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

            fr = fftshiftCreateGabor(meshgridwidth, meshgridheight, cont.container1, cont.container2);

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
                ArrayAddParami(ref tr, t, param[i][3], meshgridheight, meshgridwidth);
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
        public void ArrayAddG(ref double[][][] G, double[][] tr, double[][] fr, double[][] param, int i, int n_1, int meshgridheight, int meshgridwidth)
        {

            for (int j = 0; j < meshgridheight; j++)
            {
                for (int k = 0; k < meshgridwidth; k++)
                {
                    G[j][k][i] = Math.Exp(-10 * param[i][0] * Math.Pow((fr[j][k] / n_1 / param[i][1] - 1), 2) - 2 * param[i][2] * Math.PI * Math.Pow(tr[j][k], 2));
                }
            }
        }
        public double[] gistGabor(double[,] imgsrc, Size newsize, Parameter param)
        {
            //img = single(img);
            int w = param.NumberBlocks;
            int be = param.BoundaryExtension;
            int ny = param.ImageHeight + 2 * param.BoundaryExtension;
            int nx = param.ImageWidth + 2 * param.BoundaryExtension;
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

            G = param.G;

            int N = 1;
            int nrows = newsize.Height;
            int ncols = newsize.Width;
            int W = w * w;
            double[] g = new double[W * Nfilters];

            //init g with zeros
            for (int i = 0; i < W * Nfilters; i++)
            {
                g[i] = 0;
            }

            //pad image
            double[,] img = new double[ny,nx];
            img = ProcessPadArray(imgsrc, nrows, ncols, be, be, "both");  // img is as large as G now

            double[][] ig = new double[ny][];
            for (int i = 0; i < ny; i++)
            {
                ig[i] = new double[nx];
            }

            Complex[,] data = new Complex[ny, nx];
            data = fft2(img, ny, nx); // data is ny times smaller than matlab data
            Complex[,] d = new Complex[ny, nx];

            int k = 0;
            for (int n = 0; n < Nfilters; n++)
            {
                d = imgMultiplyG(data, G, ny, nx, n);
                d = ifft2(d, ny, nx);

                for (int i = 0; i < ny; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        ig[i][j] = d[i, j].Magnitude;
                    }
                }

                //Crop output to have same size than the input

                double[][] igOut = new double[param.ImageHeight][];
                for (int i = 0; i < param.ImageHeight; i++)
                {
                    igOut[i] = new double[param.ImageWidth];
                }

                for (int i = 0; i < param.ImageHeight; i++)
                {
                    for (int j = 0; j < param.ImageWidth; j++)
                    {
                        igOut[i][j] = ig[i + be][j + be];
                    }
                }

                double[][] v = new double[w][];
                for (int i = 0; i < w; i++)
                {
                    v[i] = new double[w];
                }

                v = downN(igOut, param.ImageHeight, param.ImageWidth, w);

                List<double> afterResize = new List<double>();
                afterResize = ResizeArrayColwise(v,w,w, W, N);

                for (int i = 0; i < W; i++)
                {
                    g[k + i] = afterResize[i];
                }

                k = k + W;
            }

            return g;

        }
        public List<double> ResizeArrayColwise(double[][] original, int orows, int ocols, int drows, int dcols)
        {
            if (drows * dcols != orows * ocols)
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
            return tlist;
        }
        public int[] LinSpace_Fix(int d1, int d2, int n = 100)
        {
            double[] nx = new double[n];
            int[] nx_int = new int[n];
            int n1 = n - 1;

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

            for (int yy = 0; yy < N; yy++)
            {
                for (int xx = 0; xx < N; xx++)
                {
                    double sum = 0;
                    int count = 0;

                    for (int k = ny[yy]; k < ny[yy + 1]; k++)
                    {
                        for (int l = nx[xx]; l < nx[xx + 1] ; l++)
                        {
                            sum = sum + x[k][l];
                            count++;
                        }
                    }
                    y[yy][xx] = sum / count;
                }
            }
            return y;
        }
        public Complex[,] imgMultiplyG(Complex[,] img, double[][][] G, int nrows, int ncols, int n)
        {
            Complex[,] d = new Complex[nrows,ncols];
            for (int j = 0; j < nrows; j++)
            {
                for (int k = 0; k < ncols; k++)
                {
                    d[j,k] = img[j,k] * G[j][k][n];
                }
            }
            return d;
        }
        public double FindDistance(List<double> refgistlist, List<double> gistlist)
        {
            double sum = 0;
            for (int i = 0; i < refgistlist.Count; i++)
            {
                sum = sum + Math.Pow((refgistlist[i] - gistlist[i]), 2);
            }
            return sum;
        }
        public void ProcessDirectory1(string targetDirectory, Gist gist)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                //gist.LMgist(fileName, ref gistcontainer, fileindex++);
                var watch = Stopwatch.StartNew();
                gist.LMgist(fileName);
                watch.Stop();
                var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/result.txt", true))
                {
                    //file.WriteLine("Distance1-{2} : {0} elapsed time: {1}", gist.FindDistance(gistcontainer), elapsedSec, fileName);
                }

            }
            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                gist.ProcessDirectory(subdirectory, gist);
        }
        public void ProcessDirectory(string targetDirectory, Gist gist)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                gist.LMgist(fileName);
            }
            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                gist.ProcessDirectory(subdirectory, gist);
        }

        //public static void Main()
        //{
        //    var watch = Stopwatch.StartNew();
        //    Gist gist = new Gist();
        //    GistContainer gistcontainer = new GistContainer();
        //    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        //    gist.LMgist("D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/demo1.jpg", ref gistcontainer, 1);
        //    gist.LMgist("D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/demo3.jpg", ref gistcontainer, 2);
        //    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        //    watch.Stop();
        //    var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
        //    Console.Write("Distance13 : {0} elapsed time: {1}", gist.FindDistance(gistcontainer), elapsedSec);
        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/result.txt", true))
        //    {
        //        file.WriteLine("Distance13 : {0} elapsed time: {1}", gist.FindDistance(gistcontainer), elapsedSec);
        //    }

        //    //gist.Print2DArray(gistcontainer.refImage, gistcontainer.refImageRows, gistcontainer.refImageCols);
        //    //gist.Print2DArray(gistcontainer.secondImage, gistcontainer.secondImageRows, gistcontainer.secondImageCols);
        //    Console.ReadKey();
        //}

        #region testing code Unused
        /*
        public static void Main()
        {
            double[][] a = new double[2][];
            a[0] = new double[2];
            a[1] = new double[2];

            a[0][0] = 1;
            a[0][1] = 2;
            a[1][0] = 3;
            a[1][1] = 4;

            Gist gist = new Gist();
            Complex[,] data = new Complex[2, 2];

            data = gist.fft2(a, 2, 2);


            Console.ReadKey();
        }
        
        public double[][] TestPrefilt()
        {
            int bitmapImageWidth = 2;
            int bitmapImageHeight = 2;
            double[][] a = new double[bitmapImageHeight][];
            a[0] = new double[] { .1, .2 };
            a[1] = new double[] { .3, .4 };
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
            //Print2DArray(b, sw, sh);
            //Console.WriteLine();

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
            //Print2DArray(output, sh, sw);

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
                    data[i, j] = data[i, j] * gf[i][j];
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
            //Print2DArray(output, sh, sw);

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
                    prefiltOut[i][j] = output[i + w][j + w];
                }
            }

            Console.Write("Priniting after croping\n");
            //Print2DArray(prefiltOut, bitmapImageHeight, bitmapImageWidth);

            return prefiltOut;
        }
         
         * */
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

            //Print2DArray(ResizeArrayColwise(original, 4, 3, 3, 4), 3, 4);

        }
        public void Print2DArray(double[,] B, int heightB, int widthB)
        {
            for (int i = 0; i < heightB; i++)
            {
                for (int j = 0; j < widthB; j++)
                {
                    Console.Write(B[i, j].ToString() + "\t");
                    Console.Write("{0:F4}\t", B[i, j]);
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
                    Console.Write("{0:F4}\t", B[i, j]);
                }
                Console.WriteLine();
            }
        }
        #endregion

    }
}
