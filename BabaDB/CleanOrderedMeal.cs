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
    class CleanOrderedMeal
    {
        [FunctionName("CleanOrderedMeal")]
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

                SqlConnection cnn = new SqlConnection(connetionString);

                StringBuilder sb = new StringBuilder();
                cnn.Open();
                String sql = sb.AppendFormat(Queries.CleanOrderedMeal).ToString();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteReader();
                cnn.Close();


                return new OkObjectResult("Meals are deleted.");

            }
            catch (Exception e)
            {
                responseMessage = e.Message.Contains("duplicate key") ? GetOrder.GetOrderExecuteQuery() : e.Message;
                return new OkObjectResult(responseMessage);
            }

        }
    }
}
