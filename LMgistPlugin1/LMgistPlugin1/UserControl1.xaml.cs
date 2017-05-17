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
using Slb.Ocean.Petrel;
using System.Diagnostics;
using ScreenShotSample;
using Tester1;
using System.IO;
using System.Windows.Forms;
using WpfApplication1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LMgistPlugin1
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : System.Windows.Controls.UserControl
    {
        public string Key = "Slb.Studio.GraphSearch";
        public string Title = "Search Similar Seismic Images";
        public string defaultOutputDirectory;
        public List<GistData> globalgistdatalist = new List<GistData>();
        public GistData globalrefgistdata = new GistData();
        public UserControl1()
        {
            InitializeComponent();
        }
        private void Snip_Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.WindowState = WindowState.Maximized;
            win.ShowDialog();
        }
        private void FSI_Button_Click(object sender, RoutedEventArgs e)
        {
            Gist gist = new Gist();
            var refgistdata = new GistData();
            //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Locate the file to compare";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == System.Windows.Forms.DialogResult.OK) // Test result.
            {
                string refImage = openFileDialog1.FileName;

                Image i = new Image();
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(refImage, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                i.Source = src;
                i.Stretch = Stretch.Uniform;
                //int q = src.PixelHeight;        // Image loads here
                sp0.Children.Clear();           // clear previous image
                sp0.Children.Add(i);

                gist.defaultOutputDirectory = defaultOutputDirectory;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                gist.LMgist(refImage, true);

                refgistdata = gist.refImgGistData;
                globalrefgistdata = refgistdata; //save it for KNN
                Console.WriteLine("Done reading reference");

                //Now compare refImage with precomputed gists
                FolderBrowserDialog infbd = new FolderBrowserDialog();
                infbd.Description = "Select the directory that contains precomputed Image gists";
                DialogResult result1 = infbd.ShowDialog();

                if (result1 == System.Windows.Forms.DialogResult.OK) // Test result1.
                {
                    string[] files = Directory.GetFiles(infbd.SelectedPath);
                    List<GistData> gistdataforSort = new List<GistData>();
                    foreach (string path in files)
                    {
                        if (File.Exists(path))
                        {
                            // This path is a file
                            var gistdata = gist.JsonRead(path);
                            var distance = gist.FindDistance(refgistdata, gistdata);
                            var gistdataWithoutValue = new GistData(distance, gistdata.filename, gistdata.metadata);
                            gistdataforSort.Add(gistdataWithoutValue);

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:/Users/MRahman37/Desktop/Output log/result.txt", true))
                            {
                                file.WriteLine("{0} {1} {2}", distance, gistdata.filename, gistdata.metadata);
                            }
                            Console.WriteLine("D= {0} File= {1}", gist.FindDistance(refgistdata, gistdata), gistdata.filename);
                        }

                        else
                        {
                            Console.WriteLine("{0} is not a valid file or directory.", path);
                        }
                    }

                    Console.WriteLine("Done FSI");

                    globalgistdatalist = gistdataforSort.OrderBy(o => o.distance).ToList();

                    //Now load the files in the stack panel
                    for (int j = 0; j < globalgistdatalist.Count; j++)
                    {
                        if (j > 0)
                            if (globalgistdatalist[j].filename == globalgistdatalist[j - 1].filename && globalgistdatalist[j].distance == globalgistdatalist[j - 1].distance)
                                continue;

                        Image img = new Image();
                        BitmapImage source = new BitmapImage();
                        source.BeginInit();
                        source.UriSource = new Uri(globalgistdatalist[j].filename, UriKind.Relative);
                        source.CacheOption = BitmapCacheOption.OnLoad;
                        source.DecodePixelWidth = 200;
                        source.DecodePixelHeight = 200;
                        source.EndInit();
                        img.Source = source;
                        img.Stretch = Stretch.UniformToFill;
                        int qq = source.PixelHeight;        // Image loads here
                        System.Windows.Controls.ToolTip tooltip = new System.Windows.Controls.ToolTip();
                        tooltip.Content = globalgistdatalist[j].metadata;
                        img.ToolTip = tooltip;
                        //sp1.Children.Add(img);
                        //lv1.Items.Add(img);
                        //imgrid.Source = source;
                        //System.Windows.Controls.CheckBox checkbox = new System.Windows.Controls.CheckBox();

                        //checkbox.HorizontalAlignment = HorizontalAlignment.Left;
                        //checkbox.VerticalAlignment = VerticalAlignment.Bottom;
                        System.Windows.Controls.RadioButton radio1like = new System.Windows.Controls.RadioButton();
                        System.Windows.Controls.RadioButton radio2dislike = new System.Windows.Controls.RadioButton();
                        radio1like.VerticalAlignment = VerticalAlignment.Bottom;
                        radio1like.Content = "Yes";
                        radio2dislike.VerticalAlignment = VerticalAlignment.Bottom;
                        radio2dislike.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        radio2dislike.Content = "No";
                        var grid = new Grid();
                        grid.Children.Add(img);
                        grid.Children.Add(radio1like);
                        grid.Children.Add(radio2dislike);
                        lv1.Items.Add(grid);

                        //if (checkbox.IsChecked == true)
                        //{
                        //    globalgistdatalist[j].IsSimilar = 1;
                        //    imgrid.Source = source;
                        //}
                        //else
                        //    globalgistdatalist[j].IsSimilar = -1;

                        if (radio1like.IsChecked == true)
                        {
                            globalgistdatalist[j].IsSimilar = 1;
                        }
                        else if (radio2dislike.IsChecked == true)
                            globalgistdatalist[j].IsSimilar = -1;




                        Console.WriteLine("FSI: " + globalgistdatalist[j].filename + " " + globalgistdatalist[j].IsSimilar + " distance :" + globalgistdatalist[j].distance);
                    }
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }

        }
        public bool hasExtension(string ext, string[] validExtensionlist)
        {
            foreach (string vExt in validExtensionlist)
            {
                if (ext.Equals(vExt, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        private void PCG_Button_Click(object sender, RoutedEventArgs e)
        {
            Gist gist = new Gist();
            FolderBrowserDialog infbd = new FolderBrowserDialog();
            // Set the help text description for the FolderBrowserDialog. 
            infbd.Description = "Select Input image directory";
            DialogResult result = infbd.ShowDialog();

            FolderBrowserDialog outfbd = new FolderBrowserDialog();
            outfbd.Description = "Select Output directory to save the image gist";
            DialogResult result1 = outfbd.ShowDialog();

            gist.defaultOutputDirectory = outfbd.SelectedPath;
            gist.defaultOutputDirectory = gist.defaultOutputDirectory + "\\";
            this.defaultOutputDirectory = gist.defaultOutputDirectory;

            string[] files = Directory.GetFiles(infbd.SelectedPath);
            string[] imageExtensionList = { ".jpg", ".jpeg", ".png", ".bmp" };



            //first create gistdata for every image files
            foreach (string path in files)
            {
                if (File.Exists(path) && (hasExtension(System.IO.Path.GetExtension(path), imageExtensionList)))
                {
                    gist.LMgist(path);
                }
            }

            //Now for every .metadata file, update the corresponding gistdata in defaultOutputDirectory
            string[] gistdatafiles = Directory.GetFiles(outfbd.SelectedPath);

            for (int i = 0; i < gistdatafiles.Length; i++)
            {
                gistdatafiles[i] = gistdatafiles[i].Replace(defaultOutputDirectory, "");
            }


            foreach (string path in files)
            {
                if (File.Exists(path) && System.IO.Path.GetExtension(path).Equals(".metadata", StringComparison.OrdinalIgnoreCase))
                {
                    string folder = System.IO.Path.GetDirectoryName(path);
                    folder = folder + "\\";
                    string metadataFilenameOnly = path.Replace(folder, "").Replace(".metadata", "");
                    string jfilename = defaultOutputDirectory + metadataFilenameOnly;
                    if (File.Exists(jfilename) /*&& gistdatafiles.Contains(metadataFilenameOnly)*/)
                    {
                        string metadatatext = System.IO.File.ReadAllText(@path);
                        var jsonMeta = JsonConvert.SerializeObject(metadatatext, Formatting.Indented);

                        string json = File.ReadAllText(@defaultOutputDirectory + metadataFilenameOnly);
                        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        jsonObj["metadata"] = jsonMeta;
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(@jfilename, output);
                    }
                }
            }

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(infbd.SelectedPath);
            foreach (string subdirectory in subdirectoryEntries)
                gist.ProcessDirectory(subdirectory, gist);

            Console.WriteLine("Done precomputing");

        }
        private void Reset_SIW_button_Click(object sender, RoutedEventArgs e)
        {
            //sp1.Children.Clear();
            lv1.Items.Clear();
        }
        private void sv1_MouseHover(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                var lvImg = lv1.SelectedIndex;
                if (lvImg != -1)
                {
                    string refImage = globalgistdatalist[lvImg].filename;

                    Image i = new Image();
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(refImage, UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    i.Source = src;
                    i.Stretch = Stretch.Uniform;
                    imgrid.Source = src;
                }


            }
            catch { }


        }
        private void sv1_MouseLeave(object sender, System.EventArgs e)
        {
            imgrid.Source = null;
        }
        private void KNN_Button_Click(object sender, RoutedEventArgs e)
        {
            imgrid.Source = null;
            KNN knn = new KNN();

            List<GistData> trainSamples = new List<GistData>();
            List<GistData> testSamples = new List<GistData>();
            List<GistData> trainClasses = new List<GistData>();

            int fakeSimilar = 0;

            foreach (var item in globalgistdatalist)
            {


                if (fakeSimilar % 3 == 2)
                    item.IsSimilar = 0;   //no comment, will go to testSamples
                else if (fakeSimilar % 3 == 1)
                    item.IsSimilar = 1;  //like, Will go to trainSamples
                else if (fakeSimilar % 3 == 0)
                    item.IsSimilar = -1;  //dislike, Will go to trainSamples

                Console.WriteLine("Before KNN: " + item.filename + " " + item.IsSimilar);

                fakeSimilar++;

                if (item.IsSimilar == 0)
                    testSamples.Add(item);
                else
                    trainSamples.Add(item);
            }


            trainClasses = knn.TestKnnCase(trainSamples, testSamples, 3, 0.5);
            globalgistdatalist.Clear();
            globalgistdatalist.AddRange(trainClasses);
            globalgistdatalist.AddRange(trainSamples);
            Console.WriteLine(globalgistdatalist.Count());

            //sp1.Children.Clear();
            lv1.Items.Clear();

            globalgistdatalist = globalgistdatalist.OrderByDescending(o => o.IsSimilar).ThenBy(o => o.distance).ToList();

            //Now load the files in the stack panel
            for (int j = 0; j < globalgistdatalist.Count; j++)
            {
                if (j > 0)
                    if (globalgistdatalist[j].filename == globalgistdatalist[j - 1].filename && globalgistdatalist[j].distance == globalgistdatalist[j - 1].distance)
                        continue;

                Image img = new Image();

                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(globalgistdatalist[j].filename, UriKind.Relative);
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.DecodePixelWidth = 200;
                source.DecodePixelHeight = 200;
                source.EndInit();
                img.Source = source;
                img.Stretch = Stretch.Uniform;
                int qq = source.PixelHeight;        // Image loads here
                System.Windows.Controls.ToolTip tooltip = new System.Windows.Controls.ToolTip();
                tooltip.Content = globalgistdatalist[j].metadata;
                img.ToolTip = tooltip;
                //sp1.Children.Add(img);




                System.Windows.Controls.RadioButton radio1like = new System.Windows.Controls.RadioButton();
                System.Windows.Controls.RadioButton radio2dislike = new System.Windows.Controls.RadioButton();
                radio1like.VerticalAlignment = VerticalAlignment.Bottom;
                radio1like.Content = "Yes";
                radio2dislike.VerticalAlignment = VerticalAlignment.Bottom;
                radio2dislike.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                radio2dislike.Content = "No";
                var grid = new Grid();
                grid.Children.Add(img);
                grid.Children.Add(radio1like);
                grid.Children.Add(radio2dislike);
                lv1.Items.Add(grid);

                //if (checkbox.IsChecked == true)
                //{
                //    globalgistdatalist[j].IsSimilar = 1;
                //    imgrid.Source = source;
                //}
                //else
                //    globalgistdatalist[j].IsSimilar = -1;

                if (radio1like.IsChecked == true)
                {
                    globalgistdatalist[j].IsSimilar = 1;
                }
                else if (radio2dislike.IsChecked == true)
                    globalgistdatalist[j].IsSimilar = -1;


                Console.WriteLine("After KNN: " + globalgistdatalist[j].filename + " " + globalgistdatalist[j].IsSimilar + " distance :" + globalgistdatalist[j].distance);
            }

            lv1.Items.Refresh();  

        }

    }
}
