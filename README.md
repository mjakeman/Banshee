# Banshee
A music player for GNOME. This is an experiment to see if part or all of
Banshee can be ported to .NET Core and Gtk3/4 for modern systems.

## Directory Structure
```
The following are the main directories of note:
 - ext: In tree copy of gir.core (Bindings for Gtk, Gst, etc)
 - src: Project source code
 - src/Hyena: A fork of Hyena being maintained in-tree
```

## Status
TODO

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