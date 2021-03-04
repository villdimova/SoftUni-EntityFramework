namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string albumsInfo=ExportAlbumsInfo(context, 9);
            Console.WriteLine(albumsInfo);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Producers
                .FirstOrDefault(p=>p.Id==producerId)
                .Albums
                
                .Select(x => new
                {
                    ProducerId = x.ProducerId,
                    albumName = x.Name,
                    releaseDate = x.ReleaseDate,
                    producerName = x.Producer.Name,
                    albumPrice = x.Price,
                    albumSongs = x.Songs.Select(s => new
                    {
                        
                        songName = s.Name,
                        songPrice = s.Price,
                        songWriterName = s.Writer.Name

                    }),

                    
                })
                .Where(x=>x.ProducerId==producerId)
                .OrderByDescending(a=>a.albumPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in albums)
            {
                int numSong = 1;
                sb.AppendLine($"-AlbumName: {a.albumName}");
                sb.AppendLine($"-ReleaseDate: {a.releaseDate:MM/dd/yyyy}");
                sb.AppendLine($"-ProducerName: {a.producerName}");
                sb.AppendLine($"-Songs:");

                foreach (var s in a.albumSongs.OrderByDescending(s=>s.songName)
                                               .ThenBy(s=>s.songWriterName))
                {

                    sb.AppendLine($"---#{numSong}");
                    sb.AppendLine($"---SongName: {s.songName}");
                    sb.AppendLine($"---Price: {s.songPrice:f2}");
                    sb.AppendLine($"---Writer: {s.songWriterName}");
                    numSong++;
                }

                sb.AppendLine($"-AlbumPrice: {a.albumPrice:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        //Export all albums which are produced by the provided Producer Id.
        //    For each Album, get the Name, Release date in format "MM/dd/yyyy",
        //    Producer Name, the Album Songs with each Song Name, Price (formatted to the second digit) 
        //    and the Song Writer Name.Sort the Songs by Song Name(descending) and by Writer(ascending).
        //    At the end export the Total Album Price with exactly two digits after the decimal place.
        //    Sort the Albums by their Total Price (descending).

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
