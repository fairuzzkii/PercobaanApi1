using Microsoft.IdentityModel.Tokens;
using Npgsql;
using PercobaanApi1.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PercobaanApi1.Models
{
    public class LoginContext
    {
        private string _constr;
        private string _ErrorMsg;

        public LoginContext(string constr)
        {
            _constr = constr;
        }

        public List<Login> Autentifikasi(string p_username, string p_password, IConfiguration p_config)
        {
            List<Login> list = new List<Login>();
            string query = string.Format(@"SELECT ps.id_person, ps.nama, ps.alamat, ps.email, 
                                           pp.id_peran, p.nama_peran 
                                           FROM person ps 
                                           INNER JOIN peran_person pp ON ps.id_person = pp.id_person
                                           INNER JOIN peran p ON pp.id_peran = p.id_peran
                                           WHERE ps.username='{0}' AND ps.password='{1}';", p_username, p_password);

            sqlDBHelper db = new sqlDBHelper(_constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Login()
                    {
                        id_person = int.Parse(reader["id_person"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        id_peran = int.Parse(reader["id_peran"].ToString()),
                        nama_peran = reader["nama_peran"].ToString(),
                        token = GenerateJwtToken(p_username, reader["nama_peran"].ToString(), p_config)
                    });
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _ErrorMsg = ex.Message;
            }
            return list;
        }

        private string GenerateJwtToken(string namaUser, string peran, IConfiguration pConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(pConfig["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, namaUser),
        new Claim(ClaimTypes.Role, peran)
    };

            var token = new JwtSecurityToken(
                issuer: pConfig["Jwt:Issuer"],
                audience: pConfig["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

