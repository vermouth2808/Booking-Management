using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Infrastructure.Mappings;
using Core.Infrastructure.Repositories;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Shared.DTOs.Movie;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Cấu hình DbContext với SQL Server
        builder.Services.AddDbContext<BookMovieTicketContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
   );


        // Đảm bảo đã thêm các controller vào container
        // Đảm bảo đăng ký các repository và service
        builder.Services.AddScoped<IMovieMapper<MovieRes>, MovieMapper<MovieRes>>();
        builder.Services.AddScoped<IMovieService<MovieRes>, MovieService<MovieRes>>();
        builder.Services.AddScoped<IMovieRepository<MovieRes>, MovieRepository<MovieRes>>();


        // Thêm các dịch vụ cần thiết cho API
        builder.Services.AddControllers();
        // Cấu hình cho Swagger UI
        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();           // Đảm bảo Swagger được sử dụng
            app.UseSwaggerUI();         // Đảm bảo Swagger UI hiển thị
        }

        app.UseHttpsRedirection();

        // Thêm đoạn này để ánh xạ các route của API controller
        app.MapControllers();  // Ánh xạ các controller API vào ứng dụng

        app.Run();
    }
}
