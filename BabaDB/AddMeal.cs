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
    public static class AddMeal
    {
        [FunctionName("AddMeal")]
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
                StringBuilder sb = new StringBuilder();
                var owner = req.Query["owner"];
                var meals = req.Query["meals"];

                List<Meal> mealList = GetAllMeals(meals);
                    SqlConnection cnn = new SqlConnection(connetionString);


                foreach (var meal in mealList)
                {
                    cnn.Open();
                    int id = GetNextId();
                    String sql = sb.AppendFormat(Queries.InsertMeals, id, owner, DateTime.Now, meal.Size, meal.Name).ToString();
                    command = new SqlCommand(sql, cnn);
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteReader();
                    cnn.Close();

                }

                return new OkObjectResult("Order is Added. " + meals );

            }
            catch (Exception e)
            {
                responseMessage = e.Message.Contains("duplicate key") ? GetOrder.GetOrderExecuteQuery() : e.Message;
                return new OkObjectResult(responseMessage);
            }

        }

        public static List<Meal> GetAllMeals(string meals)
        {
            List<Meal> mealsList = new List<Meal>();
            string[] temp = meals.Split(';');

            foreach(var item in temp)
            {
                string[] temp1 = item.Split('(');
                Meal meal = new Meal(temp1[0], temp1[1][0] - '0');
                mealsList.Add(meal);
            }


            return mealsList;

        }

        public static int GetNextId()
        {

            StringBuilder sb = new StringBuilder();
            int id = 0;
            String sql = sb.AppendFormat(Queries.GetNumberOfMeals, DateTime.Now).ToString();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection cnn = new SqlConnection(Env.DbConnectionString);
            cnn.Open();
            command = new SqlCommand(sql, cnn);
            command.CommandType = System.Data.CommandType.Text;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id++;
                }
            }
            cnn.Close();
            return ++id;
        }

    }
}
