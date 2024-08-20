using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Security;
using System.Reflection.PortableExecutable;

namespace FirstApp.Pages.Karyawan
{
    public class CreateModel : PageModel
    {
        public KaryawanInfo karyawanInfo = new KaryawanInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            karyawanInfo.kodekaryawan = Request.Form["kodekaryawan"];
            karyawanInfo.namakaryawan = Request.Form["namakaryawan"];
            karyawanInfo.jeniskelamin = Request.Form["jeniskelamin"];
            karyawanInfo.levelkaryawan = Request.Form["levelkaryawan"];
            karyawanInfo.email = Request.Form["email"];
            karyawanInfo.alamat = Request.Form["alamat"];
            karyawanInfo.nohp = Request.Form["nohp"];
            karyawanInfo.keterangan = Request.Form["keterangan"];

            if (karyawanInfo.kodekaryawan.Length == 0 || karyawanInfo.namakaryawan.Length == 0)
            {
                errorMessage = "Kode dan Nama kosong";
                return;
            }
            if (karyawanInfo.jeniskelamin.Length > 2 || karyawanInfo.jeniskelamin.Length == 0)
            {
                errorMessage = "Jenis kelamin hanya bisa P / L";
                return;
            }

            //simpan db

            try
            {
                String connetionString = "Data Source=LAPTOP-105I6KJU;Initial Catalog=pendaftaran;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO tblKaryawan (kodekaryawan, namakaryawan, jeniskelamin, levelkaryawan, email, alamat, nohp, keterangan) VALUES " +
                                "(@kodekaryawan, @namakaryawan, @jeniskelamin, @levelkaryawan, @email, @alamat, @nohp, @keterangan);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@kodekaryawan", karyawanInfo.kodekaryawan);
                        command.Parameters.AddWithValue("@namakaryawan", karyawanInfo.namakaryawan);
                        command.Parameters.AddWithValue("@jeniskelamin", karyawanInfo.jeniskelamin);
                        command.Parameters.AddWithValue("@levelkaryawan", karyawanInfo.levelkaryawan);
                        command.Parameters.AddWithValue("@email", karyawanInfo.email);
                        command.Parameters.AddWithValue("@alamat", karyawanInfo.alamat);
                        command.Parameters.AddWithValue("@nohp", karyawanInfo.nohp);
                        command.Parameters.AddWithValue("@keterangan", karyawanInfo.keterangan);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;

            }

            karyawanInfo.namakaryawan = "";
            successMessage = "Berhasil ditambahkan";

            Response.Redirect("/Karyawan/Index");

        }
    }
}
