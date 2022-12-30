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
        public static string InsertMeals = "INSERT INTO [dbo].[Meal] ([Owner],[ID_order],[Amount],[MealName]) VALUES (\'{0}\', \'{1}\', \'{2}\',\'{3}\')";
        public static string GetNumberOfMeals = "SELECT * FROM Meal";
        public static string GetAllActiveOrders = "SELECT * FROM [dbo].[Meal] WHERE ID_order=\'{0}\'";
    }
}
