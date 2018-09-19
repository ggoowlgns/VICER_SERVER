using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using System.Text;

public class receiver : MonoBehaviour
{
    //Socket sck;
    //public RawImage image;
    public GameObject sphere;
    Material runtimeMaterial;


    public bool enableLog = false;
    

    const int port = 8010;
    string IP = "192.168.35.211";
    TcpClient client;
    NetworkStream NS = null;
    StreamWriter SW = null;

    Texture2D tex;

    private bool stop = false;

    //This must be the-same with SEND_COUNT on the server
    const int SEND_RECEIVE_COUNT = 4;


    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        runtimeMaterial = new Material(Shader.Find("VertexLit"));
        tex = new Texture2D(0, 0);
        //sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

        client = new TcpClient();
        

        //Connect to server from another Thread
        Loom.RunAsync(() =>
        {
            Debug.Log("Connecting to server...");
            // if on desktop
            //client.Connect(IPAddress.Parse(IP), port);

            // if using the IPAD
            client.Connect(IPAddress.Parse(IP), port);
            NS = client.GetStream();
            SW = new StreamWriter(NS, Encoding.UTF8);
            Debug.Log("Connection");
            string SendMessage = null;
            SendMessage = " ";
            Debug.Log("er");
            SW.Write(SendMessage); // 메시지 보내기
            Debug.Log("er2");
            SW.Flush();
            Debug.Log("er3");
            //Debug.Log("Connection");


            /*sck.Connect(localEndPoint);
            Debug.Log("Connected!");
            string text = "get";
            byte[] data = Encoding.UTF8.GetBytes(text);
            sck.Send(data);*/
            
            imageReceiver();
        });
    }


    void imageReceiver()
    {
        //While loop in another Thread is fine so we don't block main Unity Thread
        Loom.RunAsync(() =>
        {
            while (!stop)
            {
                //Read Image Count
                 int imageSize = readImageByteSize(SEND_RECEIVE_COUNT);
                 LOGWARNING("Received Image byte Length: " + imageSize);

                 //Read Image Bytes and Display it
                 readFrameByteArray(imageSize);

                /*Byte[] _data = new byte[15];
                Debug.Log("Accept");
                sck.Receive(_data);

                int length = BitConverter.ToInt32(_data, 0);
                Byte[] rdata = new byte[length];
                sck.Receive(rdata);
                bool readyToReadAgain = false;
                Loom.QueueOnMainThread(() =>
                {
                    displayReceivedImage(rdata);
                    readyToReadAgain = true;
                });
                while (!readyToReadAgain)
                {
                    System.Threading.Thread.Sleep(1);
                }*/
            }
        });
    }


    //Converts the data size to byte array and put result to the fullBytes array
    void byteLengthToFrameByteArray(int byteLength, byte[] fullBytes)
    {
        //Clear old data
        Array.Clear(fullBytes, 0, fullBytes.Length);
        //Convert int to bytes
        byte[] bytesToSendCount = BitConverter.GetBytes(byteLength);
        //Copy result to fullBytes
        bytesToSendCount.CopyTo(fullBytes, 0);
    }

    //Converts the byte array to the data size and returns the result
    int frameByteArrayToByteLength(byte[] frameBytesLength)
    {
        int byteLength = BitConverter.ToInt32(frameBytesLength, 0);
        return byteLength;
    }


    /////////////////////////////////////////////////////Read Image SIZE from Server///////////////////////////////////////////////////
    private int readImageByteSize(int size)
    {
        bool disconnected = false;

        NetworkStream serverStream = client.GetStream();
        byte[] imageBytesCount = new byte[size];
        var total = 0;
        do
        {
            var read = serverStream.Read(imageBytesCount, total, size - total);
            Debug.LogFormat("Client recieved {0} bytes", total);
            Debug.LogFormat("size {0}", size);
            if (read == 0)
            {
                disconnected = true;
                break;
            }
            total += read;
        } while (total != size);

        int byteLength;

        if (disconnected)
        {
            byteLength = -1;
        }
        else
        {
            byteLength = frameByteArrayToByteLength(imageBytesCount);
        }
        return byteLength;
    }

    /////////////////////////////////////////////////////Read Image Data Byte Array from Server///////////////////////////////////////////////////
    private void readFrameByteArray(int size)
    {
        bool disconnected = false;

        NetworkStream serverStream = client.GetStream();
        byte[] imageBytes = new byte[size];
        var total = 0;
        do
        {
            var read = serverStream.Read(imageBytes, total, size - total);
            Debug.LogFormat("Client recieved {0} bytes", total);
            Debug.LogFormat("size {0}", size);
            if (read == 0)
            {
                disconnected = true;
                break;
            }
            total += read;
        } while (total != size);

        bool readyToReadAgain = false;

        //Display Image
        if (!disconnected)
        {
            //Display Image on the main Thread
            Loom.QueueOnMainThread(() =>
            {
                displayReceivedImage(imageBytes);
                readyToReadAgain = true;
            });
        }

        //Wait until old Image is displayed
        while (!readyToReadAgain)
        {
            System.Threading.Thread.Sleep(1);
        }
    }
    

    void displayReceivedImage(byte[] receivedImageBytes)
    {//yuv to rgb 가 이루어 져야함
        

        tex.LoadImage(receivedImageBytes);
        runtimeMaterial.SetTexture("_MainTex", tex);
        sphere.GetComponent<Renderer>().material = runtimeMaterial;

        //tex.LoadRawTextureData(receivedImageBytes);
        //image.texture = tex;
        //tex.LoadRawTextureData(receivedImageBytes);
        //image.texture = tex;

    }

    /// <summary>
    ///**********************
    /// ******YUV ****************
    /// *******************
    /// </summary>
    //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
    //ORIGINAL LINE: public void decodeYUV(int[] out, byte[] fg, int width, int height) throws NullPointerException, IllegalArgumentException
    
    // Update is called once per frame
    void Update()
    {

    }


    void LOG(string messsage)
    {
        if (enableLog)
            Debug.Log(messsage);
    }

    void LOGWARNING(string messsage)
    {
        if (enableLog)
            Debug.LogWarning(messsage);
    }

    void OnApplicationQuit()
    {
        LOGWARNING("OnApplicationQuit");
        stop = true;

        if (client != null)
        {
            client.Close();
        }
    }
}