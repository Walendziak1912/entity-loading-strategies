using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nauka.FactoryPattern
{
    public class Sms : INotification
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"SMS Notification: {message}");
        }
    }
}
