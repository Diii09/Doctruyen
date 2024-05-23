using AppDocTruyen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AppDocTruyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpGet]   
        [Route("GetAll")]
        public List<Account> GetALL()
        {
            List<Account> accounts = new List<Account>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
            SqlCommand cmd = new SqlCommand("Select * from Account", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Account obj = new Account();
                obj.IdAcc = int.Parse(dt.Rows[i]["idAccount"].ToString());
                obj.Ten = dt.Rows[i]["Ten"].ToString();
                obj.Username = dt.Rows[i]["username"].ToString();
                obj.PwAccount = dt.Rows[i]["pwAccount"].ToString();
                obj.TrangThai = dt.Rows[i]["TrangThai"].ToString();
                obj.Role = int.Parse(dt.Rows[i]["roleAcc"].ToString());
                accounts.Add(obj);
            }
            return accounts;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromQuery] string Ten, string Username, string Password, string TrangThai, int Role)
        {
            try
            {
                string query = "INSERT INTO Account(Ten,username,pwAccount,TrangThai,roleAcc)" + "VALUES(@Ten,@username,@password,@TrangThai,@Role)";
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Ten", Ten);
                        cmd.Parameters.AddWithValue("@username", Username);
                        cmd.Parameters.AddWithValue("@password", Password);
                        cmd.Parameters.AddWithValue("@TrangThai", TrangThai);
                        cmd.Parameters.AddWithValue("@Role", Role);
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Route("Update/Update/{IdAcc}")]
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] int IdAcc, UpdateAccountRequest profile)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("update Account set Ten='" + profile.Ten + "',username='" + profile.Username + "',pwAccount='" + profile.PwAccount + "',TrangThai='" + profile.TrangThai + "',roleAcc='" + profile.Role + "' where idAccount='" + IdAcc + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Route("Account/UpdateRole/{IdAcc}")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole([FromRoute] int IdAcc, [FromQuery] int Role)
        {
            try {

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("update Account set roleAcc='" + Role + "'where idAccount='" + IdAcc + "'",con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok();
            }catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

    }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("SELECT * FROM Account WHERE username=@username AND pwAccount=@password", con);
                cmd.Parameters.AddWithValue("@username", login.Username);
                cmd.Parameters.AddWithValue("@password", login.PwAccount);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Đăng nhập thành công
                    return Ok();
                }
                else
                {
                    // Đăng nhập thất bại
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}

