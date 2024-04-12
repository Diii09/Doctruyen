using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AppDocTruyen.Models;

namespace AppDocTruyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TheLoaiController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpGet]
        [Route("GetAll")]
        public List<TheLoai> GetALL()
        {
            List<TheLoai> tl = new List<TheLoai>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
            SqlCommand cmd = new SqlCommand("Select * from TheLoai", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TheLoai obj = new TheLoai();
                obj.IdTheLoai= int.Parse(dt.Rows[i]["idTheLoai"].ToString());
                obj.TenTheLoai = dt.Rows[i]["TenTheLoai"].ToString();
                tl.Add(obj);    
            }
            return tl;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromQuery] string TenTheLoai)
        {
            try
            {
                string query = "INSERT INTO TheLoai(TenTheLoai)" + "VALUES(@TenTheLoai)";
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TenTheLoai", TenTheLoai);
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
        [Route("Update/Update/{IdTheLoai}")]
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] int IdTheLoai, [FromQuery] string TenTheLoai)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("update TheLoai set TenTheLoai='" + TenTheLoai  + "'where idTheLoai='" + IdTheLoai + "'", con);
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

    }
}
