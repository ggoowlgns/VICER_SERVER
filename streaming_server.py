
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
