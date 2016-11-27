using System;
using System.Text;

namespace MailMessaging.Plain.Core.Text
{
    public static class ModifiedUTF7
    {
        public static string Decode(string text)
        {
            int e = 0, a = text.IndexOf('&');
            if (a < 0)
                return text;

            var sb = new StringBuilder();
            while (a >= 0)
            {
                sb.Append(text.Substring(e, a - e));
                e = text.IndexOf('-', ++a);
                if(e == a)
                {
                    sb.Append("&");
                }
                else if(e > 0)
                {
                    var part = ("+" + text.Substring(a, e - a + 1)).Replace(',', '/');
                    sb.Append(Encoding.UTF7.GetString(Encoding.UTF8.GetBytes(part)));
                }
                else
                {
                    throw new Exception("bad imap-utf-7 encoding");
                }

                a = text.IndexOf('&', ++e);
            }
            sb.Append(text.Substring(e));
            return sb.ToString();
        }

        public static string Encode(string text)
        {
            var justAscii = true;
            for (var i = 0; justAscii && i < text.Length; ++i)
                justAscii = IsPrintable(text[i]) && text[i] != '&';

            if (justAscii)
                return text;

            var sb = new StringBuilder();
            var code = new StringBuilder();
            foreach(var c in text)
            {
                if (IsPrintable(c))
                {
                    if (code.Length > 0)
                    {
                        sb.Append(Encoding.UTF8.GetString(Encoding.UTF7.GetBytes(code.ToString())).Replace('/', ',').Replace('+', '&'));
                        code = new StringBuilder();
                    }

                    if (c == '&')
                        sb.Append("&-");
                    else
                        sb.Append(c);
                }
                else
                    code.Append(c);
            }
            return sb.ToString();
        }

        private static bool IsPrintable(char c)
        {
            return c >= '\x20' && c <= '\x25' || c >= '\x27' && c <= '\x7e';
        }
    }
}
