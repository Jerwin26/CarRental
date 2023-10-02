using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CarRental.Repository
{
    public class Errorlog
    {

        public void LogError(Exception ex)
        {
            string logFilePath = HttpContext.Current.Server.MapPath("~/Errorlogger/Errorlog.txt");
            string errorMessage = $"{DateTime.Now}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";

            try
            {
     
                lock (new object())
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine(errorMessage);
                    }
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error while writing to log: {logEx.Message}");
            }
        }
    }
}