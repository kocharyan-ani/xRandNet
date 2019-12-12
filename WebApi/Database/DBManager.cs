using System.Data;
using MySql.Data.MySqlClient;
using WebApi.Models;

namespace WebApi.Database

{
    public class DbManager
    {
        private readonly MySqlConnection _connection;

        public DbManager(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public string GetSecretKey()
        {
            const string query = @"SELECT jwtSecretKey FROM auth";
            using (_connection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                var cmd = new MySqlCommand(query, _connection);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                var jwtSecretKey = (string) reader["jwtSecretKey"];
                return jwtSecretKey;
            }
        }

        public User GetUser(CredentialsForLoginDto credentials)
        {
            const string query = @"
            SELECT * 
            FROM users
            WHERE username = @username AND password = @password
            ";
            using (_connection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                var cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@username", credentials.Username);
                cmd.Parameters.AddWithValue("@password", credentials.Password);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                var firstName = (string) reader["firstName"];
                var lastName = (string) reader["lastName"];
                var isAdmin = (bool) reader["isAdmin"];
                return new User(firstName, lastName, credentials.Username, credentials.Password, isAdmin);
            }
        }

        public void AddUser(User user)
        {
            const string query = @"
            INSERT INTO users (firstName, lastName, password, username, isAdmin)
            VALUES (@firstName, @lastName, @password, @username, 0)
            ";
            using (_connection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                var cmd = new MySqlCommand(query, _connection);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                cmd.Parameters.AddWithValue("@lastName", user.LastName);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Exists(User user)
        {
            return false;
        }

        public File GetUserManual()
        {
            const string query = @"
            SELECT * 
            FROM files 
            WHERE type='UserManualFile' 
            LIMIT 1";
            _connection.Open();
            using (_connection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                var cmd = new MySqlCommand(query, _connection);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                return new File
                {
                    Id = (int) reader["id"],
                    Name = reader["name"].ToString(),
                    Data = (byte[]) reader["data"],
                    Type = reader["type"].ToString(),
                    DownloadId = reader["downloadId"].ToString(),
                    MimeType = reader["mimeType"].ToString()
                };
            }
        }

        public void SetUserManual(File file)
        {
            var query = @"
            SELECT * 
            FROM files 
            WHERE type='UserManualFile' 
            LIMIT 1";
            using (_connection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                var cmd = new MySqlCommand(query, _connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    query = "UPDATE files SET data=@data,name=@name,mimeType=@mimeType WHERE type='UserManualFile' ";
                    cmd = new MySqlCommand(query, _connection);
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@data", MySqlDbType.Blob).Value = file.Data;
                    cmd.Parameters.AddWithValue("@name", file.Name);
                    cmd.Parameters.AddWithValue("@mimeType", file.MimeType);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    query = @"INSERT INTO files (name, downloadId, mimeType, type, data)
                              VALUES (@name, @downloadId, @mimeType, @type, @data)";
                    cmd = new MySqlCommand(query, _connection);
                    cmd.CommandText = query;
                    cmd.Parameters.Add("@data", MySqlDbType.Blob).Value = file.Data;
                    cmd.Parameters.AddWithValue("@name", file.Name);
                    cmd.Parameters.AddWithValue("@mimeType", file.MimeType);
                    cmd.Parameters.AddWithValue("@downloadId", "");
                    cmd.Parameters.AddWithValue("@type", "UserManualFile");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public App GetApp(string version)
        {
            using (_connection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                var query =
                    "SELECT a.id, f.id as fileId, version, releaseNotes, name, downloadId, mimeType, type, data " +
                    "FROM app as a " +
                    "JOIN files as f ON a.fileId = f.id " +
                    "WHERE version = '1.0.0' AND type = 'AppFile' LIMIT 1";
                var cmd = new MySqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@version", version);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                return new App
                {
                    File = new File
                    {
                        Id = (int) reader["fileId"],
                        Name = reader["name"].ToString(),
                        Data = (byte[]) reader["data"],
                        Type = reader["type"].ToString(),
                        DownloadId = reader["downloadId"].ToString(),
                        MimeType = reader["mimeType"].ToString()
                    },
                    Version = version,
                    Id = (int) reader["id"],
                    ReleaseNotes = (string) reader["releaseNotes"]
                };
            }
        }

        public void Dispose() => _connection.Dispose();
    }
}