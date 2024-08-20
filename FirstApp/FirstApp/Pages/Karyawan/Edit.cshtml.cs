using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FirstApp.Pages.Karyawan
{
    public class EditModel : PageModel
    {
        public KaryawanInfo karyawanInfo = new KaryawanInfo();
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
                    string sql = "SELECT * FROM tblKaryawan WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                karyawanInfo.id = reader.GetInt32(0).ToString();
                                karyawanInfo.kodekaryawan = reader.GetString(3);
                                karyawanInfo.namakaryawan = reader.GetString(4);
                                karyawanInfo.jeniskelamin = reader.GetString(5);
                                karyawanInfo.levelkaryawan = reader.GetString(8);
                                karyawanInfo.email = reader.GetString(1);
                                karyawanInfo.alamat = reader.GetString(2);
                                karyawanInfo.nohp = reader.GetString(6);
                                karyawanInfo.keterangan = reader.GetString(7);
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
            karyawanInfo.id = Request.Form["id"];
            karyawanInfo.kodekaryawan = Request.Form["kodekaryawan"];
            karyawanInfo.namakaryawan = Request.Form["namakaryawan"];
            karyawanInfo.jeniskelamin = Request.Form["jeniskelamin"];
            karyawanInfo.levelkaryawan = Request.Form["levelkaryawan"];
            karyawanInfo.email = Request.Form["email"];
            karyawanInfo.alamat = Request.Form["alamat"];
            karyawanInfo.nohp = Request.Form["nohp"];
            karyawanInfo.keterangan = Request.Form["keterangan"];

            if (karyawanInfo.namakaryawan.Length == 0)
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
                    string sql = "UPDATE tblKaryawan SET kodekaryawan=@kodekaryawan, namakaryawan=@namakaryawan, jeniskelamin=@jeniskelamin, " +
                                 "levelkaryawan=@levelkaryawan, email=@email, alamat=@alamat, nohp=@nohp, keterangan=@keterangan " +
                                 "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", karyawanInfo.id); // Deklarasi parameter @id
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
                errorMessage = "Terjadi kesalahan: " + ex.Message;
                return;
            }

            successMessage = "Data karyawan berhasil diperbarui.";
            Response.Redirect("/Karyawan/Index");
        }
    }
}
