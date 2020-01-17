#include "cServerMain.h"

#include <iostream>
#include <cpprest/ws_client.h>
#include <cpprest/filestream.h>
#include <cpprest/json.h>

int main() {
    web::websockets::client::websocket_client client;
    client.connect(U("ws://echo.websocket.org")).wait();

    web::websockets::client::websocket_outgoing_message out_msg;
    out_msg.set_utf8_message("test");
    client.send(out_msg).wait();

    client.receive().then([](web::websockets::client::websocket_incoming_message in_msg) {
        return in_msg.extract_string();
        }).then([](std::string body) {
            std::cout << body << std::endl; // test
            }).wait();

            client.close().wait();

            return 0;
}