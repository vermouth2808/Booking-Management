namespace Core.Repositories.Mapper
{
    public interface IMovieMapper<T>
        where T: Model.Movie, new()
    {
        DataModel.Movie ToDataModel(T model);
        T ToModel(DataModel.Movie dataModel);
    }
    public class MovieMapper<T>: IMovieMapper<T>
        where T: Model.Movie, new()
    {
        public MovieMapper()
        {

        }

        public DataModel.Movie ToDataModel(T model)
        {
            var result = new DataModel.Movie
            {
                MovieId = model.MovieId,
                Title = model.Title,
                Genre = model.Genre,
                Duration = model.Duration,
                ReleaseDate = model.ReleaseDate,
                PosterUrl = model.PosterUrl,
                TrailerUrl = model.TrailerUrl,
                Description = model.Description,
               /* CreatedDate = model.CreatedDate,
                CreatedUserId = 1,
                UpdatedDate = model.UpdatedDate,
                UpdatedUserId = null,
                IsDeleted = model.IsDeleted*/
            };

            return result;

        }

        public T ToModel(DataModel.Movie dataModel)
        {
            var result = new T
            {
                MovieId = dataModel.MovieId,
                Title = dataModel.Title,
                Genre = dataModel.Genre,
                Duration = dataModel.Duration,
                ReleaseDate = dataModel.ReleaseDate,
                PosterUrl = dataModel.PosterUrl,
                TrailerUrl = dataModel.TrailerUrl,
                Description = dataModel.Description,
              /*  CreatedDate = dataModel.CreatedDate,
                CreatedUserId = 1,
                UpdatedDate = dataModel.UpdatedDate,
                UpdatedUserId = null,
                IsDeleted = dataModel.IsDeleted*/
            };

            return result;
        }
    } 
}
