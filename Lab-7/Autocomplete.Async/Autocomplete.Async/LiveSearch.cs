namespace Autocomplete.Async
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LiveSearch
    {
        private static readonly string[] SimpleWords = File.ReadAllLines(@"Data/words.txt");
        private static readonly string[] MovieTitles = File.ReadAllLines(@"Data/movies.txt");
        private static readonly string[] StageNames = File.ReadAllLines(@"Data/stagenames.txt");

        private static CancellationTokenSource _token;

        public async Task<string> FindBestSimilarAsync(string example)
        {
            if (_token != null)
            {
                _token.Cancel();
            }

            _token = new CancellationTokenSource();

            Task<SimilarLine> stageTask = Task<SimilarLine>.Run(() => BestSimilarInArray(StageNames, example));
            Task<SimilarLine> movieTask = Task<SimilarLine>.Run(() => BestSimilarInArray(MovieTitles, example));
            Task<SimilarLine> wordTask = Task<SimilarLine>.Run(() => BestSimilarInArray(SimpleWords, example));

            Task allTasks = Task.WhenAll(new[] { stageTask, movieTask, wordTask });
            await allTasks;

            if (wordTask.Result.Line == string.Empty || movieTask.Result.Line == string.Empty || stageTask.Result.Line == string.Empty)
            {
                return string.Empty;
            }

            if (wordTask.Result.SimilarityScore > movieTask.Result.SimilarityScore
               && wordTask.Result.SimilarityScore > stageTask.Result.SimilarityScore)
            {
                return wordTask.Result.Line;
            }

            if (movieTask.Result.IsBetterThan(stageTask.Result))
            {
                return movieTask.Result.Line;
            }
            
            return stageTask.Result.Line;
        }

        public async void HandleTyping(HintedControl control)
        {
             control.Hint = await FindBestSimilarAsync(control.LastWord);
        }

        internal static SimilarLine BestSimilarInArray(string[] lines, string example)
        {
            var best = new SimilarLine(string.Empty, 0);

            foreach (var line in lines)
            {
                if (_token.IsCancellationRequested)
                {
                    return new SimilarLine(string.Empty, 0);                    
                }

                var currentLine = new SimilarLine(line, line.Similarity(example));

                if (currentLine.IsBetterThan(best))
                {
                    best = currentLine;
                }
            }

            return best;
        }
    }
}
