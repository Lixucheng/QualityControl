using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityControl.Models
{
    public  class ConvertLevel: Singleton
    {
        public Db.CheckLevel GetLevelByCount(int c)
        {
            var level = Db.Levels.FirstOrDefault(e => e.Max >= c && e.Min <= c);
            return level;
        }

        public String GetLevel(int c,  int s2)
        {
            var level = Db.Levels.FirstOrDefault(e => e.Max >= c && e.Min <= c);           
                switch (s2)
                {
                    case 1:
                        return level.S1;
                    case 2:
                        return level.S2;
                    case 3:
                        return level.S3;
                    case 4:
                        return level.S4;
                case 11:
                        return level.I;
                case 12:
                        return level.II;
                case 13:
                        return level.III;
                default: return null;

                }
            
        }

        public List<Enumlevel2> GetLevel2(int c, int s1)
        {
            var level = Db.Levels.FirstOrDefault(e => e.Max >= c && e.Min <= c);
            var list = new List<Enumlevel2>();
            switch (s1)
            {
                case 0:
                {
                    list.Add(Enumlevel2.s1);
                    list.Add(Enumlevel2.s2);
                    list.Add(Enumlevel2.s3);
                    list.Add(Enumlevel2.s4);
                    return list;
                }
                case 1:
                {
                    list.Add(Enumlevel2.I);
                    list.Add(Enumlevel2.II);
                    list.Add(Enumlevel2.III);
                    return list;
                }

                default: return null;

            }
        }
        public enum EnumLevel
        {
            特殊检验水平=0,
            一般检验水平=1
        }

        public enum Enumlevel2
        {
            s1=1,
            s2=3,
            s3=3,
            s4=4,
            I=11,
            II=12,
            III=13
        }
    }
}
