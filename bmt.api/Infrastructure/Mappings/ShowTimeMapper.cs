using Core.Domain.Entities;
using Core.Shared.DTOs.Response.ShowTime;

namespace Core.Infrastructure.Mappings
{
    public interface IShowtimeMapper<T> where T : ShowTimeRes, new()
    {
        T ToModel(Showtime dataModel);
        Showtime ToDataModel(T model);
        ShowTimeRes ToModel<T>(T data) where T : ShowTimeRes, new();
    }

    public class ShowtimeMapper<T> : IShowtimeMapper<T> where T : ShowTimeRes, new()
    {
        public ShowtimeMapper() { }

        // Chuyển từ ShowTimeRes (DTO) sang Showtime (Entity)
        public Showtime ToDataModel(T model)
        {
            var result = new Showtime
            {
                ShowtimeId = model.ShowtimeId,
                MovieId = model.MovieId,
                Room = model.Room,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Price = model.Price,
                CreatedDate = DateTime.Now,
                CreatedUserId = 1,
                IsDeleted = false,
            };

            return result;
        }

        // Chuyển từ Showtime (Entity) sang ShowTimeRes (DTO)
        public T ToModel(Showtime dataModel)
        {
            var result = new T
            {
                ShowtimeId = dataModel.ShowtimeId,
                MovieId = dataModel.MovieId,
                Room = dataModel.Room,
                StartTime = dataModel.StartTime,
                EndTime = dataModel.EndTime,
                Price = dataModel.Price,
            };

            return result;
        }

        public ShowTimeRes ToModel<T>(T data) where T : ShowTimeRes, new()
        {
            return new ShowTimeRes
            {
                ShowtimeId = data.ShowtimeId,
                MovieId = data.MovieId,
                Room = data.Room,
                StartTime = data.StartTime,
                EndTime = data.EndTime,
                Price = data.Price
            };
        }
    }
}
