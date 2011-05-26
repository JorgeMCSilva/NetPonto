using System;
using System.IO;
using System.Web.Util;
using Microsoft.Security.Application;


namespace NetPonto.Common.HTMLEncoder
{

    public static class XSSHelper
    {
        public static string h(this HtmlHelper helper, string input)
        {
            return AntiXss.HtmlEncode(input);
        }
        public static string Sanitize(this HtmlHelper helper, string input)
        {
            return AntiXss.GetSafeHtml(input);
        }
        /// <summary>
        /// Encodes Javascript
        /// </summary>
        public static string hscript(this HtmlHelper helper, string input)
        {
            return AntiXss.JavaScriptEncode(input);
        }
    }

    public class XssEncoder : HttpEncoder
    {
        //public XssEncoder() { }

        protected override void HtmlEncode(string value, TextWriter output)
        {
            output.Write(Encoder.HtmlEncode(value));
        }

        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(Encoder.HtmlAttributeEncode(value));
        }

        //protected override void HtmlDecode(string value, TextWriter output)
        //{
        //    base.HtmlDecode(value, output);
        //}

        protected override byte[] UrlEncode(byte[] bytes, int offset, int count)
        {
            //Can't call AntiXss library because the AntiXss library works with Unicode strings.
            //This override works at a lower level with just a stream of bytes, independent of 
            //the original encoding.

            //
            //Internal ASP.NET implementation reproduced below.
            //
            int cSpaces = 0;
            int cUnsafe = 0;

            // count them first
            for (int i = 0; i < count; i++)
            {
                var ch = (char)bytes[offset + i];

                if (ch == ' ')
                    cSpaces++;
                else if (!IsUrlSafeChar(ch))
                    cUnsafe++;
            }

            // nothing to expand?
            if (cSpaces == 0 && cUnsafe == 0)
                return bytes;

            // expand not 'safe' characters into %XX, spaces to +s
            var expandedBytes = new byte[count + cUnsafe * 2];
            int pos = 0;

            for (int i = 0; i < count; i++)
            {
                byte b = bytes[offset + i];
                var ch = (char)b;

                if (IsUrlSafeChar(ch))
                {
                    expandedBytes[pos++] = b;
                }
                else if (ch == ' ')
                {
                    expandedBytes[pos++] = (byte)'+';
                }
                else
                {
                    expandedBytes[pos++] = (byte)'%';
                    expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
            }

            return expandedBytes;


        }

        protected override string UrlPathEncode(string value)
        {
            //AntiXss.UrlEncode is too "pessimistic" for how ASP.NET uses UrlPathEncode

            //ASP.NET's UrlPathEncode splits the query-string off, and then Url encodes
            //the Url path portion, encoding any parts that are non-ASCII, or that
            //are <= 0x20 or >=0x7F.

            //Additionally, it is expected that:
            //                       UrPathEncode(string) == UrlPathEncode(UrlPathEncode(string))
            //which is not the case for UrlEncode.

            //The Url needs to be separated into individual path segments, each of which
            //can then be Url encoded.
            string[] parts = value.Split("?".ToCharArray());
            string originalPath = parts[0];

            string originalQueryString = null;
            if (parts.Length == 2)
                originalQueryString = "?" + parts[1];

            string[] pathSegments = originalPath.Split("/".ToCharArray());

            for (int i = 0; i < pathSegments.Length; i++)
            {
                pathSegments[i] = Encoder.UrlEncode(pathSegments[i]);  //this step is currently too aggressive
            }

            return String.Join("/", pathSegments) + originalQueryString;
        }

        private static bool IsUrlSafeChar(char ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9')
                return true;

            switch (ch)
            {

                //These are the characters ASP.NET considers safe by default
                //case '-':
                //case '_':
                //case '.':
                //case '!':
                //case '*':
                //case '\'':
                //case '(':
                //case ')':
                //    return true;

                //Modified list based on what AntiXss library allows from the ASCII character set
                case '-':
                case '_':
                case '.':
                    return true;
            }

            return false;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
                return (char)(n + (int)'0');
            else
                return (char)(n - 10 + (int)'a');
        }

    }
}
