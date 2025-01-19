using Core.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IMovieService<T>
        where T : Core.Repositories.Model.Movie, new()
    {
        T? GetMovieById(int id);
        bool AddMovie(T movie);
    }
    public class MovieService<T>: IMovieService<T>
        where T: Core.Repositories.Model.Movie, new()
    {
        private readonly IMovieRepository<T> _repository;
        public MovieService(IMovieRepository<T> repository)
        {
            _repository = repository;
        }

        public T? GetMovieById(int id)
        {
            T? movie = _repository.GetMovieById(id);

            return movie;
        }

        public bool AddMovie(T movie)
        {
            bool result = _repository.AddMovie(movie);

            return result;
        }
    }
}
