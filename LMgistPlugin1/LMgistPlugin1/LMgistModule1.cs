using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Drawing;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.UI.Tools;
using Slb.Petrel.Glass.Shell.Docking;

namespace LMgistPlugin1
{

    
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    public class LMgistModule1 : IModule
    {
        public UserControl1 usercontrol;
        public LMgistModule1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IModule Members

        /// <summary>
        /// This method runs once in the Module life; when it loaded into the petrel.
        /// This method called first.
        /// </summary>
        public void Initialize()
        {
            // TODO:  Add LMgistModule1.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            
            // TODO:  Add LMgistModule1.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add LMgistModule1.IntegratePresentation implementation

            string key = "Slb.Studio.SimilarSeismicSearch";
            PetrelToolbar toolbar = new PetrelToolbar(key, "Similar Seismic Search", Docking.Top);
            PetrelSystem.ToolService.AddToolbar(toolbar);
            
            Bitmap gs_red =  LMgistPlugin1.Properties.Resources.SSS;

            PetrelButtonTool tool1 = new PetrelButtonTool("Find similar Seismic images from files", gs_red, ClickCallBackSearch);
            //PetrelButtonTool tool2 = new PetrelButtonTool("Find similar Seismic images", logo3S, ClickCallBackSearch);
            toolbar.AddTool(tool1);
            //toolbar.AddTool(tool2);
        }


        private void ClickCallBackSearch(object sender, EventArgs e)
        {
            //PetrelLogger.InfoBox("Inside  ClickCallBackSearch");
            var dockManager = CoreSystem.GetService<IDockManager>();
            

            
            if (dockManager != null)
            {
                if (usercontrol != null)
                {
                    var isPanelVisible = dockManager.IsContentVisible(usercontrol.Key);
                    dockManager.SetContentVisible(usercontrol.Key, !isPanelVisible);
                }
                else
                {
                    usercontrol = new UserControl1();

                    ContentDescription cdpdescrption = new ContentDescription(usercontrol.Key, usercontrol.Title);
                    ControlFactoryMethod filterControlFactory = delegate
                    {
                        var view = usercontrol;

                        // Avoid exception on application exit. Sergey is going to fix it:
                        if (view.Parent != null) { return new System.Windows.Forms.UserControl(); }

                        return new ElementHost()
                        {
                            Child = view,
                            Text = usercontrol.Title
                        };
                    };
                    dockManager.InstallContent(filterControlFactory, cdpdescrption,
                                               DockedLocation.Floating, new System.Drawing.Size(300, 300));
                    var isPanelVisible = dockManager.IsContentVisible(usercontrol.Key);
                    dockManager.SetContentVisible(usercontrol.Key, !isPanelVisible);
                }
            }

        }

        /// <summary>
        /// This method called once in the life of the module; 
        /// right before the module is unloaded. 
        /// It is usually when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            // TODO:  Add LMgistModule1.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add LMgistModule1.Dispose implementation
        }

        #endregion

    }


}