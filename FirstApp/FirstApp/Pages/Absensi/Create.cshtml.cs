using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;

namespace FirstApp.Pages.Absensi
{
    public class CreateModel : PageModel
    {
        public AbsensiInfo absensiInfo = new AbsensiInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            absensiInfo.kodekaryawan = Request.Form["kodekaryawan"];
            absensiInfo.namakaryawan = Request.Form["namakaryawan"];
            // Mengambil nilai dari form
            string tanggalMasukString = Request.Form["tanggalmasuk"];
            string tanggalKeluarString = Request.Form["tanggalkeluar"];
            string jamMasukString = Request.Form["jammasuk"];
            string jamKeluarString = Request.Form["jamkeluar"];

            // Mendapatkan tanggal dan waktu hari ini
            var todayDate = DateTime.Now.Date;
            var nowTime = DateTime.Now.TimeOfDay;
            // Variabel untuk menyimpan hasil parsing
            DateTime tanggalMasuk;
            DateTime tanggalKeluar;
            TimeSpan jamMasuk;
            TimeSpan jamKeluar;

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



            if (absensiInfo.kodekaryawan.Length == 0 || absensiInfo.namakaryawan.Length == 0)
            {
                errorMessage = "Kode dan Nama kosong";
                return;
            }

            //simpan db

            try
            {
                String connetionString = "Data Source=LAPTOP-105I6KJU;Initial Catalog=pendaftaran;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO tblabsensikaryawan (kodekaryawan, namakaryawan, tanggalmasuk, tanggalkeluar, jammasuk, jamkeluar) VALUES " +
                                "(@kodekaryawan, @namakaryawan, @tanggalmasuk, @tanggalkeluar, @jammasuk, @jamkeluar);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@kodekaryawan", absensiInfo.kodekaryawan);
                        command.Parameters.AddWithValue("@namakaryawan", absensiInfo.namakaryawan);
                        command.Parameters.AddWithValue("@tanggalmasuk", absensiInfo.tanggalmasuk);
                        command.Parameters.AddWithValue("@tanggalkeluar", absensiInfo.tanggalkeluar);
                        command.Parameters.AddWithValue("@jammasuk", absensiInfo.jammasuk);
                        command.Parameters.AddWithValue("@jamkeluar", absensiInfo.jamkeluar);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;

            }

            absensiInfo.namakaryawan = "";
            successMessage = "Berhasil ditambahkan";

            Response.Redirect("/Absensi/Index");

        }
    }
}
