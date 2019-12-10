using System.Data;
using MySql.Data.MySqlClient;
using WebApi.Models;

namespace WebApi.DB

{
    public class DBManager
    {
        private readonly MySqlConnection connection;

        public DBManager(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
        }

        public User GetUser(Credentials credentials)
        {
            const string query = @"
            SELECT * 
            FROM users
            WHERE username = @username AND password = @password
            ";
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", credentials.Username);
                cmd.Parameters.AddWithValue("@password", credentials.Password);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                var firstName = (string) reader["firstName"];
                var lastName = (string) reader["lastName"];
                var isAdmin = (bool) reader["isAdmin"];
                return new User(firstName, lastName, credentials, isAdmin);
            }
        }

        public File GetUserManual()
        {
            File file = null;
            const string query = @"
            SELECT * 
            FROM files 
            WHERE type='UserManualFile' 
            LIMIT 1";
            connection.Open();
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    file = new File();
                    file.Id = (int) reader["id"];
                    file.Name = reader["name"].ToString();
                    file.Data = (byte[]) reader["data"];
                    file.Type = reader["type"].ToString();
                    file.DownloadId = reader["downloadId"].ToString();
                    file.MimeType = reader["mimeType"].ToString();
                }
            }

            return file;
        }

        public void SetUserManual(File file)
        {
            var query = @"
            SELECT * 
            FROM files 
            WHERE type='UserManualFile' 
            LIMIT 1";
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    query = "UPDATE files SET data=@data,name=@name,mimeType=@mimeType WHERE type='UserManualFile' ";
                    cmd = new MySqlCommand(query, connection);
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
                    cmd = new MySqlCommand(query, connection);
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
            var app = new App();
            using (connection)
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var query =
                    "SELECT a.id, f.id as fileId, version, releaseNotes, name, downloadId, mimeType, type, data " +
                    "FROM app as a " +
                    "JOIN files as f ON a.fileId = f.id " +
                    "WHERE version = '1.0.0' AND type = 'AppFile' LIMIT 1";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@version", version);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    app.File = new File();
                    app.File.Id = (int) reader["fileId"];
                    app.File.Name = reader["name"].ToString();
                    app.File.Data = (byte[]) reader["data"];
                    app.File.Type = reader["type"].ToString();
                    app.File.DownloadId = reader["downloadId"].ToString();
                    app.File.MimeType = reader["mimeType"].ToString();
                    app.Version = version;
                    app.Id = (int) reader["id"];
                    app.ReleaseNotes = (string) reader["releaseNotes"];
                }
            }

            return app;
        }

        public void Dispose() => connection.Dispose();
    }
}