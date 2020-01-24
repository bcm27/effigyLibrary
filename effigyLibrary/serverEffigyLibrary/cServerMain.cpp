#include "cServerMain.h"

#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <cpprest\ws_client.h>
#include <producerconsumerstream.h>

using namespace utility;
using namespace web;                        
using namespace web::http;                  
using namespace concurrency::streams;       

int main(int argc, char* argv[])
{
    web::websockets::client::websocket_client client;
    client.connect(U("ws://localhost:1234")).then([]() { /* We've finished connecting. */ });

    web::websockets::client::websocket_outgoing_message msg;
    msg.set_utf8_message("I am a UTF-8 string! (Or close enough...)");
    client.send(msg).then([]() { /* Successfully sent the message. */ });

    client.receive().then([](web::websockets::client::websocket_incoming_message msg) {
        return msg.extract_string();
        }).then([](std::string body) {
            std::cout << body << std::endl;
            });

        web::websockets::client::websocket_outgoing_message msg;
        concurrency::streams::consumer_buffer<uint8_t> buf;
        std::vector<uint8_t> body(6);
        memcpy(&body[0], "a\0b\0c\0", 6);

        auto send_task = buf.putn(&body[0], body.size()).then([&](size_t length) {
            msg.set_binary_message(buf.create_istream(), length);
            return client.send(msg);
            }).then([](pplx::task<void> t)
                {
                    try
                    {
                        t.get();
                    }
                    catch (const websocket_exception & ex)
                    {
                        std::cout << ex.what();
                    }
                });
            send_task.wait();
    return 0;
}