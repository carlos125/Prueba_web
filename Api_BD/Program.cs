
using Api_BD.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api_BD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();

            builder.Services.AddSwaggerGen();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



            builder.Services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(connectionString));
            // Se agrega Jwt para la autenticacion de tokens
            var key = builder.Configuration["Jwt:Key"];  // Obtiene la clave secreta desde el archivo de configuración.
            var Issuer = builder.Configuration["Jwt:Issuer"];  // Obtiene la clave secreta desde el archivo de configuración.
            // Configuración del servicio de autenticación en la aplicación
            builder.Services.AddAuthentication(config =>
            {
                // Especifica el esquema de autenticación predeterminado como JWT
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;  // Si es 'false', permite HTTP, no solo HTTPS (en producción es mejor habilitarlo).
                config.SaveToken = true;  // Guarda el token en el contexto de la autenticación.
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,  // Valida la clave de firma del token.
                    ValidIssuer = Issuer,  //  Este valor debe coincidir con el 'Issuer' que generas al crear el token.
                    ValidateIssuer = true,  // Valida el emisor del token.
                    ValidateAudience = false,  // No valida la audiencia en este caso.
                    ValidateLifetime = true,  // Valida que el token no haya expirado.
                    ClockSkew = TimeSpan.Zero,  // Sin margen de error al verificar el tiempo de expiración.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))  // La clave secreta para verificar la firma.
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Permitir cualquier origen
                          .AllowAnyHeader()  // Permitir cualquier encabezado
                          .AllowAnyMethod(); // Permitir cualquier método (GET, POST, PUT, DELETE, etc.)
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

 
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
