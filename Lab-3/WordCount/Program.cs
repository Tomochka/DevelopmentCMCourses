namespace WordCount
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string text = File.ReadAllText(@"..\..\..\Data\Начало.txt", Encoding.GetEncoding("windows-1251")) +
                File.ReadAllText(@"..\..\..\Data\Хэппи_Энд.txt") +
                File.ReadAllText(@"..\..\..\Data\Беда_одна_не_ходит.txt");
            
            int wordCount = 0;

            wordCount += CalculateWordCount(text);

            Console.WriteLine("Count of words in story files: {0}", wordCount);
        }

        private static int CalculateWordCount(string text)
        {
            var arrayHashSet = new HashSet<string>();
            var word = string.Empty;

            foreach (var ch in text)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    word += ch;
                }
                else
                {
                    if (word.Length == 0)
                    {
                        continue;
                    }

                   
                    arrayHashSet.Add(word.ToLower());
                    word = string.Empty;
                }
            }
                       
            return arrayHashSet.Count;
        }
    }
}
