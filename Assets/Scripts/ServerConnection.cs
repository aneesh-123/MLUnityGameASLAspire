using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ServerConnection : MonoBehaviour
{
    private string serverIP = "3.133.7.92"; // Replace with your server's IP address
    private int serverPort = 1114; // Replace with your server's port number

    private TcpClient client;
    private NetworkStream stream;
    public CameraProcess cameraProcess;

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.Connect(serverIP, serverPort);

            stream = client.GetStream();
            Debug.Log("Connected to server!");

            // Start receiving data from the server in a separate task
            Task.Run(ReceiveDataFromServer);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to connect to server: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        DisconnectFromServer();
    }

    private void DisconnectFromServer()
    {
        if (client != null && client.Connected)
        {
            stream.Close();
            client.Close();
            Debug.Log("Disconnected from server!");
        }
    }

    public void SendMessageToServer(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            stream.Write(data, 0, data.Length);
            Debug.Log("Message sent to the server!");
        }
        catch (Exception e)
        {
            Debug.Log("Failed to send message to the server: " + e.Message);
        }
    }

    public void SendImageData(byte[] imageData)
    {
        try
        {
            // Send the image data size to the server
            byte[] sizeBytes = BitConverter.GetBytes(imageData.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);

            //Debug.Log("image data length " + imageData.Length);
            // Send the image data to the server
            stream.Write(imageData, 0, imageData.Length);

            Debug.Log("Image data sent to the server!");
        }
        catch (Exception e)
        {
            Debug.Log("Failed to send image data to the server: " + e.Message);
        }
    }

    public void SendByteArray(byte[] byteArray)
    {
        try
        {
            // Send the byte array size to the server
            byte[] sizeBytes = BitConverter.GetBytes(byteArray.Length);
            //Debug.Log("sizebytes "+ sizeBytes.Length + " byte Array " + byteArray.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);

            // Send the byte array to the server
            stream.Write(byteArray, 0, byteArray.Length);

            //Debug.Log("Byte array sent to the server!");
        }
        catch (Exception e)
        {
            Debug.Log("Failed to send byte array to the server: " + e.Message);
        }
    }

    private async Task ReceiveDataFromServer()
    {
        try
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //Debug.Log("receiving data rn");

                // Pass the received data to MainSend
                cameraProcess.HandleReceivedData(message);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Failed to receive data from server: " + e.Message);
        }
    }
}
