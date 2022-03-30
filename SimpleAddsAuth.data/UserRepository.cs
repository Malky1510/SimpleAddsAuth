using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAddsAuth.data
{
    public class UserRepository

    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Logins (Name, Email, PasswordHash) " +
                "VALUES (@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));

            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;

        }
        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }
        public List<Add> GetAdds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select a.Id, a.LoginId, a.Description, a.Number, l.Name, l.Email, a.Date from Adds a JOIN Logins l ON a.LoginId = l.Id";
            List<Add> addslist = new List<Add>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                addslist.Add(new Add
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Number = (string)reader["Number"],
                    Email = (string)reader["Email"],
                    Description = (string)reader["Description"],
                    Date = (DateTime)reader["Date"],
                    LoginId = (int)reader["LoginId"]
                  
                   
                   
                });
            }
            return addslist;
        }
        public List<Add> GetAllAdds(int Id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT a.id, a.LoginId,a.Description,a.Number,l.Name,l.Email,a.Date FROM Adds a JOIN Logins l on a.LoginId = l.Id where LoginId=@id";
            cmd.Parameters.AddWithValue("@id", Id);
            List<Add> adds = new List<Add>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adds.Add(new Add
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["Name"],
                    Number = (string)reader["Number"],
                    Email = (string)reader["Email"],
                    Description = (string)reader["Description"],
                    Date = (DateTime)reader["Date"],
                    LoginId = (int)reader["LoginId"]
                 
                });
            }
            return adds;
        }
        public void DeleteAdd(int Id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Adds WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", Id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public void NewAdd(Add add)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Adds VALUES (@PhoneNumber, @Description, @LoginId, GetDate())";
            cmd.Parameters.AddWithValue("@PhoneNumber", add.Number);
            cmd.Parameters.AddWithValue("@Description", add.Description);
            cmd.Parameters.AddWithValue("@LoginId", add.LoginId);
            connection.Open();
            cmd.ExecuteNonQuery();

        }

    }
}
