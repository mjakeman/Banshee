//
// YearInfo.cs
//
// Author:
//   Frank Ziegler <funtastix@googlemail.com>
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
using System.Text.RegularExpressions;
//using Mono.Unix;

using Hyena;
using Banshee.Base;
using Banshee.I18n;

namespace Banshee.Collection
{
    public class YearInfo : CacheableItem
    {
        public static readonly string UnknownYearTitle = I18n.Catalog.GetString ("Unknown Year");

        private int year;

        public YearInfo ()
        {
        }

        private string name;
        public virtual string Name {
            get {
                if (String.IsNullOrEmpty (name)) {
                    return Year == 0 ? UnknownYearTitle : Year.ToString ();
                } else {
                    return name;
                }
            }
            set { name = value; }
        }

        public virtual int Year {
            get { return year; }
            set { year = value; }
        }

    }
}
