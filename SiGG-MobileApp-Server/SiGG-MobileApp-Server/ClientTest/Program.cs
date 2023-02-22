using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SocketConnectionFactory.SetDefaultSettings(new SocketClientSettings(
                        "localhost", 7001,
                        Encoding.UTF8, 3
                    ));

                using (ISocketClientConnection conn = SocketConnectionFactory.GetConnection())
                {

                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }
    }

}
