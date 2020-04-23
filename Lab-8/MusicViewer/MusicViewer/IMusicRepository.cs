using System;
using System.Collections.Generic;
public interface IMusicRepository
{
    IEnumerable<Album> ListAlbums();
}
