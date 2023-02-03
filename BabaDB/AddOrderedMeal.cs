using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace BabaDB
{
    class AddOrderedMeal
    {
        [FunctionName("AddOrderedMeal")]
        public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            string responseMessage;
            try
            {

                String connetionString = Env.DbConnectionString;
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();

                var price = req.Query["price"];
                var meal = req.Query["meal"];

                SqlConnection cnn = new SqlConnection(connetionString);

                StringBuilder sb = new StringBuilder();
                cnn.Open();
                String sql = sb.AppendFormat(Queries.InsertOrderedMeal, meal, price).ToString();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteReader();
                cnn.Close();

                if (!IfMealExists(meal))
                {
                    UpdateMealNameTable(meal);
                }


                return new OkObjectResult("Order is Added. ");

            }
            catch (Exception e)
            {
                responseMessage = e.Message.Contains("duplicate key") ? "Duplicate key in DB" : e.Message;
                return new OkObjectResult(responseMessage);
            }

        }

        public static void UpdateMealNameTable(string mealName)
        {
            try
            {

                String connetionString = Env.DbConnectionString;
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();


                SqlConnection cnn = new SqlConnection(connetionString);

                StringBuilder sb = new StringBuilder();
                cnn.Open();
                String sql = sb.AppendFormat(Queries.InstertMealName, mealName).ToString();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteReader();
                cnn.Close();

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static bool IfMealExists(string mealName)
        {
            bool res = false;
            try
            {

                String connetionString = Env.DbConnectionString;
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();


                SqlConnection cnn = new SqlConnection(connetionString);

                StringBuilder sb = new StringBuilder();
                cnn.Open();
                String sql = sb.AppendFormat(Queries.GetMealName, mealName).ToString();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = true;
                    }
                }
                cnn.Close();
                return res;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
