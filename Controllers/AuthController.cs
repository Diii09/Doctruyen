
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AppDocTruyen.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using static AppDocTruyen.Controllers.AccountController;
using System.Data;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace YourProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                // Kiểm tra thông tin đăng nhập
                bool isValid = ValidateLogin(request.Username, request.PwAccount);
                if (!isValid)
                {
                    return Unauthorized("Invalid username or password");
                }
                Account account=GetAccountInfo(request.Username,request.PwAccount);

                 // Tạo và trả về JWT
                var token = GenerateJwtToken(request.Username);
                return Ok(new { accessToken = token,user=account});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        private bool ValidateLogin(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Account WHERE username=@username AND pwAccount=@pwAccount", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwAccount", password);
                int count = (int)cmd.ExecuteScalar();
                
                return count > 0;
            }
        }

        private Account GetAccountInfo(string username, string password)
        {
            Account acc = null;
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Account WHERE username=@username AND pwAccount=@pwAccount", con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwAccount", password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    acc = new Account
                    {
                        IdAcc = reader.GetInt32(reader.GetOrdinal("idAccount")),
                       Ten = reader.GetString(reader.GetOrdinal("Ten")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        PwAccount = reader.GetString(reader.GetOrdinal("pwAccount")),
                       TrangThai = reader.GetString(reader.GetOrdinal("TrangThai")),
                        Role = reader.GetInt32(reader.GetOrdinal("roleAcc"))
                    };
                }
                reader.Close();
            }
                return acc;

        }
        ////[ApiController]
        //[Route("api/[controller]")]


        [HttpGet("me")]
        [Authorize]
        public IActionResult GetLoggedInUserInfo()
        {

            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var userInfo = new
            {
                UserId = userId,

            };

            return Ok(userInfo);
        }


        // Hàm tạo JWT Token
        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username) // Add username to claims
        }),
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        //private string GenerateJwtToken(int id)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
        //      _configuration["Jwt:Issuer"],
        //      null,
        //      expires: DateTime.Now.AddMinutes(120),
        //      signingCredentials: credentials);

        //    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
        //    return token;
        //}
    }

}





