using System;
using System.Collections.Generic;
using System.Text;

namespace BabaDB
{
    public static class Queries
    {
        public static string GetAllMeals = "SELECT * FROM MealName";
        public static string GetOrder = "SELECT * FROM [dbo].[Order] WHERE ID=\'{0}\'";
        public static string InsertOrder = "INSERT INTO [dbo].[Order] ([ID] ,[Owner],[Receipt]) VALUES (\'{0}\', \'{1}\', \'{2}\')";
        public static string InsertMeals = "INSERT INTO [dbo].[Meal] ([ID] ,[Owner],[ID_item],[Amount],[MealName]) VALUES (\'{0}\', \'{1}\', \'{2}\',\'{3}\',\'{4}\')";
        public static string GetNumberOfMeals = "SELECT * FROM Meal";
    }
}
