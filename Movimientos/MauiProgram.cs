using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Movimientos.Data;
using Movimientos.Platforms.Android;
using Movimientos.Services;
using Movimientos.Services.Interfaces;
using Movimientos.ViewModels;
using Movimientos.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Movimientos
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IDbPathService, DbPathService>();
            builder.Services.AddSingleton<DbContext>();
            builder.Services.AddSingleton<IDbService, DbService>();

            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddTransient<MovimientoViewModel>();
            builder.Services.AddTransient<MovimientoPage>();
            builder.Services.AddTransient<RubroViewModel>();
            builder.Services.AddTransient<RubroPage>();
            builder.Services.AddSingleton<MovimientosViewModel>();
            builder.Services.AddSingleton<MovimientosPage>();
            builder.Services.AddSingleton<RubrosViewModel>();
            builder.Services.AddSingleton<RubrosPage>();

            return builder.Build();
        }
    }
}
