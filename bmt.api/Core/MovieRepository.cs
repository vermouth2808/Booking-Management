using Core.Repositories.DataModel;
using Core.Repositories.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IMovieRepository<T>
        where T: Model.Movie, new()
    {
        T? GetMovieById(int id);
        bool AddMovie(T movie);
    }
    public class MovieRepository<T>: IMovieRepository<T>
        where T: Model.Movie, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMovieMapper<T> _mapper;
        public MovieRepository(BookMovieTicketContext context, IMovieMapper<T> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public T? GetMovieById(int id)
        {
            var efItem = _context.Movies.FirstOrDefault(m => m.MovieId == id);

            if(efItem == null)
            {
                return null;
            }

            return _mapper.ToModel(efItem);
        }

        public bool AddMovie(T movie)
        {
            var efItem = _mapper.ToDataModel(movie);

            _context.Add(movie);
            _context.SaveChanges();

            return true;
        }
    }
}
