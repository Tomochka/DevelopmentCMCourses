namespace MusicBrowser.Console.DataAccess
{
    using MusicBrowser.Console.Domain;
    using System.Collections.Generic;

    public interface IMusicRepository
    {
        IEnumerable<Album> ListAlbums();

        IEnumerable<Song> ListSongs(Album album);

        void Delete(Song song);

        void Delete(Album album);

        Song Add(Song song);

        Album Add(Album album);
    }
}