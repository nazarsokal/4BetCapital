// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include <winsock2.h>
#include <ws2tcpip.h>

#pragma comment(lib, "Ws2_32.lib")

#define EXPORT_METHOD extern "C" __declspec(dllexport)

// Ініціалізація Winsock (глобальна)
static bool isInitialized = false;

// Функція для ініціалізації Winsock
void InitializeWinsock() {
    if (!isInitialized) {
        WSADATA wsaData;
        if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
            // Обробка помилок
            return;
        }
        isInitialized = true;
    }
}

// Функція для створення сокета
EXPORT_METHOD SOCKET CreateSocket() {
    InitializeWinsock();
    SOCKET sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

    // Установка сокета в неблокуючий режим
    u_long mode = 1; // 1 = неблокуючий режим
    ioctlsocket(sock, FIONBIO, &mode);

    return sock;
}

// Функція для підключення до сервера
EXPORT_METHOD int ConnectSocket(SOCKET sock, const char* ip, int port) {
    sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(port);
    inet_pton(AF_INET, ip, &serverAddr.sin_addr);

    int result = connect(sock, (sockaddr*)&serverAddr, sizeof(serverAddr));
    if (result == SOCKET_ERROR) {
        int error = WSAGetLastError();
        // Перевірка, чи сокет перебуває в очікуванні завершення підключення
        if (error != WSAEWOULDBLOCK) {
            return -1; // Інша помилка
        }
    }
    return 0; // Успішно або в процесі підключення
}

// Функція для відправки даних
EXPORT_METHOD int SendData(SOCKET sock, const char* data, int length) {
    int totalSent = 0;  // Загальна кількість відправлених байтів
    while (totalSent < length) {
        int result = send(sock, data + totalSent, length - totalSent, 0);
        if (result == SOCKET_ERROR) {
            int error = WSAGetLastError();
            if (error != WSAEWOULDBLOCK) {
                return -1; // Помилка
            }
        }
        else {
            totalSent += result;
        }
    }
    return totalSent; // Кількість відправлених байтів
}


// Функція для отримання даних
EXPORT_METHOD int ReceiveData(SOCKET sock, char* buffer, int length) {
    int result = recv(sock, buffer, length, 0);
    if (result == SOCKET_ERROR) {
        int error = WSAGetLastError();
        if (error != WSAEWOULDBLOCK) {
            // Якщо є інша помилка, повертаємо -1
            return -1;
        }
        else {
            // Якщо сокет не може отримати дані через блокування, повертаємо 0
            return 0;
        }
    }
    else if (result == 0) {
        // З'єднання було закрите
        return 0; // 0 означає, що з'єднання було закрите
    }
    return result; // Повертаємо кількість отриманих байтів
}



// Функція для прив'язки сокета
EXPORT_METHOD int BindSocket(SOCKET sock, const sockaddr* addr, int addrlen) {
    return bind(sock, addr, addrlen);
}

// Функція для прослуховування підключень
EXPORT_METHOD int ListenSocket(SOCKET sock, int backlog) {
    return listen(sock, backlog);
}

// Функція для прийому підключення
EXPORT_METHOD SOCKET AcceptSocket(SOCKET sock, sockaddr* addr, int* addrlen) {
    SOCKET clientSocket = accept(sock, addr, addrlen);
    if (clientSocket == INVALID_SOCKET) {
        int error = WSAGetLastError();
        if (error != WSAEWOULDBLOCK) {
            return INVALID_SOCKET; // Помилка
        }
    }

    // Установка неблокуючого режиму для нового сокета
    u_long mode = 1;
    ioctlsocket(clientSocket, FIONBIO, &mode);

    return clientSocket;
}

// Функція для закриття сокета
EXPORT_METHOD void CloseSocket(SOCKET sock) {
    closesocket(sock);
    if (isInitialized) {
        WSACleanup();
        isInitialized = false;
    }
}

// Перевірка доступності даних для читання (select)
EXPORT_METHOD int IsDataAvailable(SOCKET sock) {
    fd_set readSet;
    FD_ZERO(&readSet);
    FD_SET(sock, &readSet);

    timeval timeout = { 0, 0 }; // Без затримки
    int result = select(0, &readSet, NULL, NULL, &timeout);
    if (result > 0 && FD_ISSET(sock, &readSet)) {
        return 1; // Дані доступні для читання
    }
    return 0; // Дані недоступні
}

// Функція для перевірки статусу підключення (select)
EXPORT_METHOD int IsSocketReady(SOCKET sock) {
    fd_set writeSet;
    FD_ZERO(&writeSet);
    FD_SET(sock, &writeSet);

    timeval timeout = { 0, 0 }; // Без затримки
    int result = select(0, NULL, &writeSet, NULL, &timeout);
    if (result > 0 && FD_ISSET(sock, &writeSet)) {
        return 1; // Сокет готовий до запису
    }
    return 0; // Сокет не готовий
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
