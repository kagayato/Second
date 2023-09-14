using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

class Server
{
    static void Main(string[] args)
    {
        TcpListener server = null;
        try
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;

            server = new TcpListener(ipAddress, port);
            server.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            List<char> productQueue = new List<char>(); // Список для продуктов

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Подключен клиент.");

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                try
                {
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string command = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                        if (command == "1")
                        {
                            // Клиент хочет добавить продукт
                            Console.WriteLine("Выберите продукт (Г - годный, Б - брак):");
                            char productType = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            if (productType == 'Г' || productType == 'Б')
                            {
                                if (productQueue.Count < 5)
                                {
                                    productQueue.Insert(0, productType); // Добавляем выбранный продукт в начало списка
                                    Console.WriteLine($"Добавлен продукт [{productType}] в очередь.");
                                }
                                else
                                {
                                    Console.WriteLine("Очередь полна, игнорируем добавление продукта.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Некорректный выбор продукта, игнорируем команду.");
                            }
                        }
                        else if (command == "2")
                        {
                            // Клиент хочет удалить продукт (толкатель)
                            if (productQueue.Count > 0)
                            {
                                productQueue.RemoveAt(productQueue.Count - 1); // Удаляем самый правый элемент
                                Console.WriteLine("Толкатель удалил самый правый элемент из очереди.");
                            }
                            else
                            {
                                Console.WriteLine("Очередь пуста, игнорируем удаление продукта.");
                            }
                        }
                        else if (command == "0")
                        {
                            // Клиент хочет выйти
                            Console.WriteLine("Клиент отключен.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неизвестная команда, игнорируем.");
                        }

                        // Отображение состояния очереди продуктов
                        foreach (char product in productQueue)
                        {
                            Console.Write($"[{product}] -> ");
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: " + e.Message);
                }
                finally
                {
                    client.Close();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка: " + e.Message);
        }
        finally
        {
            server.Stop();
        }
    }
}
