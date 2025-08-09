using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public Transform[] spawnPoints;          // 4 spawn positions in scene
   
    public GameObject playerPrefab;          // Your Cube player prefab

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // Ensure index is within array bounds
        playerIndex = Mathf.Clamp(playerIndex, 0, spawnPoints.Length - 1);

        // Spawn at assigned position
        GameObject player = PhotonNetwork.Instantiate(
            playerPrefab.name,
            spawnPoints[playerIndex].position,
            spawnPoints[playerIndex].rotation
        );

        // Give the player a unique color
       
    }
}
