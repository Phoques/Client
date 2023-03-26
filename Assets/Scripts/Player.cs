using Riptide;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }

    public bool Islocal { get; private set; }

    private string _username;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.NetworkManagerInstance.Client.Id)
        {
            player = Instantiate(GameLogic.GameLogicInstance.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.Islocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.GameLogicInstance.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.Islocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player._username = username;

        list.Add(id, player);

    }
        [MessageHandler((ushort)ServerToClientId.playerSpawned)]
        private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }
    

}
