using Microsoft.Data.SqlClient;
using RoomMates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMates.Repositories
{
    class RoommateRepository : BaseRepository

    {
        RoomRepository room = null;
        public RoommateRepository(string connectionString) : base(connectionString)
        {
            room = new RoomRepository(connectionString);
        }
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Id, FirstName, LastName, RentPortion, MoveInDate, RoomId From Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> roommateList = new List<Roommate>() { };
                    while (reader.Read())
                    {
                        Roommate roomer = new Roommate()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = room.GetById(reader.GetInt32(reader.GetOrdinal("RoomId"))),

                        };
                        roommateList.Add(roomer);
                    }
                    reader.Close();
                    return roommateList;
                }
            }
        }
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate JOIN Room on Room.Id = Roommate.Id WHERE Roommate.Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Roommate roommate = null;
                    if (reader.Read())
                    {
                        roommate = new Roommate()
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = room.GetById(reader.GetInt32(reader.GetOrdinal("RoomId")))
                        };


                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
        
    }// end of class brace
}
