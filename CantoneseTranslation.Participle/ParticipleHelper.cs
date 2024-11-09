using JiebaNet.Segmenter.PosSeg;
using System.Collections.Generic;
using System.Linq;

namespace CantoneseTranslation.Participle
{
    public class ParticipleHelper
    {
        public static List<Pair> Cut(string content)
        {
            var jbs = new PosSegmenter();      
            
            return jbs.Cut(content).ToList();            
        }       

        public static bool IsVerbWord(string word, bool vague = false)
        {
            return HasFlag(word, "v", vague);
        }

        public static bool IsNounWord(string word, bool vague=false)
        {
            return HasFlag(word, "n", vague);
        }

        public static bool IsAdjective(string word, bool vague = false)
        {
            return HasFlag(word, "a", vague);
        }       

        private static bool HasFlag(string word, string flag, bool vague = false)
        {
            var jbs = new PosSegmenter();

            var flags = jbs.Cut(word).Select(item=>item.Flag);

            if (flags != null)
            {
                if(vague)
                {
                    if((flags.FirstOrDefault().StartsWith(flag) || flags.LastOrDefault().EndsWith(flag)))
                    {
                        return true;
                    }
                }
                else
                {
                    return flags.All(item => item == flag);
                }
                return true;
            }

            return false;
        }

        public static string GetWordFlag(string word)
        {
            return new PosSegmenter().Cut(word).FirstOrDefault()?.Flag;
        }

        public static bool IsNounOrPronoun(string word)
        {
            string flag = GetWordFlag(word);

            if (flag == "r" || flag == "n" || flag == "nr")
            {
                return true;
            }

            return false;
        }
    }
}
