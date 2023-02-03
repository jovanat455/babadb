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

namespace BabaDB
{
    class GetOrderedMeals
    {
        [FunctionName("GetOrderedMeals")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            string responseMessage = "Res: ";
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                var makeMealNameNicer = req.Query["StringCorrect"];

                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = Queries.GetOrderedMeals;
                SqlConnection cnn = new SqlConnection(Env.DbConnectionString);
                cnn.Open();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;
                //var c = command.ExecuteReader();
                List<OrderedMeal> res = new List<OrderedMeal>();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mealName = reader["Meal"].ToString();
                        if(makeMealNameNicer =="1")
                        {
                            mealName = mealName.Split('(')[0];
                        }
                        var price = reader["Price"].ToString();
                        res.Add(new OrderedMeal(mealName, float.Parse(price)));
                    }
                }


                cnn.Close();

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                string stringJson = JsonConvert.SerializeObject(res);
                return new OkObjectResult(stringJson);

            }
            catch (Exception e)
            {
                responseMessage = responseMessage + e.Message;
                return new OkObjectResult(responseMessage);
            }

        }
    }
}
