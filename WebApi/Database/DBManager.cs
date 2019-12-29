﻿using System;
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
                query = @"INSERT INTO app (version, fileId, releaseNotes) VALUES (@version, @fileId, @releaseNotes)";
                cmd = new MySqlCommand(query, connection) {CommandText = query};
                cmd.Parameters.AddWithValue("@version", app.Version);
                cmd.Parameters.AddWithValue("@fileId", app.File.Id);
                cmd.Parameters.AddWithValue("@releaseNotes", app.ReleaseNotes);
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
                    "SELECT a.id, f.id as fileId, version, releaseNotes, name, downloadId, mimeType, type, data " +
                    "FROM app as a " +
                    "JOIN files as f ON a.fileId = f.id " +
                    "WHERE version = @version AND type = 'AppFile' LIMIT 1";
                var cmd = new MySqlCommand(query, connection);
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
    }
}