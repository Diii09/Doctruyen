using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
using AppDocTruyen.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Text;

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
        /*  [HttpGet]
          [Route("GetAll")]
          public List<Truyen> GetALL(string query, int a, int b)
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
                  obj.IDTruyen = int.Parse(dt.Rows[i]["idTruyen"].ToString());
                  obj.TenTruyen = dt.Rows[i]["TenTruyen"].ToString();
                  obj.IDTheLoai = int.Parse(dt.Rows[i]["idTheLoai"].ToString());
                  obj.IDAccount = int.Parse(dt.Rows[i]["idAccount"].ToString());
                  tr.Add(obj);

              }
              return tr;
          }*/

        [HttpGet]
        [Route("GetAll")]
        public List<Truyen> GetAll(string searchQuery, int pageNumber, int pageSize)
        {
            List<Truyen> tr = new List<Truyen>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
            {
                con.Open();

                StringBuilder queryBuilder = new StringBuilder("SELECT * FROM Truyen");
              
                    
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            queryBuilder.Append(" WHERE TenTruyen LIKE @searchQuery or TacGia LIKE @searchQuery ");
                        }

                        queryBuilder.Append(" ORDER BY idTruyen OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY");

                        string query = queryBuilder.ToString();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            int offset = (pageNumber - 1) * pageSize;
                            cmd.Parameters.AddWithValue("@offset", offset);
                            cmd.Parameters.AddWithValue("@pageSize", pageSize);

                            if (!string.IsNullOrEmpty(searchQuery))
                            {
                                cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
                            }

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Truyen obj = new Truyen();
                                    obj.IDTruyen = reader.GetInt32(reader.GetOrdinal("idTruyen"));
                                    obj.TenTruyen = reader.GetString(reader.GetOrdinal("TenTruyen"));
                                    obj.IDTheLoai = reader.GetInt32(reader.GetOrdinal("idTheLoai"));
                                    obj.IDAccount = reader.GetInt32(reader.GetOrdinal("idAccount"));
                                    obj.TacGia = reader.GetString(reader.GetOrdinal("TacGia"));
                                    tr.Add(obj);
                                }
                            }
                        }
                    return tr;
            
            }

        }


        [HttpPost("Create")]
            public async Task<IActionResult> Create([FromQuery] string TenTruyen,int IDTheLoai,int IdAccount,string TacGia)
            {
            try
            {
                {
                    string query = "INSERT INTO Truyen(TenTruyen,idTheLoai,idAccount,TacGia)" +"" + "VALUES(@TenTruyen,@IDTheLoai,@IDAccount,@TacGia) ";
                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen")))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@TenTruyen", TenTruyen);
                            cmd.Parameters.AddWithValue("@IDTheLoai", IDTheLoai);
                            cmd.Parameters.AddWithValue("@IDAccount", IdAccount);
                            cmd.Parameters.AddWithValue("@TacGia", TacGia);

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
            public async Task<IActionResult> Update([FromRoute] int IDTruyen, [FromQuery] string TenTruyen,int IdTheLoai,int IdAccount,string TacGia)
            {
                try
                {
                    SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppTruyen"));
                    SqlCommand cmd = new SqlCommand("update Truyen set TenTruyen='" + TenTruyen +"',idTheLoai='"+IdTheLoai+"',idAccount='"+IdAccount+"',TacGia='"+TacGia+"' where idTruyen='" + IDTruyen + "'", con);
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

