using Microsoft.EntityFrameworkCore;
using Core.Repositories.Mapper;
using Core.Services;
using Core.Repositories.DataModel;
using Core.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Cấu hình Swagger cho API
        builder.Services.AddSwaggerGen();

        // Cấu hình DbContext với chuỗi kết nối từ appsettings.json
        builder.Services.AddDbContext<BookMovieTicketContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Thêm các dịch vụ liên quan đến API
        builder.Services.AddControllers();  // Đảm bảo đã thêm các controller vào container

        // Thêm các dịch vụ khác nếu cần (Ví dụ: AutoMapper, các dịch vụ của ứng dụng)
        builder.Services.AddScoped<IMovieService<Core.Repositories.Model.Movie>, MovieService<Core.Repositories.Model.Movie>>();  // Ví dụ thêm một dịch vụ

        // Repositories
        builder.Services.AddScoped<IMovieMapper<Core.Repositories.Model.Movie>, MovieMapper<Core.Repositories.Model.Movie>>();
        builder.Services.AddScoped<IMovieRepository<Core.Repositories.Model.Movie>, MovieRepository<Core.Repositories.Model.Movie>>();

        // Cấu hình cho Swagger UI
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