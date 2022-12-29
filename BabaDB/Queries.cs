using System;
using System.Collections.Generic;
using System.Text;

namespace BabaDB
{
    public static class Queries
    {
        public static string GetAllMeals = "SELECT * FROM MealName";
        public static string InsertOrder = "INSERT INTO [dbo].[Order] ([ID] ,[Owner],[Receipt]) VALUES (\'{0}\', \'{1}\', \'{2}\')";
    }
}
