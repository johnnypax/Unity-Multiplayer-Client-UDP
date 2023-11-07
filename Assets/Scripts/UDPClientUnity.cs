using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class UDPClientUnity : MonoBehaviour
{
    public string udpServerAddress = "127.0.0.1";
    public int messagesSendDelay = 50;              //20 times per second
    private string uniqueID;

    UdpClient client;
    IPEndPoint serverEndPoint;

    void Start()
    {
        client = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse(udpServerAddress), 11000);

        uniqueID = System.Guid.NewGuid().ToString(); // Genera un nuovo Guid come identificativo.

        // Inizia l'invio delle coordinate.
        StartSendingCoordinates();
    }

    async void StartSendingCoordinates()
    {
        try
        {
            while (true)
            {
                Vector3 position = this.transform.position;         //Player position
                Quaternion rotation = this.transform.rotation;      //Player rotation

                string message = $"{uniqueID}:{position.x};{position.y};{position.z};{rotation.x};{rotation.y};{rotation.z}";
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                Debug.Log(message);
                
                await client.SendAsync(bytes, bytes.Length, serverEndPoint);    //Asynchronusly send coordinates to the server
                await Task.Delay(messagesSendDelay);                            //Wait for the next send for avoiding DoS
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
