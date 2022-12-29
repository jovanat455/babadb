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
    public static class GetMeals
    {

        [FunctionName("GetMeals")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage = "Res: ";
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");


                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                String sql = Queries.GetAllMeals;
                SqlConnection cnn = new SqlConnection(Env.DbConnectionString);
                cnn.Open();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;
                //var c = command.ExecuteReader();
                List<MealName> res = new List<MealName>();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new MealName(reader["MealName"].ToString()));
                    }
                }


                cnn.Close();

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                //name = name ?? data?.name;

                string stringJson = JsonConvert.SerializeObject(res);
                return new OkObjectResult(stringJson);

            }
            catch(Exception e)
            {
                responseMessage = responseMessage + e.Message;
                return new OkObjectResult(responseMessage);
            }
            
        }
    }
}
