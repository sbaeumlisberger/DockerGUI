using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGUI.Utils
{
    public static class StringUtil
    {
        public static string ReplacePostfix(this string source, string postfix, string replacment)
        {
            if (source.EndsWith(postfix))
            {
                return source.Substring(0, source.Length - postfix.Length) + replacment;
            }
            return source;
        }

        public static string RemovePostfix(this string source, string postfix)
        {
            return source.ReplacePostfix(postfix, "");
        }
    }
}
