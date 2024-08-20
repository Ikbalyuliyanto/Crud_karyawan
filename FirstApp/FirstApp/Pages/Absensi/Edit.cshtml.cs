using FirstApp.Pages.Absensi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FirstApp.Pages.Absensi
{
    public class EditModel : PageModel
    {
        public AbsensiInfo absensiInfo = new AbsensiInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];

            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "ID Karyawan tidak ditemukan.";
                return;
            }

            try
            {
                string connectionString = "Data Source=LAPTOP-105I6KJU;Initial Catalog=pendaftaran;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tblabsensikaryawan WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                absensiInfo.id = reader.GetInt32(0).ToString();
                                absensiInfo.kodekaryawan = reader.GetString(1);
                                absensiInfo.namakaryawan = reader.GetString(2);

                                // Konversi string ke DateTime dan TimeSpan
                                absensiInfo.tanggalmasuk = reader.IsDBNull(3) ? DateTime.Now.Date : reader.GetDateTime(3);
                                absensiInfo.tanggalkeluar = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
                                absensiInfo.jammasuk = reader.IsDBNull(5) ? TimeSpan.Zero : reader.GetTimeSpan(5);
                                absensiInfo.jamkeluar = reader.IsDBNull(6) ? TimeSpan.Zero : reader.GetTimeSpan(6);
                            }
                            else
                            {
                                errorMessage = "Karyawan tidak ditemukan dengan ID tersebut.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Terjadi kesalahan: " + ex.Message;
            }
        }

        public void OnPost()
        {
            absensiInfo.id = Request.Form["id"];
            absensiInfo.kodekaryawan = Request.Form["kodekaryawan"];
            absensiInfo.namakaryawan = Request.Form["namakaryawan"];

            // Mendapatkan tanggal dan waktu hari ini
            var todayDate = DateTime.Now.Date;
            var nowTime = DateTime.Now.TimeOfDay;

            // Mengambil nilai dari form
            DateTime tanggalMasuk;
            DateTime tanggalKeluar;
            TimeSpan jamMasuk;
            TimeSpan jamKeluar;

            var tanggalMasukString = Request.Form["tanggalmasuk"];
            var tanggalKeluarString = Request.Form["tanggalkeluar"];
            var jamMasukString = Request.Form["jammasuk"];
            var jamKeluarString = Request.Form["jamkeluar"];

            if (DateTime.TryParse(tanggalMasukString, out tanggalMasuk))
            {
                absensiInfo.tanggalmasuk = tanggalMasuk;
            }
            else
            {
                absensiInfo.tanggalmasuk = todayDate;
            }

            if (DateTime.TryParse(tanggalKeluarString, out tanggalKeluar))
            {
                absensiInfo.tanggalkeluar = tanggalKeluar;
            }
            else
            {
                absensiInfo.tanggalkeluar = todayDate;
            }

            if (TimeSpan.TryParse(jamMasukString, out jamMasuk))
            {
                absensiInfo.jammasuk = jamMasuk;
            }
            else
            {
                absensiInfo.jammasuk = nowTime;
            }

            if (TimeSpan.TryParse(jamKeluarString, out jamKeluar))
            {
                absensiInfo.jamkeluar = jamKeluar;
            }
            else
            {
                absensiInfo.jamkeluar = nowTime;
            }

            if (string.IsNullOrEmpty(absensiInfo.namakaryawan))
            {
                errorMessage = "Nama karyawan tidak boleh kosong.";
                return;
            }

            try
            {
                string connectionString = "Data Source=LAPTOP-105I6KJU;Initial Catalog=pendaftaran;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE tblabsensikaryawan SET kodekaryawan=@kodekaryawan, namakaryawan=@namakaryawan, tanggalmasuk=@tanggalmasuk, " +
                                 "tanggalkeluar=@tanggalkeluar, jammasuk=@jammasuk, jamkeluar=@jamkeluar WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@kodekaryawan", absensiInfo.kodekaryawan);
                        command.Parameters.AddWithValue("@namakaryawan", absensiInfo.namakaryawan);
                        command.Parameters.AddWithValue("@tanggalmasuk", absensiInfo.tanggalmasuk);
                        command.Parameters.AddWithValue("@tanggalkeluar", (object)absensiInfo.tanggalkeluar ?? DBNull.Value);
                        command.Parameters.AddWithValue("@jammasuk", absensiInfo.jammasuk);
                        command.Parameters.AddWithValue("@jamkeluar", absensiInfo.jamkeluar);
                        command.Parameters.AddWithValue("@id", absensiInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Terjadi kesalahan: " + ex.Message;
                return;
            }

            successMessage = "Data karyawan berhasil diperbarui.";
            Response.Redirect("/Absensi/Index");
        }
    }
}
