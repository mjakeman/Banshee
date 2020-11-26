using System;
using Banshee.GStreamerSharp;
using Banshee.MediaEngine;
using Hyena;
using Gtk;

namespace Tinyshee
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == null || args[0] == "--help" || args[0] == "-h" || args[0] == "help")
            {
                Console.WriteLine("Tinyshee - Like Banshee, but smaller!");
                Console.WriteLine("Usage: Enter a valid URI or filename pointing to an audio track");
                Console.WriteLine("For example: dotnet run C:/tmp/track02.wav");
                Console.WriteLine("Must be an absolute path!");
                return;
            }
            
            Gtk.Global.Init();
            Gst.Application.Init();
            ThreadAssist.InitializeMainThread();

            Paths.ApplicationName = "banshee-demo";
            Banshee.MediaEngine.PlayerEngine engine = new Banshee.GStreamerSharp.PlayerEngine();
            engine.Initialize();
            SafeUri safeuri;
            try
            {
                safeuri = new SafeUri(args[0]);
                engine.Open(safeuri);

                var window = new Window("Tinyshee - Minimal Music Player")
                {
                    Child = new Button("Play!")
                    {
                        [Button.ClickedSignal] = (btn, e) => engine.Play(),
                    },
                    [Widget.DestroySignal] = (o, e) => Gtk.Global.MainQuit()
                };
                
                window.ShowAll();
            }
            catch (Exception e)
            {
                var dlg = new Hyena.Gui.Dialogs.ExceptionDialog(e);
                dlg.Run();
            }
            
            Gtk.Global.Main();
        }
    }
}