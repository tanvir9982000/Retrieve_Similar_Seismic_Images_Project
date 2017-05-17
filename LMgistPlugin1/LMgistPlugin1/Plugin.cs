using System;
using System.Collections.Generic;

using Slb.Ocean.Core;

namespace LMgistPlugin1
{
    public class Plugin : Slb.Ocean.Core.Plugin
    {
        public override string AppVersion
        {
            get { return "2014.1"; }
        }

        public override string Author
        {
            get { return "MRahman37"; }
        }

        public override string Contact
        {
            get { return "mtrahman3@uh.edu"; }
        }

        public override IEnumerable<PluginIdentifier> Dependencies
        {
            get { return null; }
        }

        public override string Description
        {
            get { return "To provide geophysicists information about similar Seismic images from historical data to making precise decisions easily\r\n"; }
        }

        public override string ImageResourceName
        {
            get { return null; }
        }

        public override Uri PluginUri
        {
            get { return new Uri("http://www.pluginuri.info"); }
        }

        public override IEnumerable<ModuleReference> Modules
        {
            get 
            {
                // Please fill this method with your modules with lines like this:
                //yield return new ModuleReference(typeof(Module));
                yield return new ModuleReference(typeof(LMgistPlugin1.LMgistModule1));
            }
        }

        public override string Name
        {
            get { return "Plugin"; }
        }

        public override PluginIdentifier PluginId
        {
            get { return new PluginIdentifier(GetType().FullName, GetType().Assembly.GetName().Version); }
        }

        public override ModuleTrust Trust
        {
            get { return new ModuleTrust("Default"); }
        }
    }
}
