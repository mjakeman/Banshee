using System;
using Banshee.MediaEngine;
using Gst;
using GstAudio;
using GstVideo;
using Hyena;

namespace Banshee.GStreamerSharp
{
    [Obsolete("Stub Code")]
    public class VideoManager
    {
        Element playbin;
        VideoDisplayContextType video_display_context_type;
        IntPtr video_window;
        IntPtr? video_window_xid;
        
        // NOTE: VideoOverlayAdapter relies on the adapter pattern for using GInterfaces
        // We are not necessarily going down this route with gir.core, so this class will
        // likely have to be rewritten.
        /*VideoOverlayAdapter*/ VideoOverlay xoverlay;

        object video_mutex = new object ();

        public VideoManager (Element playbin)
        {
            Console.WriteLine("Created VideoManager");
        }

        public void Initialize ()
        {
            Console.WriteLine("VideoManager: Initialize");
        }

        private void RaisePrepareWindow ()
        {
            Console.WriteLine("VideoManager: RaisePrepareWindow");
        }

        public delegate void PrepareWindowHandler ();
        public event PrepareWindowHandler PrepareWindow;

        public delegate void VideoGeometryHandler (int width, int height, int fps_n, int fps_d, int par_n, int par_d);
        public event VideoGeometryHandler VideoGeometry;

        private void OnSyncMessage (object o, Bus.SyncMessageSignalArgs args)
        {
            Console.WriteLine("VideoManager: OnSyncMessage");
        }

        private void OnVideoSinkElementAdded (object o, Bin.ElementAddedSignalArgs args)
        {
            Console.WriteLine("VideoManager: OnVideoSinkElementAdded");
        }

        internal void InvalidateOverlay ()
        {
            Console.WriteLine("VideoManager: InvalidateOverlay");
        }

        internal bool MaybePrepareOverlay ()
        {
            Console.WriteLine("VideoManager: MaybePrepareOverlay");
            return false;
        }

        private bool FindXOverlay ()
        {
            Console.WriteLine("VideoManager: FindXOverlay");
            return false;
        }

        public void ParseStreamInfo ()
        {
            Console.WriteLine("VideoManager: Parse Stream Info");
        }

        private void OnCapsSet (object o, GObject.Object.NotifySignalArgs args)
        {
            Console.WriteLine("VideoManager: OnCapsSet");
        }

        private void RaiseVideoGeometry (int width, int height, int fps_n, int fps_d, int par_n, int par_d)
        {
            Console.WriteLine("VideoManager: RaiseVideoGeometry");
        }

        public void WindowExpose (IntPtr window, bool direct)
        {
            Console.WriteLine("VideoManager: WindowExpose");
        }

        public void WindowRealize (IntPtr window)
        {
            Console.WriteLine("VideoManager: WindowRealize");
        }

        public VideoDisplayContextType VideoDisplayContextType {
            get { return video_display_context_type; }
        }

        public IntPtr VideoDisplayContext {
            set { 
                if (VideoDisplayContextType == VideoDisplayContextType.GdkWindow) {
                    video_window = value;
                }
            }
            get { 
                if (VideoDisplayContextType == VideoDisplayContextType.GdkWindow) {
                    return video_window;
                }
                return IntPtr.Zero;
            }
        }
    }
    
    public class CddaManager
    {
        public CddaManager (Element playbin)
        {
            Console.WriteLine("Created CddaManager");
        }

        public string Device {
            get; set;
        }

        private AudioCdSrc GetCddaSource (Element playbin)
        {
            Console.WriteLine("CddaManager: GetCddaSource");
            return null;
        }

        private void OnSourceChanged (object o, GObject.Object.NotifySignalArgs args)
        {
            Console.WriteLine("CddaManager: OnSourceChanged");
        }

        bool SeekToTrack (Element playbin, int track)
        {
            Console.WriteLine("CddaManager: SeekToTrack");

            return false;
        }

        public bool HandleURI (Element playbin, string uri)
        {
            Console.WriteLine("CddaManager: HandleURI");
            return false;
        }
    }
    
    public class DvdManager
    {
        public DvdManager (Element playbin)
        {
            Console.WriteLine("Created DvdManager");
        }
        
        // NOTE: This is actually a Gst INavigation
        Navigation Navigation {
            get; set;
        }
        Element NavigationElement {
            get; set;
        }

        public string Device {
            get; set;
        }

        public bool InDvdMenu {
            get; set;
        }

        private Element GetDvdSource (Element playbin)
        {
            Console.WriteLine("DvdManager: GetDvdSource");

            return null;
        }

        private void OnSourceChanged (object o, GObject.Object.NotifySignalArgs args)
        {
            Console.WriteLine("DvdManager: OnSourceChanged");
        }

        public bool HandleURI (Element playbin, string uri)
        {
            Console.WriteLine("DvdManager: HandleURI");

            return false;
        }

        public void HandleCommandsChanged (Element playbin)
        {
            Console.WriteLine("DvdManager: HandleCommandsChanged");
        }

        public void FindNavigation (Element playbin)
        {
            Console.WriteLine("DvdManager: FindNavigation");
        }

        public void NotifyMouseMove (Element playbin, double x, double y)
        {
            Console.WriteLine("DvdManager: NotifyMouseMove");
        }

        public void NotifyMouseButtonPressed (Element playbin, int button, double x, double y)
        {
            Console.WriteLine("DvdManager: NotifyMouseButtonPressed");
        }

        public void NotifyMouseButtonReleased (Element playbin, int button, double x, double y)
        {
            Console.WriteLine("DvdManager: NotifyMouseButtonReleased");
        }

        public void NavigateToLeftMenu (Element playbin)
        {
            Console.WriteLine("DvdManager: NavigateToLeftMenu");
        }

        public void NavigateToRightMenu (Element playbin)
        {
            Console.WriteLine("DvdManager: NavigateToRightMenu");
        }

        public void NavigateToUpMenu (Element playbin)
        {
            Console.WriteLine("DvdManager: NavigateToUpMenu");
        }

        public void NavigateToDownMenu (Element playbin)
        {
            Console.WriteLine("DvdManager: NavigateToDownMenu");
        }

        public void NavigateToMenu (Element playbin)
        {
            Console.WriteLine("DvdManager: NavigateToMenu");
        }

        public void ActivateCurrentMenu (Element playbin)
        {
            Console.WriteLine("DvdManager: ActivateCurrentMenu");
        }

        public void GoToNextChapter (Element playbin)
        {
            Console.WriteLine("DvdManager: GoToNextChapter");
        }

        public void GoToPreviousChapter (Element playbin)
        {
            Console.WriteLine("DvdManager: GoToPreviousChapter");
        }
    }
}