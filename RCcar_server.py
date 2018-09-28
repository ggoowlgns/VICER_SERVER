
import sys
from socket import *
from threading import Thread

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
                    data = self.connection.recv(1024)  # recv next message on connected$
                    sendReady = True
                    recvReady = False

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
            global sendReady,recvReady,data
            while True:
                if sendReady:
                    self.connection.send(data)  # send a reply to the client
                    recvReady = True
                    sendReady = False
                    print(data)
                    print("reciver go")

        except OSError as e:  # socket.error exception
            print('socket error:', e)
        except Exception as e:
            print('Exception:', e)



def rc_server(my_port):
    """Echo server (iterative)"""
    try:
        sock = socket(AF_INET, SOCK_STREAM) # make listening socket
        # sock.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1) # Reuse port number if used
        sock.bind(('', my_port))        # bind it to server port number
        sock.listen(5)                  # listen, allow 5 pending connects


    except OSError as e:
        print('socket error', e)
        sock.close()
