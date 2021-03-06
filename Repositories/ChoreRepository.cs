using Microsoft.Data.SqlClient;
using RoomMates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMates.Repositories
{
    class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;
                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();
                    return chore;
                }
            }
        }
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
                conn.Close();

            }
        }
        public List<Chore> GetUnassigned()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select c.id, c.name from Chore as c 
                                        left join RoommateChore as r on r.ChoreId = c.Id 
                                        where r.RoommateId is NULL;";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> unassingedList = new List<Chore>() { };
                    while (reader.Read())
                    {
                        Chore unassigned = new Chore()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };

                        unassingedList.Add(unassigned);
                    }
                    reader.Close();
                    return unassingedList;
                }
            }
        }
        public void AssignAChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection) {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"Insert into RoommateChore (RoommateId, ChoreId)
                                        Values (@roomateId, @choreid)";
                    cmd.Parameters.AddWithValue("@roomateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM CHORE";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);
                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue,
                        };
                        chores.Add(chore);
                    }
                    reader.Close();
                    return chores;
                }
            }
        }
    }
}
