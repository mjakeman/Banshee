//
// PlaybackErrorQueryValue.cs
//
// Author:
//   Alexander Kojevnikov <alexander@kojevnikov.com>
//
// Copyright (C) 2009 Alexander Kojevnikov
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
using System.Collections.Generic;

using Mono.Unix;

using Hyena.Query;

using Banshee.Streaming;

namespace Banshee.Query
{
    public class PlaybackErrorQueryValue : EnumQueryValue
    {
        private static AliasedObjectSet<EnumQueryValueItem> items = new AliasedObjectSet<EnumQueryValueItem>(
            new EnumQueryValueItem (
                (int)StreamPlaybackError.None, "None", I18n.Catalog.GetString ("None"),
                //Translators: These are unique strings for playback errors. Please, no spaces. Blank ok.
                I18n.Catalog.GetString ("None"), I18n.Catalog.GetString ("none"), I18n.Catalog.GetString ("no"),
                "None", "none", "no"),
            new EnumQueryValueItem (
                (int)StreamPlaybackError.ResourceNotFound, "ResourceNotFound", I18n.Catalog.GetString ("Resource Not Found"),
                //Translators: These are unique strings for playback errors. Please, no spaces. Blank ok.
                I18n.Catalog.GetString ("ResourceNotFound"), I18n.Catalog.GetString ("missing"), I18n.Catalog.GetString ("notfound"),
                "ResourceNotFound", "missing", "notfound"),
            new EnumQueryValueItem (
                (int)StreamPlaybackError.CodecNotFound, "CodecNotFound", I18n.Catalog.GetString ("CodecNotFound"),
                //Translators: These are unique strings for playback errors. Please, no spaces. Blank ok.
                I18n.Catalog.GetString ("CodecNotFound"), I18n.Catalog.GetString ("nocodec"),
                "CodecNotFound", "nocodec"),
            new EnumQueryValueItem (
                (int)StreamPlaybackError.Drm, "Drm", I18n.Catalog.GetString ("Drm"),
                //Translators: These are unique strings for playback errors. Please, no spaces. Blank ok.
                I18n.Catalog.GetString ("Drm"), I18n.Catalog.GetString ("drm"),
                "Drm", "drm"),
            new EnumQueryValueItem (
                (int)StreamPlaybackError.Unknown, "Unknown", I18n.Catalog.GetString ("Unknown"),
                //Translators: These are unique strings for playback errors. Please, no spaces. Blank ok.
                I18n.Catalog.GetString ("Unknown"), I18n.Catalog.GetString ("unknown"),
                "Unknown", "unknown")
        );

        public override IEnumerable<EnumQueryValueItem> Items {
            get { return items; }
        }
    }
}
