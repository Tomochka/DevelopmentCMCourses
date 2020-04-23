namespace Autocomplete.Basic
{
    using System.IO;
    using System.Linq;
    using System.Threading;

    public sealed class LiveSearch
    {
        private static readonly string[] SimpleWords = File.ReadAllLines(@"Data/words.txt");
        private static readonly string[] MovieTitles = File.ReadAllLines(@"Data/movies.txt");
        private static readonly string[] StageNames = File.ReadAllLines(@"Data/stagenames.txt");

        private static Thread _searchThread1;
        private static Thread _searchThread2;
        private static Thread _searchThread3;

        private static SimilarLine stageResult;
        private static SimilarLine movieResult;
        private static SimilarLine wordResult;
        private static string Line;

        public static void FindBestSimilar(string example, HintedControl control)
        {
            _searchThread1 = new Thread(
               () =>
               {
                   stageResult = BestSimilarInArray(StageNames, example);
               });

            _searchThread1.Start();

            _searchThread2 = new Thread(
                () =>
                {
                    movieResult = BestSimilarInArray(MovieTitles, example);
                });

            _searchThread2.Start();

            _searchThread3 = new Thread(
                () =>
                {
                    wordResult = BestSimilarInArray(SimpleWords, example);

                    _searchThread1.Join();
                    _searchThread2.Join();

                    if (wordResult.SimilarityScore > movieResult.SimilarityScore &&
                wordResult.SimilarityScore > stageResult.SimilarityScore)
                    {
                        Line = wordResult.Line;
                    }
                    else
                    {
                        Line = (stageResult.IsBetterThan(movieResult) ? stageResult : movieResult).Line;
                    }

                    control.Hint = Line;
                });

            _searchThread3.Start();
        }

        public void HandleTyping(HintedControl control)
        {
            if (_searchThread1 != null || _searchThread2 != null || _searchThread3 != null)
            {
                _searchThread1.Abort();
                _searchThread2.Abort();
                _searchThread3.Abort();

                _searchThread1.Join();
                _searchThread2.Join();
                _searchThread3.Join();

                stageResult = null;
                movieResult = null;
                wordResult = null;
            }

            FindBestSimilar(control.LastWord, control);
        }

        internal static SimilarLine BestSimilarInArray(string[] lines, string example)
        {
            return lines.Aggregate(
                new SimilarLine(string.Empty, 0),
                (SimilarLine best, string line) =>
                {
                    var current = new SimilarLine(line, line.Similarity(example));

                    return current.IsBetterThan(best) ? current : best;
                });
        }
    }
}
