//
// Program.cs
//
// Author:
//   Matthew Jakeman <mjak923@aucklanduni.ac.nz>
//
// Copyright (C) 2020 Matthew Jakeman
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;

using Hyena;
using Hyena.Addins;

using Banshee.Collection;
using Banshee.MediaEngine;
using Banshee.Preferences;
using Banshee.ServiceStack;
using Banshee.Sources;
using Catalog = Banshee.I18n.Catalog;

using Gtk;

namespace Test
{
    public class TestClient : Client
    {
        public static void Main ()
        {
            // Setup Logging
            Paths.ApplicationName = "banshee-test-player";
            Log.Information ("Test client started");

            Startup ();
        }

        private static void Startup ()
        {
            // Initialisation
            ThreadAssist.InitializeMainThread ();
            Catalog.Init ("banshee", "./locale");
            
            Gtk.Global.Init();
            Gst.Application.Init();
            
            // Look for new add-ins and update registry
            AddinManager.Initialize ();
            AddinManager.Registry.Update (null);

            // Banshee Services
            ServiceManager.Initialize ();
            ServiceManager.RegisterService<DBusServiceManager> ();
            ServiceManager.RegisterService<SourceManager> ();
            ServiceManager.RegisterService<PreferenceService> ();
            ServiceManager.RegisterService<PlayerEngineService> ();
            ServiceManager.RegisterService<Player> ();
            ServiceManager.Run ();

            ServiceManager.Get<Player> ().Run ();
        }
        
        public override string ClientId => "TestClient";
    }
}

