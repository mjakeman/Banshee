//
// BansheeQuery.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//   Gabriel Burt <gburt@novell.com>
//   Andrés G. Aragoneses <knocte@gmail.com>
//
// Copyright (C) 2007-2008 Novell, Inc.
// Copyright (C) 2011 Andrés G. Aragoneses
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
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Mono.Unix;

using Hyena.Query;

namespace Banshee.Query
{
    /*public interface IQueryDefines
    {
        QueryOrder [] Orders { get; }
        QueryLimit [] Limits { get; }
        QueryFieldSet FieldSet { get; }
        string GetSqlSort (string key, bool asc);
    }

    public class QueryDefines : IQueryDefines
    {

    }*/

    public static class BansheeQuery
    {
        private static bool asc = true;
        private static bool desc = false;

        public static readonly QueryOrder RandomOrder;
        public static readonly QueryOrder [] Orders;

        public static QueryLimit [] Limits = new QueryLimit [] {
            new QueryLimit ("songs",   I18n.Catalog.GetString ("items"), true),
            new QueryLimit ("minutes", I18n.Catalog.GetString ("minutes"), "CoreTracks.Duration/1000", (int) TimeFactor.Minute),
            new QueryLimit ("hours",   I18n.Catalog.GetString ("hours"), "CoreTracks.Duration/1000", (int) TimeFactor.Hour),
            new QueryLimit ("MB",      I18n.Catalog.GetString ("MB"), "CoreTracks.FileSize", (int) FileSizeFactor.MB),
            new QueryLimit ("GB",      I18n.Catalog.GetString ("GB"), "CoreTracks.FileSize", (int) FileSizeFactor.GB)
        };

#region QueryField Definitions

        public static QueryField ArtistField = new QueryField (
            "artist", "DisplayArtistName",
            I18n.Catalog.GetString ("Artist"), "CoreArtists.NameLowered", true,
            // Translators: These are unique search aliases for "artist". You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("artist"), I18n.Catalog.GetString ("by"), I18n.Catalog.GetString ("artists"),
            "by", "artist", "artists"
        );

        public static QueryField AlbumArtistField = new QueryField (
            "albumartist", "DisplayAlbumArtistName",
            I18n.Catalog.GetString ("Album Artist"), "CoreAlbums.ArtistNameLowered", false,
            // Translators: These are unique search aliases for "album artist". You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("albumartist"), I18n.Catalog.GetString ("compilationartist"),
            "albumartist", "compilationartist"
        );

        // TODO add IsCompilationField

        public static QueryField AlbumField = new QueryField (
            "album", "DisplayAlbumTitle",
            I18n.Catalog.GetString ("Album"), "CoreAlbums.TitleLowered", true,
            // Translators: These are unique search aliases for "album". You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("album"), I18n.Catalog.GetString ("on"), I18n.Catalog.GetString ("from"),
            "on", "album", "from", "albumtitle"
        );

        public static QueryField DiscNumberField = new QueryField (
            "disc", "DiscNumber",
            I18n.Catalog.GetString ("Disc"), "CoreTracks.Disc", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("disc"), I18n.Catalog.GetString ("cd"), I18n.Catalog.GetString ("discnum"),
            "disc", "cd", "discnum"
        );

        public static QueryField DiscCountField = new QueryField (
            "disccount", "DiscCount",
            I18n.Catalog.GetString ("Disc Count"), "CoreTracks.DiscCount", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("discs"), I18n.Catalog.GetString ("cds"),
            "discs", "cds"
        );

        public static QueryField TrackNumberField = new QueryField (
            "track", "TrackNumber",
            // Translators: noun
            I18n.Catalog.GetString ("Track Number"), "CoreTracks.TrackNumber", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            "#", I18n.Catalog.GetString ("track"), I18n.Catalog.GetString ("trackno"), I18n.Catalog.GetString ("tracknum"),
            "track", "trackno", "tracknum"
        );

        public static QueryField TrackCountField = new QueryField (
            "trackcount", "TrackCount",
            // Translators: noun
            I18n.Catalog.GetString ("Track Count"), "CoreTracks.TrackCount", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("tracks"), I18n.Catalog.GetString ("trackcount"),
            "tracks", "trackcount"
        );

        public static QueryField BpmField = new QueryField (
            "bpm", "Bpm",
            I18n.Catalog.GetString ("Beats per Minute"), "CoreTracks.BPM", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("bpm"),
            "bpm"
        );

        public static QueryField BitRateField = new QueryField (
            "bitrate", "BitRate",
            // Translators: noun
            I18n.Catalog.GetString ("Bit Rate"), "CoreTracks.BitRate", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("bitrate"), I18n.Catalog.GetString ("kbs"), I18n.Catalog.GetString ("kps"),
            "bitrate", "kbs", "kps"
        );

        public static QueryField SampleRateField = new QueryField (
            "samplerate", "SampleRate",
            // Translators: noun
            I18n.Catalog.GetString ("Sample Rate"), "CoreTracks.SampleRate", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("samplerate"), I18n.Catalog.GetString ("Hz"),
            "samplerate", "Hz"
        );

        public static QueryField BitsPerSampleField = new QueryField (
            "bitspersample", "BitsPerSample",
            // Translators: noun
            I18n.Catalog.GetString ("Bits Per Sample"), "CoreTracks.BitsPerSample", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("bitspersample"), I18n.Catalog.GetString ("bitdepth"), I18n.Catalog.GetString ("quantization"),
            "bitspersample", "bitdepth", "quantization"
        );

        public static QueryField TitleField = new QueryField (
            "title", "DisplayTrackTitle",
            I18n.Catalog.GetString ("Name"), "CoreTracks.TitleLowered", true,
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("title"), I18n.Catalog.GetString ("titled"), I18n.Catalog.GetString ("name"), I18n.Catalog.GetString ("named"),
            "title", "titled", "name", "named"
        );

        public static QueryField YearField = new QueryField (
            "year", "Year",
            I18n.Catalog.GetString ("Year"), "CoreTracks.Year", typeof(YearQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("year"), I18n.Catalog.GetString ("released"), I18n.Catalog.GetString ("yr"),
            "year", "released", "yr"
        );

        public static QueryField GenreField = new QueryField (
            "genre", "Genre",
            I18n.Catalog.GetString ("Genre"), "CoreTracks.Genre", false,
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("genre"), "genre"
        );

        public static QueryField ComposerField = new QueryField (
            "composer", "Composer",
            I18n.Catalog.GetString ("Composer"), "CoreTracks.Composer", false,
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("composer"), "composer"
        );

        public static QueryField ConductorField = new QueryField (
            "conductor", "Conductor",
            I18n.Catalog.GetString ("Conductor"), "CoreTracks.Conductor", false,
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("conductor"), "conductor"
        );

        public static QueryField GroupingField = new QueryField (
            "grouping", "Grouping",
            I18n.Catalog.GetString ("Grouping"), "CoreTracks.Grouping", false,
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("grouping"), "grouping"
        );

        public static QueryField CommentField = new QueryField (
            "comment", "Comment",
            // Translators: noun
            I18n.Catalog.GetString ("Comment"), "CoreTracks.Comment", false,
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("comment"), "comment"
        );

        public static QueryField LicenseUriField = new QueryField (
            "licenseuri", "LicenseUri",
            // Translators: noun
            I18n.Catalog.GetString ("License"), "CoreTracks.LicenseUri", typeof(ExactStringQueryValue),
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("license"), I18n.Catalog.GetString ("licensed"), I18n.Catalog.GetString ("under"),
            "license", "licensed", "under"
        );

        public static QueryField RatingField = new QueryField (
            "rating", "SavedRating",
            I18n.Catalog.GetString ("Rating"), "CoreTracks.Rating", new Type [] {typeof(RatingQueryValue)},//, typeof(NullQueryValue)},
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("rating"), I18n.Catalog.GetString ("stars"),
            "rating", "stars"
        );

        public static QueryField PlayCountField = new QueryField (
            "playcount", "PlayCount",
            I18n.Catalog.GetString ("Play Count"), "CoreTracks.PlayCount", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("plays"), I18n.Catalog.GetString ("playcount"), I18n.Catalog.GetString ("listens"),
            "plays", "playcount", "numberofplays", "listens"
        );

        public static QueryField SkipCountField = new QueryField (
            "skipcount", "SkipCount",
            I18n.Catalog.GetString ("Skip Count"), "CoreTracks.SkipCount", typeof(NaturalIntegerQueryValue),
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("skips"), I18n.Catalog.GetString ("skipcount"),
            "skips", "skipcount"
        );

        public static QueryField FileSizeField = new QueryField (
            "filesize", "FileSize",
            I18n.Catalog.GetString ("File Size"), "CoreTracks.FileSize", typeof(FileSizeQueryValue),
            // Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("size"), I18n.Catalog.GetString ("filesize"),
            "size", "filesize"
        );

        public static QueryField UriField = new QueryField (
            "uri", "Uri",
            I18n.Catalog.GetString ("File Location"), "CoreTracks.Uri", typeof(ExactUriStringQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("uri"), I18n.Catalog.GetString ("path"), I18n.Catalog.GetString ("file"), I18n.Catalog.GetString ("location"),
            "uri", "path", "file", "location"
        );

        public static QueryField DurationField = new QueryField (
            "duration", "Duration",
            I18n.Catalog.GetString ("Time"), "CoreTracks.Duration", typeof(TimeSpanQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("duration"), I18n.Catalog.GetString ("length"), I18n.Catalog.GetString ("time"),
            "duration", "length", "time"
        );

        public static QueryField MimeTypeField = new QueryField (
            "mimetype", "MimeType",
            I18n.Catalog.GetString ("Mime Type"), "CoreTracks.MimeType", typeof(ExactStringQueryValue),
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("type"), I18n.Catalog.GetString ("mimetype"), I18n.Catalog.GetString ("format"), I18n.Catalog.GetString ("ext"),
            "type", "mimetype", "format", "ext", "mime"
        );

        public static QueryField LastPlayedField = new QueryField (
            "lastplayed", "LastPlayed",
            I18n.Catalog.GetString ("Last Played"), "CoreTracks.LastPlayedStamp", new Type [] {typeof(RelativeTimeSpanQueryValue), typeof(DateQueryValue)},
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("lastplayed"), I18n.Catalog.GetString ("played"), I18n.Catalog.GetString ("playedon"),
            "lastplayed", "played", "playedon"
        );

        public static QueryField LastSkippedField = new QueryField (
            "lastskipped", "LastSkipped",
            I18n.Catalog.GetString ("Last Skipped"), "CoreTracks.LastSkippedStamp", new Type [] {typeof(RelativeTimeSpanQueryValue), typeof(DateQueryValue)},
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("lastskipped"), I18n.Catalog.GetString ("skipped"), I18n.Catalog.GetString ("skippedon"),
            "lastskipped", "skipped", "skippedon"
        );

        public static QueryField DateAddedField = new QueryField (
            "added", "DateAdded",
            I18n.Catalog.GetString ("Date Added"), "CoreTracks.DateAddedStamp", new Type [] {typeof(RelativeTimeSpanQueryValue), typeof(DateQueryValue)},
            // Translators: These are unique search fields. You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("added"), I18n.Catalog.GetString ("imported"), I18n.Catalog.GetString ("addedon"), I18n.Catalog.GetString ("dateadded"), I18n.Catalog.GetString ("importedon"),
            "added", "imported", "addedon", "dateadded", "importedon"
        );

        public static QueryField PlaylistField = new QueryField (
            "playlistid", null, I18n.Catalog.GetString ("Playlist"),
            "CoreTracks.TrackID {2} IN (SELECT TrackID FROM CorePlaylistEntries WHERE PlaylistID = {1})", typeof(PlaylistQueryValue),
            "playlistid", "playlist"
        );

        public static QueryField SmartPlaylistField = new QueryField (
            "smartplaylistid", null, I18n.Catalog.GetString ("Smart Playlist"),
            "CoreTracks.TrackID {2} IN (SELECT TrackID FROM CoreSmartPlaylistEntries WHERE SmartPlaylistID = {1})", typeof(SmartPlaylistQueryValue),
            "smartplaylistid", "smartplaylist"
        );

        public static QueryField ScoreField = new QueryField (
            "score", "Score",
            I18n.Catalog.GetString ("Score"), "CoreTracks.Score", typeof(IntegerQueryValue),
            //Translators: These are unique search fields (and nouns). You can use CSV for synonyms. Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("score"),
            "score"
        );

        public static QueryField PlaybackErrorField = new QueryField (
            "playbackerror", "PlaybackError",
            I18n.Catalog.GetString ("Playback Error"), "CoreTracks.LastStreamError", typeof(PlaybackErrorQueryValue),
            //Translators: These are unique search fields (and nouns). Please, no spaces. Blank ok.
            I18n.Catalog.GetString ("playbackerror"),
            "playbackerror", "error"
        );

#endregion

        public static QueryFieldSet FieldSet = new QueryFieldSet (
            ArtistField, AlbumField, AlbumArtistField, TitleField, TrackNumberField, TrackCountField, DiscNumberField, DiscCountField,
            YearField, GenreField, ComposerField, ConductorField, GroupingField, CommentField, LicenseUriField, RatingField, PlayCountField,
            SkipCountField, FileSizeField, UriField, DurationField, MimeTypeField, LastPlayedField, LastSkippedField,
            BpmField, BitRateField, SampleRateField, BitsPerSampleField, DateAddedField, PlaylistField, SmartPlaylistField, ScoreField, PlaybackErrorField
        );

        // Type Initializer
        static BansheeQuery ()
        {
            // Translators: noun
            BpmField.ShortLabel         = I18n.Catalog.GetString ("BPM");
            SkipCountField.ShortLabel   = I18n.Catalog.GetString ("Skips");
            PlayCountField.ShortLabel   = I18n.Catalog.GetString ("Plays");

            default_sort = String.Format (default_sort_template, "");
            default_sort_by_year = String.Format (default_sort_template, YearField.Column + " ASC, ");

            RandomOrder = CreateRandomQueryOrder ();

            Orders = new QueryOrder [] {
                RandomOrder,
                CreateQueryOrder ("Album",      asc,  I18n.Catalog.GetString ("Album"), AlbumField),
                CreateQueryOrder ("Artist",     asc,  I18n.Catalog.GetString ("Artist"), ArtistField),
                // Translators: noun
                CreateQueryOrder ("Title",      asc,  I18n.Catalog.GetString ("Name"), TitleField),
                CreateQueryOrder ("Genre",      asc,  I18n.Catalog.GetString ("Genre"), GenreField),
                null,
                CreateQueryOrder ("Rating",     desc, I18n.Catalog.GetString ("Highest Rating"), RatingField),
                CreateQueryOrder ("Rating",     asc,  I18n.Catalog.GetString ("Lowest Rating"), RatingField),
                null,
                CreateQueryOrder ("Score",      desc, I18n.Catalog.GetString ("Highest Score"), ScoreField),
                CreateQueryOrder ("Score",      asc,  I18n.Catalog.GetString ("Lowest Score"), ScoreField),
                null,
                CreateQueryOrder ("PlayCount",  desc, I18n.Catalog.GetString ("Most Often Played"), PlayCountField),
                CreateQueryOrder ("PlayCount",  asc,  I18n.Catalog.GetString ("Least Often Played"), PlayCountField),
                null,
                CreateQueryOrder ("LastPlayedStamp", desc, I18n.Catalog.GetString ("Most Recently Played"), LastPlayedField),
                CreateQueryOrder ("LastPlayedStamp", asc,  I18n.Catalog.GetString ("Least Recently Played"), LastPlayedField),
                null,
                CreateQueryOrder ("DateAddedStamp",  desc, I18n.Catalog.GetString ("Most Recently Added"), DateAddedField),
                CreateQueryOrder ("DateAddedStamp",  asc,  I18n.Catalog.GetString ("Least Recently Added"), DateAddedField)
            };
        }

        private const string default_sort_template = @"CoreAlbums.ArtistNameSortKey ASC, {0}CoreAlbums.TitleSortKey ASC, CoreTracks.Disc ASC, CoreTracks.TrackNumber ASC";
        private static readonly string default_sort;
        private static readonly string default_sort_by_year;

        public static string GetSort (QueryField key)
        {
            return GetSort (key, false);
        }

        public static string GetRandomSort ()
        {
            return "RANDOM ()";
        }

        public static string GetSort (QueryField field, bool asc)
        {
            if (field == null) {
                throw new ArgumentNullException ("field");
            }

            bool sort_by_year = Banshee.Configuration.Schema.LibrarySchema.SortByAlbumYear.Get ();
            string sort_by_year_part = sort_by_year ? YearField.Column + " ASC," : "";
            string sort = sort_by_year ? default_sort_by_year : default_sort;

            string ascDesc = asc ? "ASC" : "DESC";
            string sort_query = null;

            if (field.Equals (GroupingField) ||
                field.Equals (TrackNumberField)) {
                sort_query = String.Format (@"
                    CoreAlbums.ArtistNameSortKey ASC,
                    {0}
                    CoreAlbums.TitleSortKey ASC,
                    {1} ASC,
                    {2} {3}",
                    sort_by_year_part, DiscNumberField.Column, TrackNumberField.Column, ascDesc);
            }

            else if (field.Equals (AlbumArtistField)) {

                sort_query = String.Format (@"
                    CoreAlbums.ArtistNameSortKey {0},
                    {1}
                    CoreAlbums.TitleSortKey ASC,
                    {2} ASC,
                    {3} ASC",
                    ascDesc, sort_by_year_part, DiscNumberField.Column, TrackNumberField.Column);
            }

            else if (field.Equals (ArtistField)) {

                sort_query = String.Format (@"
                    CoreArtists.NameSortKey {0},
                    {1}
                    CoreAlbums.TitleSortKey ASC,
                    {2} ASC,
                    {3} ASC",
                    ascDesc, sort_by_year_part, DiscNumberField.Column, TrackNumberField.Column);
            }

            else if (field.Equals (AlbumField)) {
                sort_query = String.Format (@"
                    CoreAlbums.TitleSortKey {0},
                    {1}
                    {2} ASC,
                    {3} ASC",
                    ascDesc, sort_by_year_part, DiscNumberField.Column, TrackNumberField.Column);
            }

            else if (field.Equals (TitleField)) {
                sort_query = String.Format (@"
                    CoreTracks.TitleSortKey {0},
                    CoreAlbums.ArtistNameSortKey ASC,
                    {1}
                    CoreAlbums.TitleSortKey ASC", ascDesc, sort_by_year_part);
            }

            else if (field.Equals (DiscNumberField)) {
                sort_query = String.Format (@"
                    CoreAlbums.ArtistNameSortKey ASC,
                    {0}
                    CoreAlbums.TitleSortKey ASC,
                    {1} {2},
                    {3} ASC",
                    sort_by_year_part, DiscNumberField.Column, ascDesc, TrackNumberField.Column);
            }

            else if (field.Equals (ScoreField)) {
                sort_query = String.Format (@"
                    {0} {1},
                    {2} {1}, {3}",
                    field.Column, ascDesc, PlayCountField.Column, sort);
            }

            else if (field.Equals (ComposerField) ||
                     field.Equals (GenreField)    ||
                     field.Equals (ComposerField) ||
                     field.Equals (ConductorField) ||
                     field.Equals (CommentField)) {

                sort_query = String.Format (
                    "HYENA_COLLATION_KEY({0}) {1}, {2}",
                    field.Column, ascDesc, sort
                );
            }

            else if (field.Equals (LastPlayedField)    ||
                     field.Equals (LastSkippedField)   ||
                     field.Equals (DateAddedField)     ||
                     field.Equals (YearField)          ||
                     field.Equals (BitRateField)       ||
                     field.Equals (SampleRateField)    ||
                     field.Equals (BitsPerSampleField) ||
                     field.Equals (BpmField)           ||
                     field.Equals (TrackCountField)    ||
                     field.Equals (DiscCountField)     ||
                     field.Equals (DurationField)      ||
                     field.Equals (RatingField)        ||
                     field.Equals (PlayCountField)     ||
                     field.Equals (SkipCountField)     ||
                     field.Equals (FileSizeField)      ||
                     field.Equals (UriField)           ||
                     field.Equals (MimeTypeField)      ||
                     field.Equals (LicenseUriField)) {
                sort_query = String.Format (
                    "{0} {1}, {2}",
                    field.Column, ascDesc, sort
                );
            }

            else {
                Hyena.Log.ErrorFormat ("Unknown query field passed in! {0} not recognized", field.Name);
            }

            return sort_query;
        }

        private static QueryOrder CreateQueryOrder (string name, bool asc, string label, QueryField field)
        {
            return new QueryOrder (CreateOrderName (name, asc), label, GetSort (field, asc), field, asc);
        }

        private static QueryOrder CreateRandomQueryOrder ()
        {
            return new QueryOrder (CreateOrderName ("Random", true), I18n.Catalog.GetString ("Random"), GetRandomSort (), null, true);
        }

        public static QueryLimit FindLimit (string name)
        {
            foreach (QueryLimit limit in Limits) {
                if (limit.Name == name)
                    return limit;
            }
            return null;
        }

        public static QueryOrder FindOrder (QueryField field, bool asc)
        {
            return Orders.FirstOrDefault (o => o != null && o.Field == field && o.Ascending == asc);
        }

        public static QueryOrder FindOrder (string name, bool asc)
        {
            return FindOrder (CreateOrderName (name, asc));
        }

        public static QueryOrder FindOrder (string name)
        {
            foreach (QueryOrder order in Orders) {
                if (order != null && order.Name == name) {
                    return order;
                }
            }
            return null;
        }

        private static string CreateOrderName (string name, bool asc)
        {
            return String.Format ("{0}-{1}", name, asc ? "ASC" : "DESC");
        }
    }
}
