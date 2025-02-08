using Api_BD.Context;
using Api_BD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_BD.Controllers
{
    public class UserController : ControllerBase
    {
       private readonly UserContext _userContext;
        private readonly string secretKey;
        private readonly string issuer;
        private readonly IHttpClientFactory _httpClientFactory;  //para crear clientes HTTP

        public UserController (IHttpClientFactory httpClientFactory, IConfiguration configuration, UserContext context)
        {
            _httpClientFactory = httpClientFactory;// Inicializa la conexion HTTP
            secretKey = configuration.GetSection("Jwt").GetValue<string>("key")!; // Se obtiene la clave secreta desde la configuración 
            issuer = configuration.GetSection("Jwt").GetValue<string>("Issuer")!; // Se obtiene la clave secreta desde la configuración 
            _userContext = context;// Inicializa el contexto de la base de datos
        }



        [HttpGet]
        [Route("Users")]
        [EnableCors("AllowAll")] // Usar el nombre de la política CORS
        public async Task<IActionResult> GetUsers()
        {
            // Obtén solo los campos necesarios
            var users = await _userContext.Users
                .Select(user => new
                {
                    name = user.Name,
                    lastName = user.LastName,
                    email = user.Email,
                    phone = user.Phone,
                    id = user.Id,
                    rol = user.Rol,
                    accion= ""
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet]
        [Route("gettUser/{Find}")]
        public async Task<IActionResult> getUsers(string Find)
        {
            // Busca la transacción en la base de datos
            var user = await _userContext.Users
                .FirstOrDefaultAsync(t => t.Name == Find || t.LastName == Find);

            if (user == null)
            {
                return NotFound(new { message = "Error 404" });
            }

            return Ok(user);
        }
        [HttpPut]
        [Route("updateUser")]
        [EnableCors("AllowAll")] // Usar el nombre de la política CORS
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserModels request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");

                // Validación de campos vacíos
                if (string.IsNullOrWhiteSpace(request.UserName) ||
                    string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.Name) ||
                    string.IsNullOrWhiteSpace(request.LastName) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Phone) ||
                    string.IsNullOrWhiteSpace(request.Rol))
                {
                    return BadRequest("Faltan Campos por llenar.");
                }


            }
            try
            {
                var user = await _userContext.Users.FirstOrDefaultAsync(t =>  t.Id == request.Id); // Buscamos si el usuario existe en la BD

                if (user == null) // Si no existe el usuario, devolvemos un error
                {
                    return NotFound(new
                    {
                        result = "Usuario no encontrado"
                    });
                }

                // Si el usuario existe, actualizamos los campos
                user.UserName = request.UserName;
                user.Password = request.Password; 
                user.Name = request.Name;
                user.LastName = request.LastName;
                user.Phone = request.Phone;
                user.Rol = request.Rol;

                // Actualizamos el usuario en la base de datos
                _userContext.Users.Update(user);
                await _userContext.SaveChangesAsync();

                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost]
        [Route("addUser")]
        [EnableCors("AllowAll")] // Usar el nombre de la política CORS
        [Authorize]
        public async Task<IActionResult> AddUser([FromBody] UserModels request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }
            // Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Phone) ||
                string.IsNullOrWhiteSpace(request.Rol))
            {
                return BadRequest("Faltan Campos por llenar.");
            }
            try
            {

                var userValidate = await _userContext.Users.FirstOrDefaultAsync(t => t.Email == request.Email ); // Buscamos si ya existe la transacción en la BD
                if (userValidate != null)
                    return Conflict(new
                    {
                        result = "Usuario existente"
                    }); // Devolvemos error


                // Crear una instancia de UserEntity para agregar a la base de datos
                var newUser = new UserModels
                {
                    UserName = request.UserName,
                    Password = request.Password, // Asegúrate de usar un hash seguro para las contraseñas
                    Name = request.Name,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Rol = request.Rol
                };

                // Agrega el User 
                var result = await _userContext.Users.AddAsync(newUser);
                // Guarda el Users 
                await _userContext.SaveChangesAsync();
                return Ok("User added successfully.");

            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        [EnableCors("AllowAll")] // Usar el nombre de la política CORS
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email y contraseña son requeridos" });
            }

            // Buscar usuario en la base de datos
            var user = await _userContext.Users
                .FirstOrDefaultAsync(t => t.Email == request.Email && t.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            // Retornar solo los datos necesarios del usuario
            return Ok(new
            {
                token = token(),
                email = user.Email,
                name = user.Name
            });
        }
        [HttpDelete]
        [Route("deleteUser")]
        [EnableCors("AllowAll")] // Usar el nombre de la política CORS
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] UserModels request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var user = await _userContext.Users.FirstOrDefaultAsync(t => t.Id == request.Id); // Buscamos si el usuario existe en la BD

                if (user == null) // Si el usuario no existe, devolvemos un error
                {
                    return NotFound(new
                    {
                        result = "Usuario no encontrado"
                    });
                }

                // Eliminar el usuario
                _userContext.Users.Remove(user);
                await _userContext.SaveChangesAsync();

                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        public string token()
        {
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);  // La clave secreta usada para firmar el token.
            var Issuer = Encoding.ASCII.GetBytes(issuer);  // La Issuer secreta usada para firmar el token.


            var claims = new ClaimsIdentity(/* Aquí puedes agregar claims si es necesario.*/);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://localhost:7112/",
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(10),  // expiración en 10 minutos. 
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)  // Firma el token usando HMACSHA256.
            };

            var tokenHandel = new JwtSecurityTokenHandler();  // Crea el manejador de tokens JWT.
            var tokenConfig = tokenHandel.CreateToken(tokenDescriptor);  // Genera el token a partir del descriptor.
            string tokencreado = tokenHandel.WriteToken(tokenConfig);

            return tokencreado;

        }
    }
}
