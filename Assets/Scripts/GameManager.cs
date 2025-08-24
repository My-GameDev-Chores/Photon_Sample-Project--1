using UnityEngine;
using Photon.Pun;
using System.Collections;
using Photon.Voice.PUN;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;
    public GameObject playerPrefab;

    [Header("Voice Settings")]
    public float voiceInitDelay = 1.5f;

    void Start()
    {
        // PunVoiceClient auto-configures itself
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Host already in room. Spawning player...");
            StartCoroutine(SpawnPlayerWithDelay());
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room in Game Scene. Spawning player...");
        StartCoroutine(SpawnPlayerWithDelay());
    }

    IEnumerator SpawnPlayerWithDelay()
    {
        // Wait for voice system to initialize
        yield return new WaitForSeconds(0.5f);
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        playerIndex = Mathf.Clamp(playerIndex, 0, spawnPoints.Length - 1);

        PhotonNetwork.Instantiate(
            playerPrefab.name,
            spawnPoints[playerIndex].position,
            spawnPoints[playerIndex].rotation
        );
    }

    public void ExitToLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.LoadLevel("Lobby");
    }
}