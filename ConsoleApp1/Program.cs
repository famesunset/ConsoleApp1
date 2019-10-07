using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "WIN-BR9AAF20AAG", UserID = "sa", Password = "Sunsetfame05!", InitialCatalog = "JL"
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

                var type1 = _db.UserCalendar.Select(x => new ModelCalendar(x.id, x.id_User, x.id_Calendar)).ToList();

                var type2 = (from table in _db.UserCalendar
                    select new
                    {
                        table.id,
                        table.id_User,
                        table.id_Calendar
                    }).Select(x => new ModelCalendar(x.id, x.id_User, x.id_Calendar)).ToList();
            }
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
