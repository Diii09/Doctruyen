using AppDocTruyen.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AppDocTruyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChuongController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public ChuongController(IConfiguration config)
        {
            _configuration = config;
        }
        [HttpGet]
        [Route("GetAll")]
        public List<Chuong> GetALL()
        {
            List<Chuong> tr = new List<Chuong>();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
            SqlCommand cmd = new SqlCommand("Select * from Chuong", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Chuong obj = new Chuong();
                obj.IDChuong = int.Parse(dt.Rows[i]["idChuong"].ToString());
                obj.IDTruyen = int.Parse(dt.Rows[i]["idTruyen"].ToString());
                obj.SoChuong = int.Parse(dt.Rows[i]["Chuong"].ToString());
                obj.LinkTruyen = dt.Rows[i]["LinkTruyen"].ToString();
                tr.Add(obj);
            }
            return tr;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromQuery] int IDTruyen, int Chuong, string LinkTruyen)
        {
            try
            {
                string query = "INSERT INTO Chuong(idTruyen,Chuong,LinkTruyen)" + "VALUES(@idTruyen,@SoChuong,@LinkTruyen)";
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idTruyen", IDTruyen);
                        cmd.Parameters.AddWithValue("@SoChuong", Chuong);
                        cmd.Parameters.AddWithValue("@LinkTruyen", LinkTruyen);
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
        [Route("Update/Update/{IDChuong}")]
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] int IDChuong, [FromQuery] int IDTruyen, int Chuong, string LinkTruyen)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("update Chuong set idTruyen='" + IDTruyen + "',Chuong='" + Chuong + "',LinkTruyen='" + LinkTruyen + "' where idChuong='" + IDChuong + "'", con);
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
