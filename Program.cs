// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Server
{
   

    class Program
    {
        private const string dllPath = @"C:\Users\Serhiy\OS\lab7\Server\x64\Debug\Dll1.dll";

        // Імпортуємо функції з C++ бібліотеки
        [DllImport(dllPath)]
        public static extern IntPtr CreateSocket();

        [DllImport(dllPath)]
        public static extern int BindSocket(IntPtr sock, IntPtr addr, int addrlen);

        [DllImport(dllPath)]
        public static extern int ListenSocket(IntPtr sock, int backlog);

        [DllImport(dllPath)]
        public static extern IntPtr AcceptSocket(IntPtr sock, IntPtr addr, ref int addrlen);

        [DllImport(dllPath)]
        public static extern int ReceiveData(IntPtr sock, byte[] buffer, int length);

        [DllImport(dllPath)]
        public static extern int SendData(IntPtr sock, byte[] data, int length);

        [DllImport(dllPath)]
        public static extern void CloseSocket(IntPtr sock);

        static void Main(string[] args)
        {
            // Створення сокета
            IntPtr serverSocket = CreateSocket();

            // Налаштування адреси сервера
            var serverEndPoint = new sockaddr_in
            {
                sin_family = 2, // AF_INET
                sin_port = (ushort)IPAddress.HostToNetworkOrder(8080), // Порт 8080
                sin_addr = new in_addr { s_addr = BitConverter.ToUInt32(IPAddress.Parse("127.0.0.1").GetAddressBytes(), 0) }
            };

            IntPtr addrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sockaddr_in)));
            Marshal.StructureToPtr(serverEndPoint, addrPtr, false);

            // Прив'язка сокета до порту
            BindSocket(serverSocket, addrPtr, Marshal.SizeOf(typeof(sockaddr_in)));

            // Прослуховування порту
            ListenSocket(serverSocket, 10);

            Console.WriteLine("Server is listening on port 8080...");

            // Прийом з'єднання
            int addrlen = Marshal.SizeOf(typeof(sockaddr_in));
            IntPtr clientSocket = AcceptSocket(serverSocket, addrPtr, ref addrlen);

            Console.WriteLine("Client connected.");

            // Отримання даних від клієнта
            byte[] buffer = new byte[1024];
            int bytesRead = ReceiveData(clientSocket, buffer, buffer.Length);
            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received from client: " + receivedData);

            // Відправка відповіді
            byte[] response = Encoding.ASCII.GetBytes("Hello from server!");
            SendData(clientSocket, response, response.Length);

            // Закриття сокетів
            CloseSocket(clientSocket);
            CloseSocket(serverSocket);
        }

        // Структура sockaddr_in (для IP адреси)
        [StructLayout(LayoutKind.Sequential)]
        public struct sockaddr_in
        {
            public short sin_family;
            public ushort sin_port;
            public in_addr sin_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sin_zero;
        }

        // Структура in_addr для IP адреси
        [StructLayout(LayoutKind.Sequential)]
        public struct in_addr
        {
            public uint s_addr;
        }
    }
}



