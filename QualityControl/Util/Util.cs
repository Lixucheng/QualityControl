using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualityControl.Util
{
    public class Util
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
                    "Boolean",
                    "DateTime"
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
                        if (ap.DeclaringType.BaseType.Name == "Enum" || BasicTypes.Contains(ap.PropertyType.Name) && (isnoreId ? ap.Name != "Id" : true))
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

        public static bool Equal<One, Another>(One one, Another another, bool isnoreId = true, List<string> excepts = null)
        {
            var aps = typeof(One).GetProperties();
            var bps = typeof(Another).GetProperties();

            foreach (var ap in aps)
            {
                try
                {
                    
                    if (excepts == null || !excepts.Contains(ap.Name))
                    {
                        if (BasicTypes.Contains(ap.PropertyType.Name) && (isnoreId ? ap.Name != "Id" : true))
                        {
                            var bp = bps.First(i => i.Name == ap.Name);
                            var al = ap.GetValue(one);
                            var bl = bp.GetValue(another);
                            if ( (al != null && bl != null) && al.ToString() != bl.ToString())
                            {
                                return false;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public static void SetForeignKeyNull<One>(One data, List<string> excepts = null)
        {
            var aps = typeof(One).GetProperties();

            foreach (var ap in aps)
            {
                try
                {

                    if (excepts == null || !excepts.Contains(ap.Name))
                    {
                        if (!(BasicTypes.Contains(ap.PropertyType.Name) || ap.PropertyType.BaseType.Name == "Enum")) {
                            ap.SetValue(data, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

    }
}