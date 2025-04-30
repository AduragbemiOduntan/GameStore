namespace GameStore.Api
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }


        //public static void ConfigureCors(this IServiceCollection services) { 
        //services.AddCors(options =>
        //{
        //    options.AddPolicy("CorsPolicy", builder =>
        //    builder.AllowAnyOrigin()
        //    .AllowAnyMethod()
        //    .AllowAnyHeader());
        //});
        //    }

    }
}
