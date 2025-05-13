using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using Barcode_Reports_Task.Services;
using Barcode_Reports_Task.UnitOfWorks;
using Microsoft.EntityFrameworkCore;


namespace Barcode_Reports_Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<BarcodeDBContext>(s => { s.UseSqlServer(builder.Configuration.GetConnectionString("conString")); });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IFtpUploader, FakeFtpUploader>();
            builder.Services.AddScoped<IPdfExportService, PdfExportService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=report}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
