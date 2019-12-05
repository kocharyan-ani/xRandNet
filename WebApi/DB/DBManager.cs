using System;
using WebApi.Models;
using MySql.Data.MySqlClient;

namespace WebApi.DB

{
    public class DBManager
    {
        private MySqlConnection connection;

        public DBManager()
        {
            this.connection = new MySqlConnection("Server=localhost; database=xrandnet; UID=xrandnet; password=Admin@123");
        }

        public File GetUserManual()
        {
            File file = null;
            string query = "SELECT * FROM files WHERE type='UserManualFile' LIMIT 1";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();
            using (connection)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                if (reader.Read())
                {
                    file = new File();
                    file.Id = (int)reader["id"];
                    file.Name = reader["name"].ToString();
                    file.Data = (byte[])reader["data"];
                    file.Type = reader["type"].ToString();
                    file.DownloadId = reader["downloadId"].ToString();
                    file.MimeType = reader["mimeType"].ToString();
                }
            }
            return file;
        }

        public void SetUserManual(File file)
        {
            string query = "SELECT * FROM files WHERE type='UserManualFile' LIMIT 1";
            using (connection)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
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
            App app = new App();
            using (connection)
            {
                if (this.connection.State != System.Data.ConnectionState.Open)
                    this.connection.Open();
                string query = "SELECT a.id, f.id as fileId, version, releaseNotes, name, downloadId, mimeType, type, data " +
                               "FROM app as a " +
                               "JOIN files as f ON a.fileId = f.id " +
                               "WHERE version = '1.0.0' AND type = 'AppFile' LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@version", version);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    app.File = new File();
                    app.File.Id = (int)reader["fileId"];
                    app.File.Name = reader["name"].ToString();
                    app.File.Data = (byte[])reader["data"];
                    app.File.Type = reader["type"].ToString();
                    app.File.DownloadId = reader["downloadId"].ToString();
                    app.File.MimeType = reader["mimeType"].ToString();
                    app.Version = version;
                    app.Id = (int)reader["id"];
                    app.ReleaseNotes = (string)reader["releaseNotes"];
                }
            }
            return app;
        }
    }
}
