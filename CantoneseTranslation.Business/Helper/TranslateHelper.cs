using CantoneseTranslation.Business;
using CantoneseTranslation.Business.Model;
using CantoneseTranslation.Model;
using CantoneseTranslation.Participle;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace CantoneseLearning.Business
{
    public class TranslateHelper
    {
        internal static IEnumerable<V_Mandarin2Cantonese> Mandarin2Cantoneses { get; private set; }
        internal static IEnumerable<V_CantoneseExample> CantoneseExamples { get; private set; }
        internal static IEnumerable<CantoneseSynonym> CantoneseSynonyms { get; set; }
        internal static IEnumerable<CantoneseSentencePattern> CantoneseSentencePatterns { get; set; }

        internal static string RegexPlaceHolder = "...";
        internal static char[] ItemsSplitors = [',', '，'];

        internal static async Task<TranslationResult> Translate(TranslateType translateType, string content)
        {
            content = ChineseConverterHelper.TraditionalToSimplified(content);

            TranslationResult result = new TranslationResult();

            if (Mandarin2Cantoneses == null)
            {
                Mandarin2Cantoneses = await DataProcessor.GetVMandarin2Cantoneses();
            }

            if (CantoneseExamples == null)
            {
                CantoneseExamples = await DataProcessor.GetVCantoneseExamples();
            }

            if (CantoneseSynonyms == null)
            {
                CantoneseSynonyms = await DataProcessor.GetCantoneseSynonyms();
            }

            if (CantoneseSentencePatterns == null)
            {
                CantoneseSentencePatterns = await DataProcessor.GetCantoneseSentencePatterns();
            }

            Action<V_Mandarin2Cantonese> setResult = (res) =>
            {
                if (translateType == TranslateType.Mandarin2Cantonese)
                {
                    var cantoneses = GetItemsBySynonym(res.Cantonese, res.CantoneseSynonym);

                    cantoneses = ExcludeItems(translateType, cantoneses, res.Exclusion);

                    result.Contents.AddRange(cantoneses);

                    var examples = CantoneseExamples.Where(item => item.Mandarin2CantoneseId == res.Id).OrderBy(item => item.Priority);

                    if (examples != null && examples.Count() > 0)
                    {
                        result.Examples.AddRange(examples.Select(item => item.Example));
                    }

                    result.PatternNotes = res.PatternNotes;
                }
                else
                {
                    var mandarins = GetItemsBySynonym(res.Mandarin, res.MandarinSynonym);

                    mandarins = ExcludeItems(translateType, mandarins, res.Exclusion);

                    result.Contents.AddRange(mandarins);
                }
            };

            V_Mandarin2Cantonese target = FindMandarin2Cantonese(translateType, content);

            if (target != null)
            {
                setResult(target);
            }
            else
            {
                #region 普=>粤
                if (translateType == TranslateType.Mandarin2Cantonese)
                {
                    foreach (var mc in Mandarin2Cantoneses)
                    {
                        if (!string.IsNullOrEmpty(mc.MandarinRegex))
                        {
                            var regexItems = mc.MandarinRegex.Split(ItemsSplitors);

                            foreach (var regexItem in regexItems)
                            {
                                string mandarinRegex = GetAppropriateRegex(regexItem);

                                var matches = Regex.Matches(content, mandarinRegex);

                                var parts = regexItem.Split(RegexPlaceHolder, StringSplitOptions.RemoveEmptyEntries);

                                foreach (Match match in matches)
                                {
                                    var value = match.Value;

                                    foreach (var part in parts)
                                    {
                                        value = value.Replace(part, "");
                                    }

                                    target = FindMandarin2Cantonese(translateType, value);

                                    string targetCantonese = null;

                                    if (target != null)
                                    {
                                        var cantoneses = GetItemsBySynonym(target.Cantonese, target.CantoneseSynonym);

                                        targetCantonese = mc.CantoneseRegex.Replace(RegexPlaceHolder, cantoneses.FirstOrDefault());
                                    }
                                    else
                                    {
                                        targetCantonese = mc.CantoneseRegex.Replace(RegexPlaceHolder, value);
                                    }

                                    var cantonese = targetCantonese.Split(ItemsSplitors).FirstOrDefault();

                                    content = content.Replace(match.Value, cantonese);
                                }
                            }
                        }
                    }

                    var pieces = GetCutPieces(translateType, content);

                    bool handled1 = false;
                    bool handled2 = false;
                    bool handled3 = false;
                    bool handled4 = false;
                    bool handled5 = false;

                    for (int i = 0; i < pieces.Count; i++)
                    {
                        string word = pieces[i].Word;

                        if (word == "比")
                        {
                            if (!handled1)
                            {
                                if (i + 1 <= pieces.Count)
                                {
                                    ExchangeCutPiece(pieces, i, i + 1);
                                }

                                if (i + 2 <= pieces.Count)
                                {
                                    ExchangeCutPiece(pieces, i, i + 2);
                                }

                                handled1 = true;
                            }
                        }
                        else if (word == "暂时")
                        {
                            if (!handled2)
                            {
                                var moods = GetCantoneseMoods();

                                int moodIndex = -1;

                                for (int j = pieces.Count - 1; j > 0; j--)
                                {
                                    if (moods.Any(item => item.Pattern == pieces[j].ReplaceValue))
                                    {
                                        moodIndex = j;
                                        break;
                                    }
                                }

                                int lastWordIndex = moodIndex == -1 ? pieces.Count - 1 : moodIndex - 1;

                                pieces.Insert(lastWordIndex + 1, pieces[i]);
                                pieces.RemoveAt(i);

                                handled2 = true;
                            }
                        }
                        else if (word == "正在")
                        {
                            if (!handled3)
                            {
                                if (i + 1 <= pieces.Count)
                                {
                                    string nextWord = pieces[i + 1].Word;

                                    string combination = "";
                                    int count = 0;

                                    for (int j = 0; j < nextWord.Length; j++)
                                    {
                                        combination += nextWord[j];

                                        if ((combination == "上" || combination == "下")
                                           || ParticipleHelper.IsVerbWord(combination))
                                        {
                                            count++;
                                            break;
                                        }
                                    }

                                    if (count > 0)
                                    {
                                        var subPieces1 = GetCutPieces(translateType, combination);

                                        if (count < nextWord.Length)
                                        {
                                            bool isTranslated = pieces[i + 1].ReplaceValue != null;

                                            if (!isTranslated)
                                            {
                                                string leftWord = nextWord.Substring(count);

                                                var subPieces2 = GetCutPieces(translateType, leftWord);

                                                pieces.RemoveAt(i + 1);
                                                pieces.InsertRange(i + 1, subPieces2);
                                            }
                                            else
                                            {
                                                int verbWordLength = subPieces1.Select(item => item.ReplaceValue ?? item.Word).Sum(item => item.Length);

                                                string leftWord = pieces[i + 1].ReplaceValue.Substring(verbWordLength);

                                                pieces[i + 1].ReplaceValue = leftWord;
                                            }
                                        }

                                        pieces.InsertRange(i, subPieces1);
                                    }
                                }

                                handled3 = true;
                            }
                        }
                        else if (word == "太")
                        {
                            if (!handled4)
                            {
                                if (i + 1 < pieces.Count)
                                {
                                    ExchangeCutPiece(pieces, i, i + 1);

                                    handled4 = true;
                                }
                            }
                        }
                        else if(word == "几乎")
                        {
                            if(!handled5)
                            {
                                pieces.Add(pieces[i]);
                                pieces.RemoveAt(i);

                                handled5 = true;
                            }
                        }
                    }

                    result.Contents.Add(string.Join("", pieces.Select(item => item.ReplaceValue ?? item.Word)));
                }
                #endregion
                #region 粤=>普
                else
                {
                    var moods = GetCantoneseMoods();

                    foreach (var endParttern in moods)
                    {
                        if (content.EndsWith(endParttern.Pattern))
                        {
                            content = content.Substring(0, content.Length - endParttern.Pattern.Length);
                        }
                    }

                    List<SentenceItem> items = new List<SentenceItem>();
                    List<SentenceItem> matchedItems = new List<SentenceItem>();

                    foreach (var mc in Mandarin2Cantoneses)
                    {
                        var cantoneses = GetItemsBySynonym(mc.Cantonese, mc.CantoneseSynonym);

                        Func<Match, bool> checkMatches = (match) =>
                        {
                            int index = match.Index;
                            int stopIndex = match.Index + match.Length - 1;

                            if (matchedItems.Any(item => item.StartIndex <= index && item.StopIndex >= stopIndex))
                            {
                                return false;
                            }

                            var smallMatchedItems = matchedItems.Where(item => item.StartIndex >= index && item.StopIndex <= stopIndex);

                            foreach (var sm in smallMatchedItems.OrderByDescending(item => item.StartIndex))
                            {
                                matchedItems.RemoveAt(matchedItems.IndexOf(sm));
                            }

                            return true;
                        };

                        if (!string.IsNullOrEmpty(mc.CantoneseRegex))
                        {
                            var regexItems = mc.CantoneseRegex.Split(ItemsSplitors);

                            foreach (var regexItem in regexItems)
                            {
                                string cantoneseRegex = GetAppropriateRegex(regexItem);

                                var matches = Regex.Matches(content, cantoneseRegex);

                                var parts = regexItem.Split(RegexPlaceHolder, StringSplitOptions.RemoveEmptyEntries);

                                foreach (Match match in matches)
                                {
                                    if (checkMatches(match) == false)
                                    {
                                        continue;
                                    }

                                    var value = match.Value;

                                    foreach (var part in parts)
                                    {
                                        value = value.Replace(part, "");
                                    }

                                    target = FindMandarin2Cantonese(translateType, value);

                                    string targetMandarin = null;

                                    if (target != null)
                                    {
                                        var mandarins = GetItemsBySynonym(target.Mandarin, target.MandarinSynonym);

                                        targetMandarin = mc.MandarinRegex.Replace(RegexPlaceHolder, mandarins.FirstOrDefault());
                                    }
                                    else
                                    {
                                        targetMandarin = mc.MandarinRegex.Replace(RegexPlaceHolder, value);
                                    }

                                    var mandarin = targetMandarin.Split(ItemsSplitors).FirstOrDefault();

                                    SentenceItem item = new SentenceItem() { Word = match.Value, StartIndex = match.Index, IsMatched = true, ReplaceValue = mandarin };

                                    matchedItems.Add(item);
                                }
                            }
                        }
                        else
                        {
                            string regex = $@"({string.Join("|", cantoneses)})";

                            MatchCollection matches = Regex.Matches(content, regex);

                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                {
                                    if (checkMatches(match) == false)
                                    {
                                        continue;
                                    }

                                    SentenceItem item = new SentenceItem() { Word = match.Value, StartIndex = match.Index, IsMatched = true };

                                    var mandarins = GetItemsBySynonym(mc.Mandarin, mc.MandarinSynonym);

                                    item.ReplaceValue = mandarins.FirstOrDefault();

                                    matchedItems.Add(item);
                                }
                            }
                        }
                    }

                    items.AddRange(matchedItems);

                    int startIndex = 0;

                    for (int i = 0; i < content.Length;)
                    {
                        var matchedItem = matchedItems.FirstOrDefault(item => item.StartIndex == i);

                        if (matchedItem != null)
                        {
                            i += matchedItem.Word.Length;
                            startIndex = i;
                            continue;
                        }
                        else if (matchedItems.Any(item => item.StartIndex - 1 == i))
                        {
                            string word = content.Substring(startIndex, i - startIndex + 1);

                            SentenceItem item = new SentenceItem() { Word = word, StartIndex = startIndex, ReplaceValue = word };

                            items.Add(item);

                            i++;
                            startIndex = i;
                        }
                        else if (i == content.Length - 1)
                        {
                            string word = content.Substring(startIndex);

                            SentenceItem item = new SentenceItem() { Word = word, StartIndex = startIndex, ReplaceValue = word };

                            items.Add(item);

                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    items = items.OrderBy(item => item.StartIndex).ToList();

                    foreach (var pattern in CantoneseSentencePatterns)
                    {
                        if (pattern.IsMiddle || pattern.IsEnd)
                        {
                            var flag = pattern.Pattern;

                            var foundItem = items.FirstOrDefault(item => item.Word == flag);

                            if (foundItem != null)
                            {
                                int index = items.IndexOf(foundItem);

                                bool isMiddle = index > 0 && index < items.Count - 1;
                                bool isEnd = index == items.Count - 1;

                                if (flag == "先")
                                {
                                    if (pattern.IsEnd && isEnd)
                                    {
                                        items[index].ReplaceValue = "先";

                                        if (index > 0)
                                        {
                                            ExchangeSentenceItem(items, index - 1, index);
                                        }
                                    }
                                }
                                else if (flag == "得滞")
                                {
                                    if (pattern.IsEnd && isEnd)
                                    {
                                        if (index > 0)
                                        {
                                            string previousWord = items[index - 1].ReplaceValue ?? items[index - 1].Word;

                                            if (ParticipleHelper.IsAdjective(previousWord))
                                            {
                                                ExchangeSentenceItem(items, index - 1, index);
                                            }
                                            else
                                            {
                                                var cuts = ParticipleHelper.Cut(previousWord);

                                                if (cuts.LastOrDefault()?.Flag == "a")
                                                {
                                                    items[index - 1].ReplaceValue = string.Join("", cuts.Take(cuts.Count - 1).Select(item => item.Word));

                                                    items.Insert(index + 1, new SentenceItem() { ReplaceValue = cuts.LastOrDefault()?.Word });
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (flag == "乜滞")
                                {
                                    if (pattern.IsEnd && isEnd)
                                    {
                                        int foundIndex = 0;

                                        for (int j = 0; j < items.Count; j++)
                                        {
                                            bool isNounOrPronoun = ParticipleHelper.IsNounOrPronoun(items[j].ReplaceValue ?? items[j].Word);

                                            if(!isNounOrPronoun)
                                            {
                                                foundIndex = j;
                                                break;
                                            }
                                        }

                                        items.Insert(foundIndex, foundItem);
                                        items.RemoveAt(index + 1);
                                    }
                                }
                                else if (flag == "开")
                                {
                                    if (index == 0
                                        || (index > 0 && items[index - 1].ReplaceValue == items[index].ReplaceValue)
                                        || (index > 0 && !ParticipleHelper.IsVerbWord(items[index - 1].ReplaceValue))
                                    )
                                    {
                                        items[index].ReplaceValue = items[index].Word;
                                    }
                                }
                                else if (flag == "过" && isMiddle)
                                {
                                    ExchangeSentenceItem(items, index - 1, index);

                                    ExchangeSentenceItem(items, index, index + 1);
                                }
                                else if (flag == "紧" && index > 0)
                                {
                                    ExchangeSentenceItem(items, index - 1, index);

                                    if (index < items.Count - 1)
                                    {
                                        string combination = items[index].Word;

                                        V_Mandarin2Cantonese res = null;

                                        int lastIndex = -1;

                                        for (int i = index + 1; i < items.Count; i++)
                                        {
                                            combination += items[i].Word;

                                            res = FindMandarin2Cantonese(translateType, combination);

                                            lastIndex = i;
                                        }

                                        if (res != null)
                                        {
                                            var mandarins = GetItemsBySynonym(res.Mandarin, res.MandarinSynonym);

                                            mandarins = ExcludeItems(translateType, mandarins, res.Exclusion);

                                            items[index].ReplaceValue = mandarins.FirstOrDefault();

                                            for (int j = lastIndex; j > index; j--)
                                            {
                                                items.RemoveAt(j);
                                            }
                                        }
                                    }
                                }
                                else if (flag == "住" && index > 0)
                                {
                                    if (!ParticipleHelper.IsVerbWord(items[index - 1].ReplaceValue, true))
                                    {
                                        items.Insert(0, items[index]);
                                        items.RemoveAt(index + 1);
                                    }
                                }
                            }
                        }
                    }

                    result.Contents.Add(string.Join("", items.Select(item => item.ReplaceValue)));
                }
                #endregion
            }

            return result;
        }

        private static List<CutPiece> GetCutPieces(TranslateType translateType, string content)
        {
            var cuts = ParticipleHelper.Cut(content);

            List<CutPiece> pieces = new List<CutPiece>();

            bool previousIsOrderIncreased = false;

            for (int i = 0; i < cuts.Count; i++)
            {
                var cut = cuts[i];

                CutPiece piece = new CutPiece() { Word = cut.Word, Flag = cut.Flag, Order = i };

                if (previousIsOrderIncreased)
                {
                    piece.Order--;
                    previousIsOrderIncreased = false;
                }

                var target = FindMandarin2Cantonese(translateType, cut.Word);

                if (target != null)
                {
                    var cantonese = GetItemsBySynonym(target.Cantonese, target.CantoneseSynonym);

                    piece.ReplaceValue = ExcludeItems(translateType, cantonese, target.Exclusion).FirstOrDefault();
                }
                else
                {
                    foreach (var mc in Mandarin2Cantoneses)
                    {
                        var mandarins = GetItemsBySynonym(mc.Mandarin, mc.MandarinSynonym);

                        foreach (var mandarin in mandarins)
                        {
                            if (!string.IsNullOrEmpty(mandarin))
                            {
                                Regex regex = new Regex(mandarin);

                                var matches = regex.Matches(content);

                                foreach (Match match in matches)
                                {
                                    var cantoneses = GetItemsBySynonym(mc.Cantonese, mc.CantoneseSynonym);

                                    cantoneses = ExcludeItems(translateType, cantoneses, mc.Exclusion);

                                    if (cantoneses.Count() > 0)
                                    {
                                        piece.ReplaceValue = piece.Word.Replace(match.Value, cantoneses.FirstOrDefault());
                                    }
                                }
                            }
                        }
                    }
                }

                pieces.Add(piece);

                string word = cuts[i].Word;

                if (word == "先")
                {
                    if (i + 1 < cuts.Count && cuts[i + 1].Flag.StartsWith("v")) //动词
                    {
                        piece.Order++;
                        previousIsOrderIncreased = true;
                    }
                }
            }

            pieces = pieces.OrderBy(i => i.Order).ToList();

            return pieces;
        }

        private static V_Mandarin2Cantonese FindMandarin2Cantonese(TranslateType translateType, string word)
        {
            V_Mandarin2Cantonese target = null;

            foreach (var mc in Mandarin2Cantoneses)
            {
                if (translateType == TranslateType.Mandarin2Cantonese)
                {
                    if (mc.Mandarin == word)
                    {
                        target = mc;
                    }
                    else
                    {
                        var items = GetItemsBySynonym(mc.Mandarin, mc.MandarinSynonym);

                        if (items.Contains(word))
                        {
                            target = mc;
                        }
                    }
                }
                else
                {
                    if (mc.Cantonese == word)
                    {
                        target = mc;
                    }
                    else
                    {
                        var items = GetItemsBySynonym(mc.Cantonese, mc.CantoneseSynonym);

                        if (items.Contains(word))
                        {
                            target = mc;
                        }
                    }
                }

                if (target != null)
                {
                    break;
                }
            }

            return target;
        }

        private static void ExchangeSentenceItem(List<SentenceItem> sentenceItems, int index1, int index2)
        {
            SentenceItem temp = null;

            temp = sentenceItems[index1];
            sentenceItems[index1] = sentenceItems[index2];
            sentenceItems[index2] = temp;
        }

        private static void ExchangeCutPiece(List<CutPiece> cutPieces, int index1, int index2)
        {
            CutPiece temp = null;

            temp = cutPieces[index1];
            cutPieces[index1] = cutPieces[index2];
            cutPieces[index2] = temp;
        }

        private static IEnumerable<CantoneseSentencePattern> GetCantoneseMoods()
        {
            var moods = CantoneseSentencePatterns.Where(item => item.IsEnd && item.IsMood);

            return moods;
        }

        public static IEnumerable<string> GetItemsBySynonym(string content, string synonym)
        {
            var items = content.Split(ItemsSplitors);

            if (string.IsNullOrEmpty(synonym))
            {
                synonym = string.Empty;
            }

            List<string> newItems = new List<string>();

            var synonymItems = synonym.Split(ItemsSplitors);

            if(CantoneseSynonyms == null)
            {
                CantoneseSynonyms = DataProcessor.GetCantoneseSynonyms().Result;
            }

            foreach (var sItem in synonymItems.Concat(CantoneseSynonyms.Select(item => item.Item)))
            {
                var kvs = sItem.Split('=');

                if (kvs.Length < 2)
                {
                    continue;
                }

                foreach (var item in items)
                {
                    char? containsChar = item.FirstOrDefault(t => kvs.Contains(t.ToString()));

                    if (containsChar.HasValue)
                    {
                        foreach (var kv in kvs)
                        {
                            if (!item.Contains(kv))
                            {
                                string newItem = item.Replace(containsChar.Value.ToString(), kv);

                                if (!newItems.Contains(newItem) && !item.Contains(newItem))
                                {
                                    newItems.Add(newItem);
                                }
                            }
                        }
                    }
                }
            }

            return items.Concat(newItems);
        }

        private static string GetAppropriateRegex(string regex)
        {
            return regex.Replace(RegexPlaceHolder, @"\S+");
        }

        private static IEnumerable<string> ExcludeItems(TranslateType translateType, IEnumerable<string> items, string exclusion)
        {
            if (string.IsNullOrEmpty(exclusion))
            {
                return items;
            }

            string[] exclusions = exclusion.Split(ItemsSplitors);

            foreach (var exclusionItem in exclusions)
            {
                var kvs = exclusionItem.Split('≠');

                if (kvs.Length == 2)
                {
                    if (translateType == TranslateType.Mandarin2Cantonese)
                    {
                        items = items.Except(new string[] { kvs[1] });
                    }
                    else
                    {
                        items = items.Except(new string[] { kvs[0] });
                    }
                }
            }

            return items;
        }
    }
}
