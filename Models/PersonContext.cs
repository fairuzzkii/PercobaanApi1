using System;
using System.Collections.Generic;
using Npgsql;
using PercobaanApi1.Helper;

namespace PercobaanApi1.Models
{
    public class PersonContext
    {
        private string __constr;

        public PersonContext(string pConstr)
        {
            __constr = pConstr;
        }

        public List<Person> ListPerson()
        {
            List<Person> list = new List<Person>();
            string query = "SELECT * FROM person";
            sqlDBHelper db = new sqlDBHelper(this.__constr);

            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Person()
                        {
                            id_person = int.Parse(reader["id_person"].ToString()),
                            nama = reader["nama"].ToString(),
                            alamat = reader["alamat"].ToString(),
                            email = reader["email"].ToString()
                        });
                    }
                }
                db.closeConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching persons: " + ex.Message);
            }

            return list;
        }

        public Person GetMuridById(int id)
        {
            Person person = null;
            string query = "SELECT * FROM person WHERE id_person = @id";
            sqlDBHelper db = new sqlDBHelper(this.__constr);

            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new Person()
                            {
                                id_person = int.Parse(reader["id_person"].ToString()),
                                nama = reader["nama"].ToString(),
                                alamat = reader["alamat"].ToString(),
                                email = reader["email"].ToString()
                            };
                        }
                    }
                }
                db.closeConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching person by ID: " + ex.Message);
            }

            return person;
        }

        public bool AddMurid(Person person)
        {
            string query = "INSERT INTO person (nama, alamat, email) VALUES (@nama, @alamat, @email)";
            sqlDBHelper db = new sqlDBHelper(this.__constr);

            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@nama", person.nama);
                    cmd.Parameters.AddWithValue("@alamat", person.alamat);
                    cmd.Parameters.AddWithValue("@email", person.email);
                    cmd.ExecuteNonQuery();
                }
                db.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding person: " + ex.Message);
            }
        }

        public bool UpdateMurid(Person person)
        {
            string query = "UPDATE person SET nama = @nama, alamat = @alamat, email = @email WHERE id_person = @id";
            sqlDBHelper db = new sqlDBHelper(this.__constr);

            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", person.id_person);
                    cmd.Parameters.AddWithValue("@nama", person.nama);
                    cmd.Parameters.AddWithValue("@alamat", person.alamat);
                    cmd.Parameters.AddWithValue("@email", person.email);
                    cmd.ExecuteNonQuery();
                }
                db.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating person: " + ex.Message);
            }
        }

        public bool DeleteMurid(int id)
        {
            string query = "DELETE FROM person WHERE id_person = @id";
            sqlDBHelper db = new sqlDBHelper(this.__constr);

            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                db.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting person: " + ex.Message);
            }
        }
    }
}
