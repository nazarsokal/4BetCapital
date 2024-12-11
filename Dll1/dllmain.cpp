// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include <winsock2.h>
#include <ws2tcpip.h>

#pragma comment(lib, "Ws2_32.lib")


#define EXPLOREED_METHOD extern "C" __declspec(dllexport)


EXPLOREED_METHOD
extern "C" __declspec(dllexport) SOCKET CreateSocket() {
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);

    return socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) int ConnectSocket(SOCKET sock, const char* ip, int port) {
    sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(port);
    inet_pton(AF_INET, ip, &serverAddr.sin_addr);

    return connect(sock, (sockaddr*)&serverAddr, sizeof(serverAddr));
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) int SendData(SOCKET sock, const char* data, int length) {
    return send(sock, data, length, 0);
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) int ReceiveData(SOCKET sock, char* buffer, int length) {
    return recv(sock, buffer, length, 0);
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) void CloseSocket(SOCKET sock) {
    closesocket(sock);
    WSACleanup();
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) int BindSocket(SOCKET sock, const sockaddr * addr, int addrlen) {
    return bind(sock, addr, addrlen);
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) int ListenSocket(SOCKET sock, int backlog) {
    return listen(sock, backlog);
}
EXPLOREED_METHOD
extern "C" __declspec(dllexport) SOCKET AcceptSocket(SOCKET sock, sockaddr * addr, int* addrlen) {
    return accept(sock, addr, addrlen);
}


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

