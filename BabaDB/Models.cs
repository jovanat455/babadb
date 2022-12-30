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
}
