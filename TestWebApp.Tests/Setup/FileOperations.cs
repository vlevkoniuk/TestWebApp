using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestWebApp.Tests.Setup
{
    public static class FileOperations
    {
        public static void BeforeTest()
        {
            if(!Directory.Exists("./Data"))
                Directory.CreateDirectory("./Data");
            File.Copy("../../../../TestWebApp/Data/Data[1].xml", "./Data/Data[1].xml", true);
        }

        public static void AfterTest()
        {
            //Directory.CreateDirectory("./Data");
            File.Copy("./Data/Data[1].xml", "../../../../TestWebApp/Data/Data[1].xml", true);
        }
    }
}
