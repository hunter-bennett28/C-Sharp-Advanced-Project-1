﻿using System;

namespace Project1_Group_17
{
    class Program
    {
        static void Main(string[] args)
        {
            DataModeler test = new DataModeler();
            test.ParseXML("Canadacities-XML.xml");
        }
    }
}
