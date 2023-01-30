using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public void Add(Bike bike)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Bike (
                            Brand, Color, OwnerId, BikeTypeId )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Brand, @Color, @OwnerId, @BikeTypeId )";
                    cmd.Parameters.AddWithValue("@Brand", bike.Brand);
                    cmd.Parameters.AddWithValue("@Color", bike.Color);
                    cmd.Parameters.AddWithValue("@OwnerId", bike.OwnerId);
                    cmd.Parameters.AddWithValue("@BikeTypeId", bike.BikeTypeId);

                    bike.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public List<Bike> GetAllBikes()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id, b.Brand, b.Color, b.OwnerId, b.BikeTypeId,

                                               o.Name AS OwnerName, o.Address, o.Email, o.Telephone,
                                               t.Name AS BikeTypeName
                                          FROM Bike b 
                                               JOIN Owner o on b.OwnerId = o.Id
                                               LEFT JOIN BikeType t on b.BikeTypeId = t.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Bike> bikes = new List<Bike>();

                    while (reader.Read())
                    {
                        Bike bike = new Bike()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                            },
                            BikeTypeId = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                            BikeType = new BikeType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("BikeTypeName"))
                            }
                        };
                        bikes.Add(bike);
                    }

                    reader.Close();

                    return bikes;
                }
            }
        }

        public Bike GetBikeById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id, b.Brand, b.Color, b.OwnerId, b.BikeTypeId,

                                               o.Name AS OwnerName, o.Address, o.Email, o.Telephone,
                                               t.Name AS BikeTypeName,
                                               w.Id AS WorkId, w.DateInitiated, w.Description, w.DateCompleted, w.BikeId
                                          FROM Bike b 
                                               JOIN Owner o on b.OwnerId = o.Id
                                               LEFT JOIN BikeType t on b.BikeTypeId = t.Id
                                               LEFT JOIN WorkOrder w on b.Id = w.BikeId 
                                          WHERE b.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        Bike bike = null;
                        while (reader.Read())
                        {
                            if (bike == null)
                            {
                                bike = new Bike()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                    Color = reader.GetString(reader.GetOrdinal("Color")),
                                    OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Owner = new Owner()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                        Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                        Address = reader.GetString(reader.GetOrdinal("Address")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                                    },
                                    BikeTypeId = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    BikeType = new BikeType()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                        Name = reader.GetString(reader.GetOrdinal("BikeTypeName"))
                                    },
                                    WorkOrders = new List<WorkOrder>()
                                };

                            }


                            if (!reader.IsDBNull(reader.GetOrdinal("WorkId")))
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("DateCompleted")))
                                {
                                    WorkOrder workorder = new WorkOrder()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("WorkId")),
                                        DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        DateCompleted = reader.GetDateTime(reader.GetOrdinal("DateCompleted"))

                                    };
                                    bike.WorkOrders.Add(workorder);
                                }


                            }


                        }
                        reader.Close();
                        return bike;

                    }
                }
            }
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id, b.Brand, b.Color,

                                               w.Id AS WorkId, w.DateInitiated, w.Description, w.DateCompleted, w.BikeId
                                          FROM Bike b 
                                               LEFT JOIN WorkOrder w on b.Id = w.BikeId";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        Bike bike = null;
                        while (reader.Read())
                        {
                            if (bike == null)
                            {
                                bike = new Bike()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                    WorkOrders = new List<WorkOrder>()
                                };

                            }


                            if (!reader.IsDBNull(reader.GetOrdinal("WorkId")))
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("DateCompleted")))
                                {
                                    WorkOrder workorder = new WorkOrder()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("WorkId")),
                                        DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        DateCompleted = reader.GetDateTime(reader.GetOrdinal("DateCompleted"))

                                    };
                                    bike.WorkOrders.Add(workorder);
                                    count++;
                                }


                            }


                        }
                        reader.Close();


                    }
                }
            }

            return count;
        }
    }
    
}
