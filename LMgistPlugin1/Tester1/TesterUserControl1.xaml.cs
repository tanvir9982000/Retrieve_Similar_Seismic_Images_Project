using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Tester1
{
    /// <summary>
    /// Interaction logic for TesterUserControl1.xaml
    /// </summary>
    public partial class TesterUserControl1 : UserControl
    {
        public TesterUserControl1()
        {
            InitializeComponent();
        }

        private void FSI_Button_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();
            Gist gist = new Gist();
            GistContainer gistcontainer = new GistContainer();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            gist.LMgist("D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/demo1.jpg", ref gistcontainer, 1);
            gist.LMgist("D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/demo3.jpg", ref gistcontainer, 2);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            watch.Stop();
            var elapsedSec = watch.ElapsedMilliseconds / 1000.0;
            Console.Write("Distance13 : {0} elapsed time: {1}", gist.FindDistance(gistcontainer), elapsedSec);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:/Codes/Petrel 2015 Plugin/LMgistPlugin1/Tester1/Resources/result.txt", true))
            {
                file.WriteLine("Distance13 : {0} elapsed time: {1}", gist.FindDistance(gistcontainer), elapsedSec);
            }

            //gist.Print2DArray(gistcontainer.refImage, gistcontainer.refImageRows, gistcontainer.refImageCols);
            //gist.Print2DArray(gistcontainer.secondImage, gistcontainer.secondImageRows, gistcontainer.secondImageCols);
            Console.ReadKey();
        }

        
    }
}
