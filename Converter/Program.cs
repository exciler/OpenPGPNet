using System;
using System.Collections.Generic;
using System.Text;

namespace Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0];
            var conv = new SmartCard.Converter.Converter(file);
            conv.Convert();
        }
    }
}
