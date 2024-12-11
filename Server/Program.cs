using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Server
{
    class Program
    {
        private const string dllPath = @"C:\Users\Admin\Documents\GitHub\4BetCapital\Server\x64\Debug\Dll1.dll";

        // Імпортуємо функції з C++ бібліотеки
        [DllImport(dllPath)] public static extern IntPtr CreateSocket();
        [DllImport(dllPath)] public static extern int BindSocket(IntPtr sock, IntPtr addr, int addrlen);
        [DllImport(dllPath)] public static extern int ListenSocket(IntPtr sock, int backlog);
        [DllImport(dllPath)] public static extern IntPtr AcceptSocket(IntPtr sock, IntPtr addr, ref int addrlen);
        [DllImport(dllPath)] public static extern int ReceiveData(IntPtr sock, byte[] buffer, int length);
        [DllImport(dllPath)] public static extern int SendData(IntPtr sock, byte[] data, int length);
        [DllImport(dllPath)] public static extern void CloseSocket(IntPtr sock);
        [DllImport(dllPath)] public static extern int IsDataAvailable(IntPtr sock);

        // Структура sockaddr_in для адреси
        [StructLayout(LayoutKind.Sequential)]
        public struct sockaddr_in
        {
            public short sin_family;
            public ushort sin_port;
            public in_addr sin_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sin_zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct in_addr
        {
            public uint s_addr;
        }

        static void Main(string[] args)
        {
            // Створення сокета сервера
            IntPtr serverSocket = CreateSocket();

            // Налаштування адреси сервера
            var serverEndPoint = new sockaddr_in
            {
                sin_family = 2, // AF_INET
                sin_port = (ushort)IPAddress.HostToNetworkOrder((short)1111), // Порт
                sin_addr = new in_addr { s_addr = BitConverter.ToUInt32(IPAddress.Parse("192.168.2.59").GetAddressBytes(), 0) },
                sin_zero = new byte[8]
            };

            IntPtr addrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sockaddr_in)));
            Marshal.StructureToPtr(serverEndPoint, addrPtr, false);

            // Прив'язка сокета до порту
            if (BindSocket(serverSocket, addrPtr, Marshal.SizeOf(typeof(sockaddr_in))) != 0)
            {
                Console.WriteLine("Failed to bind socket.");
                return;
            }

            // Прослуховування вхідних підключень
            ListenSocket(serverSocket, 10);
            Console.WriteLine("Server is listening on 192.168.2.59:1111...");

            // Список для зберігання клієнтських сокетів
            List<IntPtr> clientSockets = new List<IntPtr>();

            while (true)
            {
                // Перевірка нового підключення
                int addrlen = Marshal.SizeOf(typeof(sockaddr_in));
                IntPtr clientSocket = AcceptSocket(serverSocket, addrPtr, ref addrlen);
                if (clientSocket != IntPtr.Zero && clientSocket.ToInt64() != -1)
                {
                    Console.WriteLine("New client connected.");
                    clientSockets.Add(clientSocket);
                }

                // Перевірка даних у кожного клієнта
                for (int i = 0; i < clientSockets.Count; i++)
                {
                    IntPtr sock = clientSockets[i];
                    if (IsDataAvailable(sock) == 1)
                    {
                        DbManager dbManager = new DbManager();
                        byte[] buffer = new byte[1024];
                        int bytesRead = ReceiveData(sock, buffer, buffer.Length);

                        if (bytesRead > 0)
                        {
                            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            if (IsJson(receivedData))
                            {
                                var obj = ParseJson(receivedData);

                                if(obj is Person person)
                                {
                                    dbManager.AddPersonToDb(person);
                                }
                            }
                            else
                            {

                            }

                            // Відправка відповіді
                            var UsersList = dbManager.GetUsersFromDB();
                            foreach (var item in UsersList)
                            {
                                string responseMessage = JsonSerializer.Serialize(item, typeof(Person));
                                byte[] response = Encoding.ASCII.GetBytes(responseMessage);
                                int bytesSent = SendData(sock, response, response.Length);
                                Console.WriteLine($"Sent to client {i + 1}: {responseMessage}, Bytes sent: {bytesSent}");
                            }
  
                        }
                        else
                        {
                            Console.WriteLine($"Client {i + 1} disconnected.");
                            CloseSocket(sock);
                            clientSockets.RemoveAt(i);
                            i--;
                        }
                    }
                }

            }
        }

        private static bool IsJson(string input)
        {
            try
            {
                JsonDocument.Parse(input);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private static object ParseJson(string input)
        {
            // Десеріалізація з динамічним визначенням типу
            var baseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(input);

            if (baseObject.ContainsKey("type"))
            {
                string type = baseObject["type"].ToString();

                switch (type)
                {
                    case "Person":
                        return JsonSerializer.Deserialize<Person>(input);
                    default:
                        throw new InvalidOperationException($"Unknown type: {type}");
                }
            }

            throw new InvalidOperationException("JSON does not contain a 'type' field.");
        }
    }
}
