using System;
using Riptide; // The Riptide DLL, and XML needs to be added to the Unity project to allow these usings.
using Riptide.Utils;
using Unity.VisualScripting;
using UnityEngine;

//All of these scripts are what the package does automatically. (Mostly)

public enum ServerToClientId : ushort
{
    playerSpawned = 1,
}
public enum ClientToServerID : ushort
{
    name = 1,
}

public class NetworkManager : MonoBehaviour
{
    //We want to make sure there is only one instance of the Network manager.
    private static NetworkManager _networkManagerInstance;
    public static NetworkManager NetworkManagerInstance
    {
        get => _networkManagerInstance;
        private set
        {
            //
            //If we dont have an instance set the instance to this.
            if (_networkManagerInstance == null)
                _networkManagerInstance = value;
            //else if we do have an instance and the instance we have isnt us
            else if (_networkManagerInstance != value)
            {
                // the $ allows you to put variables within curly brackets to then post in the debug, else it just is wanting a string. or just ())
                Debug.Log($"{nameof(NetworkManager)} _networkManagerInstance already exists, destroying duplicate!");
                //Destroy the instance that isnt the
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake()
    {
        //When object that this script is on, set instance to this.
        NetworkManagerInstance = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;

    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.eventargs?view=net-7.0
    private void DidConnect(object sender, EventArgs e)
    {

        UIManager.UIManagerInstance.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.BackToMain();
    }

}
