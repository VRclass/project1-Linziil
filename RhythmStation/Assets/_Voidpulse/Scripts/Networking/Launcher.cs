using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


namespace com.voidpulse.rhythmstation
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
            [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
            [SerializeField]
            private byte maxPlayersPerRoom = 4;
            #endregion

            #region Private Fields
            // this client's version number. Users are seperated from each other by gameVersion
            string gameVersion = "1";
        #endregion

        #region Public Serializable Fields
            [Tooltip("The Ui Panel to let the user enter name, connect and play")]
            [SerializeField]
            private GameObject controlPanel;
            [Tooltip("The UI Label to inform the user that the connection is in progress")]
            [SerializeField]
            private GameObject progressLabel;

        #endregion

        #region MonoBehaviour CallBacks
        // MonoBehaviourr method called on GameObject by Unity during early init phase
        void Awake()
        {
            // critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automaticall
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Start()
        {
          progressLabel.SetActive(false);
          controlPanel.SetActive(true);
        }

        #endregion


        #region Public Methods

        // start the connection process
        // - if already connected, we attempt joining a random room
        // - if not yet connected, connect this application instance to Photon Cloud Network

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // check if we are connected or not, we join if we are, else we init the connection
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            } else {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }

        #endregion
    }
}
