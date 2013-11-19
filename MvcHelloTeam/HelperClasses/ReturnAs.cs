using System;

namespace MemberCenter20NS
{
    public static class ReturnAs
    {
        public static bool Bool(string s)
        {
            var result = false;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    s = s.ToLower().Trim();

                    switch (s)
                    {
                        case "1":
                        case "true":
                        case "yes":
                            result = true;
                            break;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
        public static string String(object o)
        {
            var result = string.Empty;

            if (o != null)
                result = o.ToString();

            return result;
        }

        public static int Int(string s)
        {
            var result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToInt32(s);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }
    }
}