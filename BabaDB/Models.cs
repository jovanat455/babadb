using System;
using System.Collections.Generic;
using System.Text;

namespace BabaDB
{
    public class MealName
    {
        public string Name { get; set; }
        public MealName(string name)
        {
            Name = name;
        }
    }

    public class Meal
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public Meal(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }

    public class OrderItem
    {
        public string MealName { get; set; }
        public int Amount { get; set; }

        public OrderItem(string name, int size)
        {
            MealName = name;
            Amount = size;
        }
    }

    public class UserOrder
    {
        public string Name { get; set; }
        public List<OrderItem> OrderedItems {get; set;}

        public UserOrder(string name)
        {
            Name = name;
            OrderedItems = new List<OrderItem>();
        }
    }
}
