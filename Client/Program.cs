using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Client
{
    class Program
    {
        private const string dllPath = @"C:\Users\Serhiy\OS\lab7\Client\x64\Debug\Dll1.dll";


        // Імпортуємо функції з C++ бібліотеки
        [DllImport(dllPath)]
        public static extern IntPtr CreateSocket();

        [DllImport(dllPath)]
        public static extern int ConnectSocket(IntPtr sock, string ip, int port);

        [DllImport(dllPath)]
        public static extern int SendData(IntPtr sock, byte[] data, int length);

        [DllImport(dllPath)]
        public static extern int ReceiveData(IntPtr sock, byte[] buffer, int length);

        [DllImport(dllPath)]
        public static extern void CloseSocket(IntPtr sock);

        //static void Main(string[] args)
        //{
        //    // Створення сокета
        //    IntPtr clientSocket = CreateSocket();

        //    // Підключення до сервера
        //    int result = ConnectSocket(clientSocket, "127.0.0.1", 1111);
        //    if (result == -1)
        //    {
        //        Console.WriteLine("Connection failed.");
        //        return;
        //    }

        //    Console.WriteLine("Connected to the server.");

        //    // Відправка даних на сервер
        //    string message = "Hello from client!";
        //    byte[] data = Encoding.ASCII.GetBytes(message);
        //    SendData(clientSocket, data, data.Length);

        //    // Отримання відповіді від сервера
        //    byte[] buffer = new byte[1024];
        //    int bytesRead = ReceiveData(clientSocket, buffer, buffer.Length);
        //    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        //    Console.WriteLine("Received from server: " + response);

        //    // Закриття сокета
        //    CloseSocket(clientSocket);
        //}
    }
}
