using System;

namespace Project1_Group_17
{
    class Program
    {
        static void Main(string[] args)
        {
            DataModeler modeler = new DataModeler();
            modeler.ParseFile("Canadacities-JSON.json", DataModeler.SupportedFileTypes.JSON);
            modeler.ParseFile("Canadacities-XML.xml", DataModeler.SupportedFileTypes.XML);
            modeler.ParseFile("Canadacities.csv", DataModeler.SupportedFileTypes.CSV);

        }
    }
}
