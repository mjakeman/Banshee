//
// ExceptionDialog.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//   Matthew Jakeman <mjakeman26@outlook.co.nz>
//
// Copyright (C) 2005-2007 Novell, Inc.
//               2020 Matthew Jakeman
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
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Gtk;

namespace Hyena.Gui.Dialogs
{
    public class ExceptionDialog : Dialog
    {
        private string debugInfo;

        // Widgets
        private Box vbox;

        public ExceptionDialog(Exception e)
        {
            debugInfo = BuildExceptionMessage(e);

            Resizable = false;
            SetBorderWidth(5);

            // Translators: {0} is substituted with the application name
            Title = String.Format(Catalog.GetString("{0} Encountered a Fatal Error"),
                                  ApplicationContext.ApplicationName);

            // TODO: Add Accelerators
            // TODO: Escape Markup Functions

            // Create GUI
            vbox = (Box)GetContentArea();
            vbox.Spacing = 12;

            // Header
            var hbox = new Box(Orientation.horizontal, 12);
            vbox.PackStart(hbox, false, false, 0);
            
            var image = Image.NewFromIconName("dialog-error", IconSize.dialog);
            image.VAlign = Align.start;
            hbox.PackStart(image, false, false, 0);

            Box label_vbox = new Box(Orientation.vertical, 12);
            hbox.PackStart(label_vbox, true, true, 0);

            Label label = new Label(String.Format("<b><big>{0}</big></b>", Title));
            label.UseMarkup = true;
            label.Justify = Justification.left;
            label.Wrap = true;
            label.XAlign = 0;
            label_vbox.PackStart(label, false, false, 0);

            label = new Label(e.Message);

            label.UseMarkup = true;
            label.UseUnderline = false;
            label.Justify = Justification.left;
            label.Wrap = true;
            label.Selectable = true;
            label.XAlign = 0;
            label_vbox.PackStart(label, false, false, 0);

            Label details_label = new Label(String.Format("<b>{0}</b>",
                Catalog.GetString("Error Details")));
            details_label.UseMarkup = true;

            Expander details_expander = new Expander("Details");
            details_expander.LabelWidget = details_label;
            details_expander.ResizeToplevel = true;
            label_vbox.PackStart(details_expander, true, true, 0);

            var scrolledWindow = new ScrolledWindow();
            var textView = new TextView();
            scrolledWindow.Add(textView);

            scrolledWindow.SetMinContentWidth(650);
            scrolledWindow.SetMinContentHeight(450);
            
            textView.Editable = false;
            textView.Buffer.Text = debugInfo;

            details_expander.Add(scrolledWindow);
            
            ShowAll();

            // TODO: Button
            // AddButton(Stock.Close, ResponseType.Close, true);
        }

        private string BuildExceptionMessage(Exception e)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();

            msg.Append(Catalog.GetString("An unhandled exception was thrown: "));

            Stack<Exception> exception_chain = new Stack<Exception> ();

            while (e != null) {
                exception_chain.Push (e);
                e = e.InnerException;
            }

            while (exception_chain.Count > 0) {
                e = exception_chain.Pop ();
                msg.AppendFormat ("{0}\n\n{1}\n", e.Message, e.StackTrace);
            };

            msg.Append("\n");
            msg.AppendFormat(".NET Version: {0}\n", Environment.Version);
            msg.AppendFormat("OS Version: {0}\n", Environment.OSVersion);
            msg.Append("\nAssembly Version Information:\n\n");

            foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) {
                AssemblyName name = asm.GetName();
                msg.AppendFormat("{0} ({1})\n", name.Name, name.Version);
            }

            // if (Environment.OSVersion.Platform != PlatformID.Unix) {
            //     return msg.ToString();
            // }

            // TODO: Print helpful information relating to the user's
            // distribution if we are on Linux.

            return msg.ToString();
        }
    }
}