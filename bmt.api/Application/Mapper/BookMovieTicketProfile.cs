using AutoMapper;
using Core.Domain.Entities;
using Core.Shared.DTOs.Response.Banner;
using Core.Shared.DTOs.Response.Category;
using Core.Shared.DTOs.Response.FoodCombo;
using Core.Shared.DTOs.Response.FoodOrder;
using Core.Shared.DTOs.Response.Movie;
using Core.Shared.DTOs.Response.Room;
using Core.Shared.DTOs.Response.ShowTime;

namespace Core.Application.Mapper
{
    public class BookMovieTicketProfile : Profile
    {
        public BookMovieTicketProfile()
        {
            CreateMap<Movie, MovieRes>();
            CreateMap<Movie, MovieSearchRes>();
            CreateMap<Banner, BannerRes>();
            CreateMap<Banner, ReadAllBannerRes>();
            CreateMap<Showtime, ShowTimeRes>();
            CreateMap<Showtime, ShowTimeSearchRes>(); 
            CreateMap<Category, CategoryRes>();
            CreateMap<FoodCombo, FoodComboRes>();
            CreateMap<Room, RoomRes>();
            CreateMap<FoodOrder, FoodOrderRes>();
            CreateMap<FoodOrder, FoodOrderSearchRes>();
        }
    }
}
