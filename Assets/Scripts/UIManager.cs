using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Riptide;

//All of these scripts are what the package does automatically. (Mostly)
public class UIManager : MonoBehaviour
{
    //We want to make sure there is only one instance of the UI manager.
    private static UIManager _uiManagerInstance;
    public static UIManager UIManagerInstance
    {
        get => _uiManagerInstance;
        private set
        {
            //
            //If we dont have an instance set the instance to this.
            if (_uiManagerInstance == null)
                _uiManagerInstance = value;
            //else if we do have an instance and the instance we have isnt us
            else if (_uiManagerInstance != value)
            {
                // the $ allows you to put variables within curly brackets to then post in the debug, else it just is wanting a string. or just ())
                Debug.Log($"{nameof(UIManager)} _networkManagerInstance already exists, destroying duplicate!");
                //Destroy the instance that isnt the
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject _connectUI;
    [SerializeField] private InputField usernameField;


    private void Awake()
    {
        //When object that this script is on, set instance to this.
        UIManagerInstance = this;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        _connectUI.SetActive(false);
        NetworkManager.NetworkManagerInstance.Connect();
    }

    public void BackToMain()
    {
        usernameField.interactable = false;
        _connectUI.SetActive(true);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name);
        message.AddString(usernameField.text);
        NetworkManager.NetworkManagerInstance.Client.Send(message);

    }
}
