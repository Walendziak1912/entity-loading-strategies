using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nauka.FactoryPattern
{
    public class FactoryNotification
    {
        public static INotification CreateNotification(INotification notification)
        {
            switch (notification)
            {
                case Email:
                    return new Email();
                case Sms:
                    return new Sms();
                default:
                    throw new ArgumentException("Invalid notification type");
            }
        }
    }
}
