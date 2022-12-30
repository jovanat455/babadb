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
    public static class GetOrder
    {

        [FunctionName("GetOrder")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string responseMessage;
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                var getOrderStatus = GetOrderExecuteQuery();

                string stringJson = JsonConvert.SerializeObject(getOrderStatus);
                return new OkObjectResult(stringJson);

            }
            catch (Exception e)
            {
                responseMessage = e.Message;
                return new OkObjectResult(responseMessage);
            }

        }

        public static string GetOrderExecuteQuery()
        {
            StringBuilder sb = new StringBuilder();

            String sql = sb.AppendFormat(Queries.GetOrder, DateTime.Now).ToString();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlConnection cnn = new SqlConnection(Env.DbConnectionString);
            cnn.Open();
            command = new SqlCommand(sql, cnn);
            command.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = command.ExecuteReader();
            var result = reader.Read();
            var owner = reader["Owner"].ToString();
            cnn.Close();

            return result ? "Order is started owner is " + owner : "Order is not started";
        }

    }
}
