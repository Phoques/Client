using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    //We want to make sure there is only one instance of the Network manager.
    private static GameLogic _gameLogicInstance;
    public static GameLogic GameLogicInstance
    {
        get => _gameLogicInstance;
        private set
        {
            //
            //If we dont have an instance set the instance to this.
            if (_gameLogicInstance == null)
                _gameLogicInstance = value;
            //else if we do have an instance and the instance we have isnt us
            else if (_gameLogicInstance != value)
            {
                // the $ allows you to put variables within curly brackets to then post in the debug, else it just is wanting a string. or just ())
                //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
                Debug.Log($"{nameof(GameLogic)} _networkManagerInstance already exists, destroying duplicate!");
                //Destroy the instance that isnt the
                Destroy(value);
            }
        }
    }

    public GameObject LocalPlayerPrefab => localPlayerPrefab;

    public GameObject PlayerPrefab => playerPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;
    private void Awake()

    {
        GameLogicInstance = this;
    }
}
