using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ADO_Homework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Orders"].ConnectionString;
            
            //Task 1

            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;
                cn.Open();

                SqlCommand command = new SqlCommand("Select * from orders", cn);

                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Console.WriteLine($"ID: {dr[0]} | Date: {dr[1]} | Shipped: {dr[2]}");  //и так далее...
                }

                dr.Close();

                Console.WriteLine("=======================");

                command.CommandText = "Select * from customers";

                dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Console.WriteLine($"ID: {dr[0]} | First Name: {dr[1]} | Last Name: {dr[2]}");  //и так далее...
                }

                cn.Close();
            }

            Console.WriteLine();
            //Task 2

            using (SqlConnection cn2 = new SqlConnection())
            {
                cn2.ConnectionString = connectionString;
                cn2.Open();

                SqlCommand command = new SqlCommand("Select * from order_info", cn2);

                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Console.WriteLine($"ID: {dr[0]}, City: {dr[1]}, Ship mode: {dr[2]}, Customer: {dr[3]}");
                }

                cn2.Close();
            }

            Console.WriteLine();
            // Task 3

            var adapter = new SqlDataAdapter("Exec GetOrdersByDate @Date = '11-8-2016'", connectionString);

            var orderSet = new DataSet();
            adapter.Fill(orderSet);

            var orderTable = orderSet.Tables[0];

            foreach(DataRow row in orderTable.Rows)
            {
                Console.WriteLine(row[0]);
            }

            Console.WriteLine();
            //Task 4

            var adapter2 = new SqlDataAdapter("Select * from ship_modes", connectionString);

            var builder = new SqlCommandBuilder();

            builder.DataAdapter = adapter2;

            var shipModeSet = new DataSet();
            builder.DataAdapter.Fill(shipModeSet);

            var shipModeTable = shipModeSet.Tables[0];
            shipModeTable.Columns[0].AutoIncrement = true;
            shipModeTable.Columns[0].AutoIncrementSeed = shipModeTable.Rows.Count + 1;

            DataRow shipModeRow = shipModeTable.NewRow();
            shipModeRow["ship_mode_name"] = "VIP shipment";

            shipModeTable.Rows.Add(shipModeRow);

            DataRow shipModeRow2 = shipModeTable.NewRow();
            shipModeRow2["ship_mode_name"] = "VIP shipment 2";

            shipModeTable.Rows.Add(shipModeRow2);

            shipModeTable.Rows.Remove(shipModeRow2);

            builder.DataAdapter.Update(shipModeTable);

            foreach (DataRow row in shipModeTable.Rows)
            {
                Console.WriteLine($"ID: {row[0]}, Ship mode: {row[1]}");
            }
        }
    }
}
