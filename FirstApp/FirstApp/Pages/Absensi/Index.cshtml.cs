using FirstApp.Pages.Absensi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FirstApp.Pages.Absensi
{
    public class IndexModel : PageModel
    {
        public List<AbsensiInfo> ListAbsensis { get; set; } = new List<AbsensiInfo>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            try
            {
                // Connection string
                string connectionString = "Data Source=LAPTOP-105I6KJU;Initial Catalog=pendaftaran;Integrated Security=True;Encrypt=False";

                // Create and open connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tblabsensikaryawan";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AbsensiInfo absensiInfo = new AbsensiInfo
                                {
                                    id = reader.GetInt32(0).ToString(),
                                    kodekaryawan = reader.GetString(1),
                                    namakaryawan = reader.GetString(2),
                                    tanggalmasuk = reader.GetDateTime(3),
                                    tanggalkeluar = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    jammasuk = reader.GetTimeSpan(5),
                                    jamkeluar = reader.GetTimeSpan(6),
                                };

                                ListAbsensis.Add(absensiInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception: " + ex.Message; // Simpan pesan exception
            }
        }
    }

    public class AbsensiInfo
    {
        public string id { get; set; }
        public string kodekaryawan { get; set; }
        public string namakaryawan { get; set; }
        public DateTime tanggalmasuk { get; set; }
        public DateTime? tanggalkeluar { get; set; } // Nullable DateTime
        public TimeSpan jammasuk { get; set; }
        public TimeSpan jamkeluar { get; set; }
    }
}
