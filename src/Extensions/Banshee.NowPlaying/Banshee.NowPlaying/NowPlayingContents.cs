//
// NowPlayingContents.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//
// Copyright (C) 2008 Novell, Inc.
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
using Gtk;

using Banshee.Gui.Widgets;

namespace Banshee.NowPlaying
{
    public class NowPlayingContents : Table, IDisposable
    {
        private VideoDisplay video_display;
        private bool video_display_initial_shown = false;
        
        private TrackInfoDisplay track_info_display;
    
        public NowPlayingContents () : base (1, 1, false)
        {
            NoShowAll = true;
        
            video_display = new XOverlayVideoDisplay ();
            video_display.IdleStateChanged += OnVideoDisplayIdleStateChanged;
            Attach (video_display, 0, 1, 0, 1, 
                AttachOptions.Expand | AttachOptions.Fill, 
                AttachOptions.Expand | AttachOptions.Fill, 0, 0);
                
            track_info_display = new NowPlayingTrackInfoDisplay ();
            Attach (track_info_display, 0, 1, 0, 1, 
                AttachOptions.Expand | AttachOptions.Fill, 
                AttachOptions.Expand | AttachOptions.Fill, 0, 0);
        }
        
        public override void Dispose ()
        {
            if (video_display != null) {
                video_display.IdleStateChanged -= OnVideoDisplayIdleStateChanged;
                video_display = null;
            }
            
            base.Dispose ();
        }
        
        protected override void OnShown ()
        {
            base.OnShown ();
            
            // Ugly hack to ensure the video window is mapped/realized
            if (!video_display_initial_shown) {
                video_display_initial_shown = true;
                video_display.Show ();
                GLib.Idle.Add (delegate { 
                    CheckIdle (); 
                    return false;
                });
                return;
            }
            
            CheckIdle ();
        }
        
        protected override void OnHidden ()
        {
            base.OnHidden ();
            video_display.Hide ();
        }

        private void CheckIdle ()
        {
            video_display.Visible = !video_display.IsIdle;
            track_info_display.Visible = video_display.IsIdle;
        }

        private void OnVideoDisplayIdleStateChanged (object o, EventArgs args)
        {
            CheckIdle ();
        }
    }
}
