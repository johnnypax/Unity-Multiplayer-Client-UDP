using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class UDPClientUnity : MonoBehaviour
{
    public string udpServerAddress = "127.0.0.1";
    public int messagesSendDelay = 50;              //20 times per second

    UdpClient client;
    IPEndPoint serverEndPoint;

    void Start()
    {
        client = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse(udpServerAddress), 11000);

        // Inizia l'invio delle coordinate.
        StartSendingCoordinates();
    }

    async void StartSendingCoordinates()
    {
        try
        {
            while (true)
            {
                // Serializza le coordinate del GameObject.
                Vector3 position = this.transform.position;
                Quaternion rotation = this.transform.rotation;
                string message = $"{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z}";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                // Invia le coordinate al server.
                await client.SendAsync(bytes, bytes.Length, serverEndPoint);

                await Task.Delay(messagesSendDelay);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    void OnApplicationQuit()
    {
        client.Close();
    }
}
