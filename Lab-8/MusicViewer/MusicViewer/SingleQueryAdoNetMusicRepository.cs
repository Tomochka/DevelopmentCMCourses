using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MusicViewer
{
    class SingleQueryAdoNetMusicRepository : IMusicRepository
    {
        private readonly string _connectionString;
        Dictionary<int, List<Song>> songs = new Dictionary<int, List<Song>>();
        Dictionary<int, Album> albums = new Dictionary<int, Album>();

        public SingleQueryAdoNetMusicRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Album> ListAlbums()
        {
            IList<Album> resultsAlbum = new List<Album>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [albums].albumId, [albums].date, [albums].title as titleAlbum, [songs].duration, [songs].songId, [songs].title as titleSong" +
                      " FROM [albums] INNER JOIN [songs] ON [albums].albumId = [songs].albumId", connection);


                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var albumId = (int)dataReader["albumId"];

                        if (!albums.ContainsKey(albumId))
                        {
                            albums.Add(albumId, new Album(
                            albumId,
                            (DateTime)dataReader["date"],
                            (string)dataReader["titleAlbum"],
                            null));
                        }

                        if (songs.ContainsKey(albumId))
                        {
                            songs[albumId].Add(new Song(
                                 (int)dataReader["songId"],
                                 (string)dataReader["titleSong"],
                                 (TimeSpan)dataReader["duration"]
                                 ));
                        }
                        else
                        {
                            songs.Add(albumId, new List<Song>());
                            songs[albumId].Add(new Song(
                                 (int)dataReader["songId"],
                                 (string)dataReader["titleSong"],
                                 (TimeSpan)dataReader["duration"]
                                 ));
                        }
                    }

                    IList<Song> resultsSong = new List<Song>();

                    foreach (var album in albums)
                    {
                        resultsSong = songs[album.Key];
                        resultsAlbum.Add(new Album(album.Value.Id, album.Value.Date, album.Value.Title, resultsSong));
                    };
                }
            }

            return resultsAlbum;
        }
    }
}
