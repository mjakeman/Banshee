// 
// PlayQueueBox.cs
//  
// Author:
//   Aaron Bockover <abockover@novell.com>
// 
// Copyright 2009 Novell, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gtk;

using Hyena.Data.Gui;
using Banshee.Collection.Gui;

using Banshee.ServiceStack;
using Banshee.Sources;
using Banshee.PlayQueue;

namespace Banshee.Moblin
{
    public class PlayQueueBox : VBox
    {
        private TerseTrackListView playqueue_view;
        
        public PlayQueueBox ()
        {
            Spacing = 5;
            
            FindPlayQueue ();
            
            PackStart (new PlaybackBox (), false, false, 0);
            
            PackStart (new MoblinTrackInfoDisplay () {
                HeightRequest = 64
            }, false, false, 10);
            
            PackEnd (new Hyena.Widgets.ScrolledWindow () {
                (playqueue_view = new TerseTrackListView () {
                    HasFocus = true
                })
            }, true, true, 0);
            
            playqueue_view.ColumnController.Insert (new Column (null, "indicator",
                new ColumnCellStatusIndicator (null), 0.05, true, 20, 20), 0);
            playqueue_view.ColumnController.Add (new Column ("Rating", new ColumnCellRating ("Rating", false), 0.15));
        }
        
        private void FindPlayQueue ()
        {
            Banshee.ServiceStack.ServiceManager.SourceManager.SourceAdded += delegate (SourceAddedArgs args) {
                if (args.Source is Banshee.PlayQueue.PlayQueueSource) {
                    InitPlayQueue (args.Source as Banshee.PlayQueue.PlayQueueSource);
                }
            };

            foreach (Source src in ServiceManager.SourceManager.Sources) {
                if (src is Banshee.PlayQueue.PlayQueueSource) {
                    InitPlayQueue (src as Banshee.PlayQueue.PlayQueueSource);
                }
            }
        }

        private void InitPlayQueue (PlayQueueSource play_queue)
        {
            ServiceManager.SourceManager.SetActiveSource (play_queue);
            //play_queue.TrackModel.Reloaded += HandleTrackModelReloaded;
            playqueue_view.SetModel (play_queue.TrackModel);

            if (MoblinPanel.Instance.ToolbarPanel != null) {
                int old_pref = play_queue.PlayedSongsPreference.Value;
                MoblinPanel.Instance.ToolbarPanel.ShowBeginEvent += (o, a) => play_queue.PlayedSongsPreference.Value = 2;
                MoblinPanel.Instance.ToolbarPanel.HideBeginEvent += (o, a) => play_queue.PlayedSongsPreference.Value = old_pref;
            }
            
            var header = play_queue.CreateHeaderWidget ();
            PackStart (header, false, false, 0);
            header.ShowAll ();
        }
    }
}
