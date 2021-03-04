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

            string albumsInfo= ExportSongsAboveDuration(context, 4);
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

        
        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .ToList()
                .Select(x => new
                {
                    Name = x.Name,
                    Performer = x.SongPerformers
                    .Select(p => p.Performer.FirstName + " " + p.Performer.LastName
                    ).FirstOrDefault(),
                    writerName = x.Writer.Name,
                    albumProducer = x.Album.Producer.Name,
                    duration = x.Duration

                })
                .Where(x => x.duration.TotalSeconds > duration)
                .OrderBy(x => x.Name).ThenBy(x => x.writerName).ThenBy(x => x.Performer)
                .ToList();

            StringBuilder sb = new StringBuilder();

            int numSong = 1;
            foreach (var s in songs)
            {
                sb.AppendLine($"-Song #{numSong++}");
                sb.AppendLine($"---SongName: {s.Name}");
                sb.AppendLine($"---Writer: {s.writerName}");
                sb.AppendLine($"---Performer: {s.Performer}");
                sb.AppendLine($"---AlbumProducer: {s.albumProducer}");
                sb.AppendLine($"---Duration: {s.duration:c}");
            }

           return sb.ToString().TrimEnd();
        }
    }
}

//-Song #1
//--- SongName: Away
//--- Writer: Norina Renihan
//---Performer: Lula Zuan
//---AlbumProducer: Georgi Milkov
//---Duration: 00:05:35


//export its Name, Performer Full Name, Writer Name, 
//    Album Producer and Duration (in format("c")). 
//    Sort the Songs by their Name (ascending), by Writer(ascending) and by Performer (ascending).