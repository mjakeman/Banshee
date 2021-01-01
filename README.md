# Banshee
A music player for GNOME. This is a port of Banshee to .NET 5 and
Gtk 3 (later Gtk 4) for modern systems.

## Directory Structure
```
The following are the main directories of note:
 - ext: Git submodules for various dependent libraries
 - src: Project source code
 - src/Hyena: A fork of Hyena as a submodule
```

## Status
The following projects build:
**Core:**
 - `src/Core/Banshee.Core`
 - `src/Core/Banshee.CollectionIndexer`
 - `src/Core/Banshee.Services`

**Libraries:**
 - `src/Libraries/Lastfm`
 - `src/Libraries/Musicbrainz`
 - `src/Libraries/Mono.Media`

**Backends:**
 - `src/Backends/Banshee.GStreamerSharp`

**Hyena (Submodule):**
 - [`src/Hyena`](https://github.com/firox263/Hyena)

**Clients:**
 - `src/Clients/Beroe`
 - `src/Clients/Booter`
 - `src/Clients/Demo`
 - `src/Clients/Tinyshee`



There is a fully working demo player with real audio playback
located at `src/Clients/Tinyshee` (Tinyshee = Banshee, but smaller).
See below for instructions on how to build.

Additionally, I added `src/Hyena/Hyena.Glue` to provide stub code for
certain `Mono` classes/functions with no direct replacement.

## Build
You're on your own :)

### Prerequisites:
1. Have Gtk and GStreamer installed (for windows, preferably mingw) and in your PATH
2. Have .NET 5 SDK installed
   
#### Building
3. Initialise submodules recursively
4. gir.core is located under `src/Hyena/ext/gir.core` - build that first
5. Hyena is located under `src/Hyena`. This is a separate solution, must be **built separately** (i.e. building banshee will not rebuild any Hyena projects, be aware when working on Hyena directly)
6. Build `ext/dbus-sharp` **with branch netcore** - Solution file should work

A typical build will look like this:
```bash
# Checkout Source (Init Recursively)
$ git clone https://github.com/firox263/Banshee
$ git submodule update --init --recursive

# Build Gir.Core
$ cd src/Hyena/ext/gir.core/Build
Build$ dotnet run 

# Build Hyena
Build$ cd ../../../
Hyena$ dotnet build

# Build Banshee
Hyena$ cd ../../
$ dotnet build

# Run Tinyshee Demo
$ dotnet run --project src/Clients/Tinyshee 
```

You will need `gtk3`, `gstreamer`, `gst-plugins-good`, and `gst-plugins-bad`
installed to run the demo program.

### Banshee
You can now build any of the projects listed under [Status](#status) above. Use the dotnet cli where possible to build. The solution file has some projects added for easy VS/Rider integration.

## External Dependencies
Banshee depends on `Hyena.Addins` and `dbus-sharp`/`dbus-sharp-glib` to
build. `Hyena.Addins` is a port of `Mono.Addins` to .NET Standard 2.1, and
works for the most part. DBus is completely non-functional, so `DBusConnection.Enabled`
is hardcoded to `false`.

## TODO
I am currently working on:
 - Finish Porting `src/Backends/Banshee.GStreamerSharp` (for media playback)
    - Audio playback works as intended, but there are a few remaining issues with
      closures/delegates in gir.core that need to be resolved before it can work
      properly. Video, DVD, CDDA, etc are not yet implemented.
 - Porting `Hyena.Gui` and `Hyena.Widgets` to gir.core
 - Porting `src/Core/Banshee.Widgets`
 - Porting `src/Core/Banshee.ThickClient`
 
## Backlog
Things that need to be done at some point:
 - Porting `src/Clients/Nereid` (the main GUI)
 - Port `dbus-sharp` to GDBus (provided by gir.core)
 - Get rid of `dbus-sharp-glib`
 - Re-enable DBus support so the indexer can work (needs design)
 - Figure out what to do with `src/Core/Banshee.WebBrowser`
     - Remove entirely
     - Port to Webkit2Gtk (Doesn't run on Windows?)
     - Embed some other browser engine? (e.g. Servo)

## Original Readme
The original README can be seen below at the time of forking:

```
Banshee is a multimedia management and playback application for GNOME, with
support for OS X and Windows as well.

Play your music and videos. Stay entertained and up to date with podcasts
and video podcasts.  Sync your Android, iPod, and other devices. We think
you'll love the new Banshee!

         Download  http://banshee.fm/download/
         Get Help  http://banshee.fm/support/
     Mailing List  http://mail.gnome.org/mailman/listinfo/banshee-list
              IRC  #banshee on irc.gnome.org (GIMPNet)
             Bugs  http://bugzilla.gnome.org/browse.cgi?product=banshee
              Git  http://git.gnome.org/browse/banshee/
       Contribute  http://banshee.fm/contribute/
 Release Schedule  http://banshee.fm/about/calendar/
             More  http://banshee.fm/
 
=== Dependencies ===

See http://banshee.fm/download/development/ for instructions on building
on various operating systems/distros.  The listed versions are the minimum
acceptable; higher versions are fine.

- Required
  * Mono                3.2.8
  * SQlite              3.4
  * GStreamerSharp      0.99.0
  * GStreamer           1.0.0
  * Gtk#                2.99.1
  * GLib                2.22
  * dbus-sharp          0.8.1
  * dbus-sharp-glib     0.6
  * Mono.Addins         0.6.2
  * TagLib#             2.0.3.7

- Required unless you pass some --disable flags to configure
  * libmtp              1.1.0
  * mono-zeroconf       0.8.0
  * boo                 0.8.1
  * gio-sharp           2.99.1
  * gdata-sharp-youtube 1.4
  * webkitgtk-3.0       1.2.2
  * libsoup             2.42
  * gudev-sharp         3.0
  * gkeyfile-sharp      0.1
  * libgpod-sharp       0.7.95
  * mono-upnp           0.1
  * dbus-glib           0.80

- Required at run-time:
  * udev
  * media-player-info
  * Brasero >= 0.8.1
  * Avahi
  * gst-plugins-bad (providing the bpmdetect GStreamer plugin)

=== Packaging Notes ===

* Follow NEWS for changes to dependencies and other packager instructions

* Pass --with-vendor-build-id="foo" to configure, where foo is like one of:

    openSUSE 11.1
    home:gabrielburt:branches:Banshee / openSUSE_Factory
    Banshee Team Daily PPA / Ubuntu 11.04
```
