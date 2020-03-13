using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WebApi.Models;

namespace WebApi.Database

{
    public class DbManager
    {
        private readonly string _connectionString;

        public DbManager(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public string GetSecretKey()
        {
            const string query = @"SELECT jwtSecretKey FROM auth";
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                var jwtSecretKey = (string) reader["jwtSecretKey"];
                return jwtSecretKey;
            }
        }

        public void UpdateBug(Bug bug)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "UPDATE bugs set status = @status " +
                    "WHERE id = @id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@status", bug.Status);
                cmd.Parameters.AddWithValue("@id", bug.Id);
                var reader = cmd.ExecuteNonQuery();
            }
        }

        public void DeleteBug(Bug bug)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query = "DELETE FROM bugs " +
                            "WHERE id = @id ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", bug.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public Bug GetBug(int bugId)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "SELECT id, version, summary, description, reporter, reportDate, status " +
                    "FROM bugs " +
                    "WHERE id = @id ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", bugId);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string summary = reader["summary"].ToString();
                    string description = reader["description"].ToString();
                    string reporter = reader["reporter"].ToString();
                    string version = reader["version"].ToString();
                    int status = Convert.ToInt32(reader["status"]);
                    string reportDate = reader["reportDate"].ToString();
                    return new Bug(bugId, summary, description, reporter, version, status, reportDate);
                }

                return null;
            }
        }

        public User GetUser(CredentialsForLoginDto credentials)
        {
            const string query = @"
            SELECT * 
            FROM users
            WHERE username = @username AND password = @password
            ";
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(query, connection);
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
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(query, connection);
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
            const string query = @"
            SELECT * 
            FROM users 
            WHERE username = @username 
            LIMIT 1
            ";
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(query, connection);
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", user.Username);
                var reader = cmd.ExecuteReader();
                return reader.Read();
            }
        }

        public File GetUserManual()
        {
            const string query = @"
            SELECT * 
            FROM files 
            WHERE type='UserManualFile' 
            LIMIT 1";
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(query, connection);
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
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
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

        public void SaveApp(App app)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query = @"INSERT INTO files (name, downloadId, mimeType, type, data)
                              VALUES (@name, @downloadId, @mimeType, @type, @data);
                              SELECT LAST_INSERT_ID() as id;";
                var cmd = new MySqlCommand(query, connection) {CommandText = query};
                cmd.Parameters.Add("@data", MySqlDbType.Blob).Value = app.File.Data;
                cmd.Parameters.AddWithValue("@name", app.File.Name);
                cmd.Parameters.AddWithValue("@mimeType", app.File.MimeType);
                cmd.Parameters.AddWithValue("@downloadId", "");
                cmd.Parameters.AddWithValue("@type", "AppFile");
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return;
                var fileId = reader["id"];
                app.File.Id = Convert.ToInt64(fileId);
                reader.Close();
                query = @"INSERT INTO app (version, fileId, releaseNotes, releaseDate) VALUES (@version, @fileId, @releaseNotes, @releaseDate)";
                cmd = new MySqlCommand(query, connection) {CommandText = query};
                cmd.Parameters.AddWithValue("@version", app.Version);
                cmd.Parameters.AddWithValue("@fileId", app.File.Id);
                cmd.Parameters.AddWithValue("@releaseNotes", app.ReleaseNotes);
                cmd.Parameters.AddWithValue("@releaseDate", app.ReleaseDate);
                cmd.ExecuteNonQuery();
            }
        }

        public App GetApp(string version)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "SELECT a.id, f.id as fileId, version, releaseNotes, releaseDate, name, downloadId, mimeType, type, data " +
                    "FROM app as a " +
                    "JOIN files as f ON a.fileId = f.id " +
                    "WHERE version = @version AND type = 'AppFile' LIMIT 1";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@version", version);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                return new App((int) reader["id"], version, new File
                    {
                        Id = (int) reader["fileId"],
                        Name = reader["name"].ToString(),
                        Data = (byte[]) reader["data"],
                        Type = reader["type"].ToString(),
                        DownloadId = reader["downloadId"].ToString(),
                        MimeType = reader["mimeType"].ToString()
                    },
                    (string) reader["releaseNotes"], (DateTime) reader["releaseDate"]);
            }
        }

        public string GetAboutUsInfo()
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "SELECT content " +
                    "FROM info " +
                    "LIMIT 1";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                return !reader.Read() ? null : reader["content"].ToString();
            }
        }

        public Link UpdateLink(Link link)
        {
            return null;
        }

        public List<Announcement> GetAnnouncements()
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "SELECT title, content, datePosted " +
                    "FROM news ";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                List<Announcement> announcements = new List<Announcement>();
                while (reader.Read())
                {
                    announcements.Add(new Announcement(reader["title"].ToString(), reader["content"].ToString(),
                        DateTime.Parse(reader["datePosted"].ToString())));
                }

                return announcements;
            }
        }

        public List<Link> GetLinks(int type)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "SELECT id, name, url, type " +
                    "FROM links " +
                    "WHERE type = @type ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@type", type);
                var reader = cmd.ExecuteReader();
                List<Link> links = new List<Link>();
                while (reader.Read())
                {
                    links.Add(new Link((int) reader["id"], reader["name"].ToString(),
                        reader["url"].ToString(), (int) reader["type"]));
                }

                return links;
            }
        }

        public void DeleteLink(Link link)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "DELETE FROM links " +
                    "WHERE id = @id ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", link.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public Link AddLink(Link link)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query = @"INSERT INTO links (name, url, type)
                       VALUES(@name, @url, @type);
                       SELECT LAST_INSERT_ID() as id; ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", link.Name);
                cmd.Parameters.AddWithValue("@url", link.Url);
                cmd.Parameters.AddWithValue("@type", link.Type);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                var linkId = Convert.ToInt32(reader["id"]);
                link.Id = linkId;
                return link;
            }
        }

        public void UpdateAboutUsInfo(InfoAboutUs infoAboutUs)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query = "UPDATE info set content = @content";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@content", infoAboutUs.Content);
                cmd.ExecuteNonQuery();
            }
        }

        public Bug SaveBug(Bug bug)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query = @"INSERT INTO bugs (description, summary, reportDate, reporter, version)
                       VALUES(@description, @summary, @reportDate, @reporter, @version);
                       SELECT LAST_INSERT_ID() as id; ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@description", bug.Description);
                cmd.Parameters.AddWithValue("@version", bug.Version);
                cmd.Parameters.AddWithValue("@summary", bug.Summary);
                cmd.Parameters.AddWithValue("@reportDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@reporter", bug.Reporter);
                var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                var bugId = Convert.ToInt32(reader["id"]);
                bug.Id = bugId;
                return bug;
            }
        }

        public List<Bug> GetBugs(string version)
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query =
                    "SELECT id, version, summary, description, reporter, reportDate, status " +
                    "FROM bugs " +
                    "WHERE version = @version ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@version", version);
                var reader = cmd.ExecuteReader();
                var bugs = new List<Bug>();
                while (reader.Read())
                {
                    int bugId = Convert.ToInt32(reader["id"]);
                    string summary = reader["summary"].ToString();
                    string description = reader["description"].ToString();
                    string reporter = reader["reporter"].ToString();
                    int status = Convert.ToInt32(reader["status"]);
                    string reportDate = reader["reportDate"].ToString();
                    bugs.Add(new Bug(bugId, summary, description, reporter, version, status, reportDate));
                }

                return bugs;
            }
        }

        public List<App> GetAppVersions()
        {
            var connection = new MySqlConnection(_connectionString);
            using (connection)
            {
                connection.Open();
                var query = "SELECT version, releaseNotes, releaseDate FROM app";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                var apps = new List<App>();
                while (reader.Read())
                {
                    apps.Add(new App(reader["version"].ToString(), reader["releaseNotes"].ToString(),
                        DateTime.Parse(reader["releaseDate"].ToString())));
                }

                return apps;
            }
        }
    }
}