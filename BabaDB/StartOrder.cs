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
    public static class StartOrder
    {
        [FunctionName("StartOrder")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            string responseMessage = "Res: ";
            try
            {
                String connetionString = Env.DbConnectionString;
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                StringBuilder sb = new StringBuilder();

                String sql = sb.AppendFormat(Queries.InsertOrder, DateTime.Now, req.Query["owner"], req.Query["receipt"]).ToString();
                SqlConnection cnn = new SqlConnection(connetionString);
                cnn.Open();
                command = new SqlCommand(sql, cnn);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteReader();
                cnn.Close();
                
                return new OkObjectResult("Ok");

            }
            catch (Exception e)
            {
                responseMessage = responseMessage + e.Message;
                return new OkObjectResult(responseMessage);
            }

        }
    }
}
