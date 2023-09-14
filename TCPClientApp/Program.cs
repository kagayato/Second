using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Client
{
    static void Main(string[] args)
    {
        while (true)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient("127.0.0.1", 12345); // Подключение к серверу
                Console.WriteLine("Подключено к серверу.");

                NetworkStream stream = client.GetStream();

                while (true)
                {
                    Console.WriteLine("Введите команду (1 - камера, 2 - толкатель, 0 - выход):");
                    string command = Console.ReadLine();

                    if (command != "2" && command != "0" && command != "1")
                    {
                        Console.WriteLine("Некорректная команда. Пожалуйста, введите 1 (камера), 2 (толкатель) или 0 (выход).");
                        continue;
                    }

                    // Отправляем команду серверу
                    byte[] data = Encoding.ASCII.GetBytes(command);
                    stream.Write(data, 0, data.Length);

                    if (command == "0")
                    {
                        Console.WriteLine("Выход из клиента.");
                        return; 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
                Console.WriteLine("Ожидание подключения к серверу...");
                Thread.Sleep(5000); // Подождать 5 секунд перед новой попыткой подключения
            }
            finally
            {
                if (client != null)
                    client.Close();
            }
        }
    }
}
