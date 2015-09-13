using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualityControl.Util
{
    public class Dumper
    {
        private static List<string> _basicTypes;

        public static List<string> BasicTypes
        {
            get
            {
                return _basicTypes ?? (new List<string>
                {
                    "Int64",
                    "String",
                    "Boolean"
                });
            }
        }

        public static void Dump<A, B>(A src, B dest, bool isnoreId = true, List<string> excepts = null)
             where B : new()
        {
            if (src == null)
            {
                return;
            }
            if (dest == null)
            {
                dest = new B();
            }
            var aps = typeof(A).GetProperties();
            var bps = typeof(B).GetProperties();

            foreach (var ap in aps)
            {
                try
                {
                    if (excepts == null || !excepts.Contains(ap.Name))
                    {
                        if (BasicTypes.Contains(ap.PropertyType.Name) && ap.Name != "Id")
                        {
                            var bp = bps.First(i => i.Name == ap.Name);
                            bp.SetValue(dest, ap.GetValue(src));
                        }

                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}