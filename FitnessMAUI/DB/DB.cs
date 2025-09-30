using FitnessMAUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.db
{
    internal class DB
    {
        public static DB Instance
        {
            get
            {
                if (instance == null)
                    instance = new DB();
                return instance;
            }
        }

        public int AutoIncrement { get; set; }

        List<Movie> Movies { get; set; } = new List<Movie>();

        List<Studio> Studios { get; set; } = new List<Studio>();

        string filename = "db.bin";
        private static DB instance;

        public DB()
        {
            if (!File.Exists(filename))
                return;
            int count;
            using (var fs = File.OpenRead(filename))
            using (var br = new BinaryReader(fs))
            {
                if (fs.Length == 0)
                    return;
                count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                    Movies.Add(new Movie
                    {
                        Id = br.ReadInt32(),
                        Title = br.ReadString(),
                        StudioId = br.ReadInt32(),
                        Relise = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),  
                        Status = br.ReadBoolean(),
                    });
                for (int i = 0; i < count; i++)
                    Studios.Add(new Studio
                    {
                        Id = br.ReadInt32(),
                        Name = br.ReadString(),
                        DirectorName = br.ReadString(),
                        DirectorPatronymic = br.ReadString(),
                        DirectorSurname = br.ReadString(),
                        Rating = br.ReadInt32()
                    });
            }
        }
        public void SaveMovie()
        {
            using (var fs = File.Create(filename))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(Movies.Count);
                foreach (var movie in Movies)
                {
                    bw.Write(movie.Id);
                    bw.Write(movie.Title);
                    bw.Write(movie.StudioId);
                    bw.Write(movie.Relise.Year);
                    bw.Write(movie.Relise.Month);
                    bw.Write(movie.Relise.Day);
                    bw.Write(movie.Status);   
                }
            }
        }

        public void SaveStudio()
        {
            using (var fs = File.Create(filename))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(Studios.Count);
                foreach (var studio in Studios)
                {
                    bw.Write(studio.Id);
                    bw.Write(studio.Name);
                    bw.Write(studio.DirectorName);
                    bw.Write(studio.DirectorPatronymic);
                    bw.Write(studio.DirectorSurname);
                    bw.Write(studio.Rating);
                }
            }
        }

        public void AddMovie(Movie movie)
        {
            movie.Id++;
            Movies.Add(movie);
            SaveMovie();
        }

        public List<Movie> GetMovies()
        {
            return Movies;
        }

        public void DeleteMovie(Movie movie)
        {
            Movies.Remove(movie);
            SaveMovie();
        }

        public void AddStudio(Studio studio)
        {
            studio.Id++;
            Studios.Add(studio);
            SaveStudio();
        }

        public List<Studio> GetStudios()
        {
            return Studios;
        }

        public void DeleteStudio(Studio studio)
        {
            Studios.Remove(studio);
            SaveStudio();
        }
    }
}
