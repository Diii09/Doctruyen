using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
using AppDocTruyen.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace AppDocTruyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruyenController : ControllerBase
    {

        
            private readonly IConfiguration _configuration;
               private object accRepository;

        public TruyenController(IConfiguration config)
            {
                _configuration = config;
            }
            [HttpGet]
            [Route("GetAll")]
            public List<Truyen> GetALL()
            {
                List<Truyen> tr = new List<Truyen>();
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                SqlCommand cmd = new SqlCommand("Select * from Truyen", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Truyen obj = new Truyen();
                    obj.IDTruyen= int.Parse(dt.Rows[i]["idTruyen"].ToString());
                    obj.TenTruyen = dt.Rows[i]["TenTruyen"].ToString();
                    obj.IDTheLoai = int.Parse(dt.Rows[i]["idTheLoai"].ToString());
                    obj.IDAccount = int.Parse(dt.Rows[i]["idAccount"].ToString());
                tr.Add(obj);    
            }
                return tr;
            }

            [HttpPost("Create")]
            public async Task<IActionResult> Create([FromQuery] string TenTruyen,int IDTheLoai,int IdAccount)
            {
            try
            {
                {
                    string query = "INSERT INTO Truyen(TenTruyen,idTheLoai,idAccount)" +"" + "VALUES(@TenTruyen,@IDTheLoai,@IDAccount) ";
                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@TenTruyen", TenTruyen);
                            cmd.Parameters.AddWithValue("@IDTheLoai", IDTheLoai);
                            cmd.Parameters.AddWithValue("@IDAccount", IdAccount);


                            await con.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    return Ok();
                }
         
             
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            }
            [Route("Update/Update/{IDTruyen}")]
            [HttpPost]
            public async Task<IActionResult> Update([FromRoute] int IDTruyen, [FromQuery] string TenTruyen,int IdTheLoai,int IdAccount)
            {
                try
                {
                    SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                    SqlCommand cmd = new SqlCommand("update Truyen set TenTruyen='" + TenTruyen +"',idTheLoai='"+IdTheLoai+"',idAccount='"+IdAccount+"' where idTruyen='" + IDTruyen + "'", con);
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

