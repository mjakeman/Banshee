//
// Tinyshee.cs
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
using Banshee.Collection;
using Banshee.MediaEngine;
using Hyena;
using Gtk;

namespace Tinyshee
{
    /// <summary>
    /// This is a minimal example of using Banshee's PlayerEngine system
    /// to open and play a local file on the computer. Typically, most uses
    /// of PlayerEngine would go through PlayerEngineService, so this is
    /// purely for demonstration.
    /// </summary>
    class Program
    {
        private static Window window;
        private static PlayerEngine engine;
        private static Button playButton;
        
        static void Main(string[] args)
        {
            // Initialisation
            Gtk.Global.Init();
            Gst.Application.Init();
            ThreadAssist.InitializeMainThread();
            Paths.ApplicationName = "banshee-demo";

            // Setup Player Engine
            engine = (PlayerEngine)new Banshee.GStreamerSharp.PlayerEngine();
            engine.Initialize();
            engine.EventChanged += PlayerStateChanged;

            // Construct UI
            ConstructUI();
            
            // Run Application
            Gtk.Global.Main();
        }

        private static void ConstructUI()
        {
            // File Chooser
            var fileChooseButton = FileChooserButton.New("Select a file", FileChooserAction.Open);
            fileChooseButton.OnFileSet += (o, e) =>
            {
                var uri = (o as FileChooser).GetUri();
                if (TryOpenSafeUri(uri, out SafeUri safeuri))
                    engine.Open(safeuri);
            };

            // Pause/Play Button
            playButton = new Button("Play!")
            {
                WidthRequest = 100,
                [Button.ClickedSignal] = (btn, e) =>
                {
                    if (engine.CurrentState == PlayerState.Playing)
                        engine.Pause();
                    else
                        engine.Play();
                }
            };

            // Horizontal Box
            var box = new Box(Orientation.Horizontal);
            box.Spacing = 10;
            box.PackStart(fileChooseButton, true, true, 0);
            box.PackStart(playButton, false, false, 0);

            // Main Window
            window = new Window("Tinyshee")
            {
                Child = box,
                DefaultWidth = 350,
                Resizable = false,
                [Widget.DestroySignal] = (o, e) => Gtk.Global.MainQuit()
            };

            window.SetBorderWidth(10);

            // Start Running
            window.ShowAll();
        }

        private static void PlayerStateChanged(PlayerEventArgs args)
        {
            switch (args.Event)
            {
                case PlayerEvent.StateChange:
                    var stateChangeArgs = (PlayerEventStateChangeArgs) args;

                    if (stateChangeArgs.Current == PlayerState.Playing)
                    {
                        // Prefer: Filename -> Unknown Track
                        string? trackTitle = null;
                        if (engine.CurrentTrack?.Uri != null)
                            trackTitle = Path.GetFileName(new Uri(engine.CurrentTrack?.Uri).LocalPath);

                        window.Title = $"Tinyshee - {trackTitle ?? TrackInfo.UnknownTitle}";
                        playButton.Label = "Pause";
                    }
                    else
                    {
                        window.Title = "Tinyshee";
                        playButton.Label = "Play!";
                    }
                    
                    break;
            }
        }

        public static bool TryOpenSafeUri(string uri, out SafeUri safeuri)
        {
            try
            {
                safeuri = new SafeUri(uri);
                return true;
            }
            catch (Exception e)
            {
                var dlg = new Hyena.Gui.Dialogs.ExceptionDialog(e);
                dlg.Run();

                safeuri = null;
                return false;
            }
        }
    }
}
