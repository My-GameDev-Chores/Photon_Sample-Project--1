using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_InputField roomInputField; // Room name input
    public GameObject lobbyPanel;         // Lobby UI panel
    public GameObject roomPanel;          // Room UI panel
    public TextMeshProUGUI roomNameText;  // Current room name display
    public Transform roomListParent;      // ScrollView Content transform
    public GameObject roomItemPrefab;     // Prefab for each room in the list
    public GameObject startButton;        // Start game button for Master Client

    private List<GameObject> roomItemList = new List<GameObject>();
    private bool createRoomQueued = false;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);

        // Create room if it was queued before joining lobby
        if (createRoomQueued && !string.IsNullOrEmpty(roomInputField.text))
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4 });
            createRoomQueued = false;
        }
    }

    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(roomInputField.text)) return;

        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4 });
        }
        else
        {
            Debug.Log("Not in lobby yet — queuing room creation...");
            createRoomQueued = true;
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomNameText.text = "Room : " + PhotonNetwork.CurrentRoom.Name;

        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Clear old list
        foreach (GameObject item in roomItemList)
        {
            Destroy(item);
        }
        roomItemList.Clear();

        // Populate new list
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) continue;

            GameObject newRoom = Instantiate(roomItemPrefab, roomListParent);
            newRoom.GetComponent<RoomItem>().SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void OnClickStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game"); // Must match Build Settings
        }
    }
}
