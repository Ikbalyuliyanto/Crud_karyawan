using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FirstApp.Pages.Karyawan
{
    public class IndexModel : PageModel
    {
        public List<KaryawanInfo> ListKaryawans = new List<KaryawanInfo>();

        public void OnGet()
        {
            try
            {
                // Connection string
                String connetionString = "Data Source=LAPTOP-105I6KJU;Initial Catalog=pendaftaran;Integrated Security=True;Encrypt=False";

                // Create and open connection
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM tblkaryawan";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                KaryawanInfo karyawanInfo = new KaryawanInfo
                                {
                                    id = reader.GetInt32(0).ToString(),
                                    kodekaryawan = reader.GetString(3),
                                    namakaryawan = reader.GetString(4),
                                    jeniskelamin = reader.GetString(5),
                                    levelkaryawan = reader.GetString(8),
                                    email = reader.GetString(1), // Sesuaikan indeks kolom dengan data di tabel
                                    alamat = reader.GetString(2), 
                                    nohp = reader.GetString(6), 
                                    keterangan = reader.GetString(7),
                                };

                                ListKaryawans.Add(karyawanInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message); // Cetak pesan exception
            }
        }
    }

    public class KaryawanInfo
    {
        public string id { get; set; }
        public string namakaryawan { get; set; }
        public string email { get; set; }
        public string alamat { get; set; }
        public string kodekaryawan { get; set; }
        public string jeniskelamin { get; set; }
        public string nohp { get; set; }
        public string levelkaryawan { get; set; }
        public string keterangan { get; set; }
    }
}
