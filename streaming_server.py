import sys
from socket import *
from threading import Thread
import binascii
import PIL.Image
import io

data = (bytes)

sendReady = False
recvReady = True

class myThread_sender(Thread):
    def __init__(self,connection):
        Thread.__init__(self)
        self.connection = connection
        self.resend = True
    def run(self):
        try:
            global data, sendReady, recvReady
            while True:
                if recvReady:
                    #//print("data :" ,data)
                    data = self.connection.recv(65536)  # recv next message on connected socket
                    sendReady = True
                    recvReady = False


                # # Convert bytes to stream (file-like object in memory)
                # picture_stream = io.BytesIO(data)
                #
                # # Create Image object
                # picture = PIL.Image.open(picture_stream)
                #
                # # display image
                # picture.show()


                if not data:
                    print("socekt end")
                    break  # eof when the socket closed


        except OSError as e:  # socket.error exception
            print('socket error:', e)
        except Exception as e:
            print('Exception:', e)


class myThread_recver(Thread):
    def __init__(self,connection):
        Thread.__init__(self)
        self.connection = connection
        self.resend = True
    def run(self):
        try:
            global sendReady,recvReady
            while True:
                if sendReady:
                    self.connection.send(data)  # send a reply to the client
                    recvReady = True
                    sendReady = False
                    #print("reciver go")

        except OSError as e:  # socket.error exception
            print('socket error:', e)
        except Exception as e:
            print('Exception:', e)



def echo_server(my_port):
    """Echo server (iterative)"""
    try:
        sock = socket(AF_INET, SOCK_STREAM) # make listening socket
        # sock.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1) # Reuse port number if used
        sock.bind(('', my_port))        # bind it to server port number
        sock.listen(5)                  # listen, allow 5 pending connects


    except OSError as e:
        print('socket error', e)
        sock.close()
        sys.exit(1)
    else:
        print('Server started')

        while True:  # do forever (until process killed)
            conn, cli_addr = sock.accept()  # wait for next client connect
            # conn: new socket, addr: client addr
            print('Connected by', cli_addr)
            data_encoded = conn.recv(65536)
            print(data_encoded)

            try:
                data_decoded =  data_encoded.decode("utf-8")
                print("decoded data"+ data_decoded)
                # if b'\xef\xbb\xbf'.decode("utf-8") in data_decoded:
                #     print("in mexception")
                #     raise ex
                print("start thread_reciver")
                th = myThread_recver(conn)
                th.start()


            except Exception as ex: #exception 걸려서 오는 상황 (byte가 들어온 sender 상황)
                #data_encoded2 = conn.recv(22000000)
                print("start thread_sender")
                th = myThread_sender(conn)
                th.start()


if __name__ == '__main__':
    echo_server(8010)

