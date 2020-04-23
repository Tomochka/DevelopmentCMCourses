namespace ColorReplacement
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            var colorsDictionary = new Dictionary<string, string>();
            const string hex6 = @"#(\w){6}";
            const string hex3 = @"#(\w){3}\b";
            const string rgb = @"rgb\(([\d]{1,3},(\s)*){2}[\d]{1,3}\)";
            var colorRegex = new Regex(hex6 + "|" + hex3 + "|" + rgb, RegexOptions.IgnoreCase);

            var colorsSortedList = new SortedList<string, string>();

            using (var source = new StreamReader("Data/colors.txt", Encoding.UTF8))
            {
                // initializes colors
                string line;

                while ((line = source.ReadLine()) != null)
                {
                    var matchColor = colorRegex.Match(line);
                    var index = line.IndexOf(matchColor.ToString(), StringComparison.Ordinal);
                    line = line.Remove(index, matchColor.Length).Trim();
                    colorsDictionary.Add(matchColor.ToString(), line); //line = color name
                }
            }

            using (var source = new StreamReader("Data/source.txt", Encoding.UTF8))
            using (var target = new StreamWriter("Data/target.txt"))
            {
                // reads source.txt, replaces colors, writes target.txt, collects data about replaced colors
                var text = source.ReadToEnd();
                var matchColor = colorRegex.Match(text);
                var keyColor = matchColor.ToString();

                while (matchColor.Success)
                {
                    string nameColor;

                    if (Regex.IsMatch(keyColor, hex6))
                    {
                        if (colorsDictionary.TryGetValue(keyColor.ToUpper(), out nameColor))
                        {
                            text = text.Replace(keyColor, nameColor);

                            if (!colorsSortedList.ContainsKey(keyColor.ToUpper()))
                            {
                                colorsSortedList.Add(keyColor.ToUpper(), nameColor);
                            }
                        }
                    }

                    if (Regex.IsMatch(keyColor, hex3))
                    {
                        var hex3Hex6 = Regex.Replace(keyColor, @"(\w)", @"${1}${1}").ToUpper();

                        if (colorsDictionary.TryGetValue(hex3Hex6, out nameColor))
                        {
                            text = Regex.Replace(text, keyColor + @"\b", nameColor);

                            if (!colorsSortedList.ContainsKey(hex3Hex6))
                            {
                                colorsSortedList.Add(hex3Hex6, nameColor);
                            }
                        }
                    }

                    if (Regex.IsMatch(keyColor, rgb))
                    {
                        var evaluator = new MatchEvaluator(MEv);
                        var rgbHex6 = Regex.Replace(keyColor, @"(\d{1,3})", evaluator);

                        rgbHex6 = Regex.Replace(rgbHex6, @"\W", string.Empty);
                        rgbHex6 = Regex.Replace(rgbHex6, @"rgb", "#");

                        if (colorsDictionary.TryGetValue(rgbHex6, out nameColor))
                        {
                            text = text.Replace(keyColor, nameColor);

                            if (!colorsSortedList.ContainsKey(rgbHex6))
                            {
                                colorsSortedList.Add(rgbHex6, nameColor);
                            }
                        }

                        string MEv(Match match)
                        {
                            var i = int.Parse(match.ToString());
                            var hex = (i == 0) ? "00" : Convert.ToString(i, 16).ToUpper();
                            return (hex.Length != 2) ? "0" + hex : hex;
                        }
                    }

                    matchColor = matchColor.NextMatch();
                    keyColor = matchColor.ToString();
                }

                target.Write(text);
            }

            using (var target = new StreamWriter("Data/used_colors.txt"))
            {
                // writes data about replaced colors
                foreach (var c in colorsSortedList)
                {
                    target.WriteLine(c.Key + " " + c.Value);
                }
            }
        }
    }
}
