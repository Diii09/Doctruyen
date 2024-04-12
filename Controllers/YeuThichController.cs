using AppDocTruyen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AppDocTruyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YeuThichController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public YeuThichController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpGet]
        [Route("GetAll")]
        public List<YeuThich> GetALL()
        {
            List<YeuThich> tl = new List<YeuThich>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
            SqlCommand cmd = new SqlCommand("Select * from YeuThich", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                YeuThich obj = new YeuThich();
                obj.IDYeuThich = int.Parse(dt.Rows[i]["idYeuThich"].ToString());
                obj.IDTruyen = int.Parse(dt.Rows[i]["idTruyen"].ToString());
                tl.Add(obj);
            }
            return tl;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromQuery] int IDTruyen)
        {
            try
            {
                string query = "INSERT INTO YeuThich(idTruyen)" + "VALUES(@idTruyen)";
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idTruyen", IDTruyen);
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
        [Route("Update/Update/{IdYeuThich}")]
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] int IdYeuThich, [FromQuery] string IDTruyen)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("update YeuThich set idTruyen='" + IDTruyen + "'where idYeuThich='" + IdYeuThich + "'", con);
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
