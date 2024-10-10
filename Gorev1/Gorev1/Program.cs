using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorev1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection("Data Source=CAYIRAILESI\\SQLEXPRESS;Initial Catalog=StudentsDB;Integrated Security=True;TrustServerCertificate=True");

            while (true)
            {
                Console.WriteLine("1. Öğrenci Ekle");
                Console.WriteLine("2. Öğrenci Sil");
                Console.WriteLine("3. Öğrenci Güncelle");
                Console.WriteLine("4. Öğrencileri Listele");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddStudent(connection);
                        break;
                    case 2:
                        DeleteStudent(connection);
                        break;
                    case 3:
                        UpdateStudent(connection);
                        break;
                    case 4:
                        Listele(connection);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        static void AddStudent(SqlConnection connection)
        {
            string name, lastname;
            int age;
            DateTime enrollmentDate;
            Console.WriteLine("Kişi adı Giriniz :  ");
            name = Console.ReadLine();
            Console.WriteLine("Soyadı Giriniz :  ");
            lastname = Console.ReadLine();
            Console.WriteLine("Yaşını Giriniz : ");
            age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Kayıt Tarihini Giriniz (yyyy-MM-dd) : ");
            enrollmentDate = DateTime.Parse(Console.ReadLine());

            connection.Open();
            SqlCommand command = new SqlCommand("insert into Students (FirstName, LastName, Age, EnrollmentDate) values (@1, @2, @3, @4)", connection);
            command.Parameters.AddWithValue("@1", name);
            command.Parameters.AddWithValue("@2", lastname);
            command.Parameters.AddWithValue("@3", age);
            command.Parameters.AddWithValue("@4", enrollmentDate);

            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("Kayıt başarıyla eklendi.");
        }

        static void DeleteStudent(SqlConnection connection)
        {
            Console.WriteLine("Silmek istediğiniz öğrencinin ID'sini giriniz: ");
            int id = Convert.ToInt32(Console.ReadLine());

            
            connection.Open();
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE ID = @id", connection);
            checkCmd.Parameters.AddWithValue("@id", id);
            int recordExists = (int)checkCmd.ExecuteScalar();
            if (recordExists == 0)
            {
                Console.WriteLine("Kayıt bulunamadı.");
                connection.Close();
                return;
            }

            SqlCommand command = new SqlCommand("DELETE FROM Students WHERE ID = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Kayıt başarıyla silindi.");
            }
            else
            {
                Console.WriteLine("Kayıt bulunamadı.");
            }
        }


        static void UpdateStudent(SqlConnection connection)
        {
            Console.WriteLine("Güncellemek istediğiniz öğrencinin ID'sini giriniz: ");
            int id = Convert.ToInt32(Console.ReadLine());

           
            connection.Open();
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE ID = @id", connection);
            checkCmd.Parameters.AddWithValue("@id", id);
            int recordExists = (int)checkCmd.ExecuteScalar();
            if (recordExists == 0)
            {
                Console.WriteLine("Kayıt bulunamadı.");
                connection.Close();
                return;
            }

            Console.WriteLine("Yeni Kişi adı Giriniz :  ");
            string name = Console.ReadLine();
            Console.WriteLine("Yeni Soyadı Giriniz :  ");
            string lastname = Console.ReadLine();
            Console.WriteLine("Yeni Yaşını Giriniz : ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Yeni Kayıt Tarihini Giriniz (yyyy-MM-dd) : ");
            DateTime enrollmentDate = DateTime.Parse(Console.ReadLine());

            SqlCommand command = new SqlCommand("UPDATE Students SET FirstName = @1, LastName = @2, Age = @3, EnrollmentDate = @4 WHERE ID = @id", connection);
            command.Parameters.AddWithValue("@1", name);
            command.Parameters.AddWithValue("@2", lastname);
            command.Parameters.AddWithValue("@3", age);
            command.Parameters.AddWithValue("@4", enrollmentDate);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Kayıt başarıyla güncellendi.");
            }
            else
            {
                Console.WriteLine("Kayıt bulunamadı.");
            }
        }


        static void Listele(SqlConnection connection)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0] + " - " + reader[1] + " - " + reader[2] + " - " + reader[3] + " - " + reader[4]);
                Console.WriteLine("-------------------------------------------------");
            }
            connection.Close();
        }
    }
}
