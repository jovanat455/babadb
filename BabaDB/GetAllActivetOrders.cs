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
    class GetAllActivetOrders
    {
        [FunctionName("GetAllActivetOrders")]
        public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            string responseMessage;
            try
            {

                var makeMealNameNicer = req.Query["StringCorrect"];
               
                log.LogInformation("C# HTTP trigger function processed a request.");

                var getAllActiveOrders = GetAllActiveOrdersExecuteQuery(makeMealNameNicer == "1");

                string stringJson = JsonConvert.SerializeObject(getAllActiveOrders);
                return new OkObjectResult(stringJson);

            }
            catch (Exception e)
            {
                responseMessage = e.Message;
                return new OkObjectResult(responseMessage);
            }

        }

        public static List<UserOrder> GetAllActiveOrdersExecuteQuery(bool embellishString=false)
        {
            StringBuilder sb = new StringBuilder();

            String sql = sb.AppendFormat(Queries.GetAllActiveOrders, DateTime.Now).ToString();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection cnn = new SqlConnection(Env.DbConnectionString);
            cnn.Open();
            command = new SqlCommand(sql, cnn);
            command.CommandType = System.Data.CommandType.Text;

            List<UserOrder> allOrders = new List<UserOrder>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = reader["Owner"].ToString();
                    var meal = reader["MealName"].ToString();
                    if(embellishString)
                    {
                        meal = meal.Split('(')[0];
                    }
                    var amount = reader["Amount"].ToString();
                    var x = allOrders.Find(a => a.Name.Equals(user));
                    if (x != null)
                    {
                        x.OrderedItems.Add(new OrderItem(meal, int.Parse(amount)));
                    }
                    else
                    {
                        var userOrder = new UserOrder(user);
                        userOrder.OrderedItems.Add(new OrderItem(meal, int.Parse(amount)));
                        allOrders.Add(userOrder);
                    }
                    //res.Add(new MealName(reader["MealName"].ToString()));
                }
            }

            cnn.Close();

            return allOrders;
        }
    }
}
