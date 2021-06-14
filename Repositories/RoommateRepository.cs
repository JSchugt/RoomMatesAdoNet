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
        public RoommateRepository(string connectionString) : base(connectionString) {
            room = new RoomRepository(connectionString);
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
    }
}
