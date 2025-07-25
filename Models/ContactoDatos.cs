using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AppSocial.Models
{
    public class ContactoDatos
    {
        private readonly string _cadenaSQL;

        public ContactoDatos(IConfiguration configuration)
        {
            _cadenaSQL = configuration.GetConnectionString("CadenaSQL");
        }

        public List<ContactoModel> Listar()
        {
            var lista = new List<ContactoModel>();

            using var conexion = new SqlConnection(_cadenaSQL);
            using var cmd = new SqlCommand("sp_Listar", conexion) { CommandType = CommandType.StoredProcedure };

            conexion.Open();
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new ContactoModel
                {
                    IdContacto = Convert.ToInt32(dr["IdContacto"]),
                    Nombre = dr["Nombre"].ToString(),
                    Telefono = dr["Telefono"].ToString(),
                    Correo = dr["Correo"].ToString()
                });
            }

            return lista;
        }

        public ContactoModel Obtener(int idContacto)
        {
            var contacto = new ContactoModel();

            using var conexion = new SqlConnection(_cadenaSQL);
            using var cmd = new SqlCommand("sp_Obtener", conexion) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("IdContacto", idContacto);

            conexion.Open();
            using var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                contacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                contacto.Nombre = dr["Nombre"].ToString();
                contacto.Telefono = dr["Telefono"].ToString();
                contacto.Correo = dr["Correo"].ToString();
            }

            return contacto;
        }

        public bool Guardar(ContactoModel contacto)
        {
            try
            {
                using var conexion = new SqlConnection(_cadenaSQL);
                using var cmd = new SqlCommand("sp_Guardar", conexion) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("Nombre", contacto.Nombre);
                cmd.Parameters.AddWithValue("Telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", contacto.Correo);

                conexion.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error en logs si usas ILogger
                Console.WriteLine($"Error al guardar: {ex.Message}");
                return false;
            }
        }

        public bool Editar(ContactoModel contacto)
        {
            try
            {
                using var conexion = new SqlConnection(_cadenaSQL);
                using var cmd = new SqlCommand("sp_Editar", conexion) { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("IdContacto", contacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombre", contacto.Nombre);
                cmd.Parameters.AddWithValue("Telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", contacto.Correo);

                conexion.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar: {ex.Message}");
                return false;
            }
        }

        public bool Eliminar(int idContacto)
        {
            try
            {
                using var conexion = new SqlConnection(_cadenaSQL);
                using var cmd = new SqlCommand("sp_Eliminar", conexion) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("IdContacto", idContacto);

                conexion.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
                return false;
            }
        }
    }
}
