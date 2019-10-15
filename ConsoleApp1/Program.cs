using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using Dapper;
using System.ComponentModel;
using System.Data.Linq.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "WIN-BR9AAF20AAG", UserID = "sa", Password = "Sunsetfame05!", InitialCatalog = "Calendar"
            };

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }

                DataClasses1DataContext _db;
                _db = new DataClasses1DataContext(connection);

                var type1 = _db.Users.Select(x => new User(x.Name, x.Role, x.Mobile, x.Email)).ToList();
                var TypeTheuser = ConvertToDatatable(type1);

                var result = connection.Query(
                    "InsertUsersToDB", 
                    new {TypeTheuser}, 
                    commandType: CommandType.StoredProcedure);





                //SqlCommand cmdProc = new SqlCommand("InsertUsersToDB", connection);
                //cmdProc.CommandType = CommandType.StoredProcedure;
                //cmdProc.Parameters.AddWithValue("@TypeTheUser", dt);
                //cmdProc.ExecuteNonQuery();
            }
        }

        private static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable("UserType");
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }

    


    public class User
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public User(string name, string role, string mobile, string email)
        {
            this.Name = name;
            this.Role = role;
            this.Mobile = mobile;
            this.Email = email;
        }
    }

    class ModelCalendar
    {
        int id;
        int id_User;
        int id_Calendar;

        public ModelCalendar(int Id, int Id_User, int Id_Calendar)
        {
            this.id = Id;
            this.id_User = Id_User;
            this.id_Calendar = Id_Calendar;
        }
    }
}
