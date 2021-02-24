using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;


namespace ChatWebsocket
{
   public class WebsocketZ : MonoBehaviour
   {

      struct SocketEvent
      {
         public string eventName;
         public string data;

         public SocketEvent(string eventName, string data)
         {
            this.eventName = eventName;
            this.data = data;
         }
      }

      public class EventServer
      {
         public string eventName;
      }

      public class EventAddMoney
      {
         public string eventName;
         public string userID;
         public int addmoney;
      }

      public class EventCallbackAddMoney
      {
         public string eventName;
         public string status;
         public int data;
      }

      public class EventCallbackGeneral
      {
         public string eventName;
         public string data;
      }

      public delegate void DelegateHandler(string msg);
      public delegate void DelegateHandlerAddMoney(string status, int money);

      public event DelegateHandlerAddMoney OnAddMoney;

      public event DelegateHandler OnConnectionSuccess;
      public event DelegateHandler OnConnectionFail;
      public event DelegateHandler OnReceiveMessage;
      public event DelegateHandler OnCreateRoom;
      public event DelegateHandler OnJoinRoom;
      public event DelegateHandler OnLeaveRoom;
      public event DelegateHandler OnLogin;
      public event DelegateHandler OnRegister;

      private bool isConnection;
      public string myID;

      private string callbackData;
      private float countDataTime;
      private float currentDataTime;

      private List<string> messageQueue = new List<string>();



      [Header("Websocket")]
      private WebSocket webSocket;


      [Header("CONNECT PANEL")]
      public GameObject connectPanel;

      [Header("LOGIN PANEL")]
      public GameObject loginPanel;
      public InputField userID;
      public InputField password;

      [Header("REGISTER PANEL")]
      public GameObject registerPanel;
      public InputField regUserID;
      public InputField regName;
      public InputField regPassword;
      public InputField regRepassword;



      [Header("CHAT PANEL")]
      public GameObject chatPanel;
      public InputField inputText;
      public Text sendText;
      public Text receiveText;
      public Text roomName;
      public Transform chatContent;
      public GameObject emotionPanel;
      bool isEmotionOpen = false;


      [Header("CREATE ROOM PANEL")]
      public GameObject createRoomPanel;
      public InputField RoomNameInputfield;

      private string tempMessageString;
      private string tempCreateString;

      [Header("POPUP")]
      //register
      public GameObject regerrorPopup;
      public GameObject regSuccessPopup;
      public GameObject pwNotMatchPopup;
      public GameObject errorInputPopup;

      [Header("Agent")]

      public TextMesh agentName;

   
      //login
      public GameObject failloginPopup;
      public GameObject successloginPopup;

      public GameObject errjoinpopup;
      public GameObject errcreatepopup;
      
      
      
      
      



      public void Start()
      {
          isConnection = false;
      }

      public void Update()
      {
         if (messageQueue.Count > 0)
         {
            NotifyCallback(messageQueue[0]);
            messageQueue.RemoveAt(0);
         }

      }

      public void Connect()
      {
          if(isConnection)
            return;

         isConnection = true;
         webSocket = new WebSocket($"ws://127.0.0.1:9523/");
         webSocket.OnMessage += OnMessage;
         webSocket.Connect();
            
         connectPanel.gameObject.SetActive(false);
         //chatPanel.gameObject.SetActive(true);
         loginPanel.gameObject.SetActive(true);
      }

      public void AddMoney()
      {
         EventAddMoney eventData = new EventAddMoney();

         eventData.eventName = "AddMoney";
         eventData.userID = "test2222";
         eventData.addmoney = 100;

         string toJson = JsonUtility.ToJson(eventData);

         webSocket.Send(toJson);

      }

      public void SendMessage()
      {
         if (inputText.text != "")
         {
            var message = new EventCallbackGeneral()
            {
               eventName = "Message",
               data = roomName.text + "#" + myID +"#" + agentName.text +"#" + inputText.text
            };

            var toJson = JsonUtility.ToJson(message);
            webSocket.Send(toJson);
         }

         inputText.text = "";

      }

      public bool IsConnected()
      {
          if( webSocket == null)
            return false;

          return webSocket.ReadyState == WebSocketState.Open;
      }

      private void NotifyCallback(string callbackData)
      {
         Debug.Log("Data From Server : " + callbackData);

         EventServer recieveEvent = JsonUtility.FromJson<EventServer>(callbackData);

         switch (recieveEvent.eventName)
         {
            case "Register":
            {
               EventCallbackGeneral receiveEventGeneral = JsonUtility.FromJson<EventCallbackGeneral>(callbackData);
               var splitStr = receiveEventGeneral.data.Split('#');
               var status = splitStr[0];
               var data = splitStr[1];

               Debug.Log(data);

               if(status == "success")
               {
                  regSuccessPopup.gameObject.SetActive(true);
               }
               else if (status == "fail")
               {
                  regerrorPopup.gameObject.SetActive(false);
               } 

               break;
            }
            case "Login":
            {
               EventCallbackGeneral receiveEventGeneral = JsonUtility.FromJson<EventCallbackGeneral>(callbackData);
               var splitStr = receiveEventGeneral.data.Split('#');
               var status = splitStr[0];
               var name = splitStr[1];

               if(status == "success")
               {
                  agentName.text = name;
                  myID = userID.text;
                  successloginPopup.gameObject.SetActive(true);
               }
               else if (status == "fail")
               {
                  failloginPopup.gameObject.SetActive(true);
               }

               break;
            }
            case "CreateRoom":
            {
               EventCallbackGeneral receiveEventGeneral = JsonUtility.FromJson<EventCallbackGeneral>(callbackData);
               var splitRoomData = receiveEventGeneral.data.Split('#');
               var status = splitRoomData[0];
               if(status == "success")
               {
                  createRoomPanel.gameObject.SetActive(false);
                  chatPanel.gameObject.SetActive(true);
                  roomName.text = splitRoomData[1];
                  
                  Debug.Log("Room : " + splitRoomData[1]);
                  
               }
               else if (status == "fail")
               {
                  errcreatepopup.gameObject.SetActive(true);
                  Debug.Log(splitRoomData[1]);
               }
               break;
            }
            case "JoinRoom":
            {
               EventCallbackGeneral receiveEventGeneral = JsonUtility.FromJson<EventCallbackGeneral>(callbackData);
               var splitRoomData = receiveEventGeneral.data.Split('#');
               var status = splitRoomData[0];
               if(status == "success")
               {
                  createRoomPanel.gameObject.SetActive(false);
                  chatPanel.gameObject.SetActive(true);
                  roomName.text = splitRoomData[1];

                  Debug.Log(splitRoomData[1]);
               }
               else if (status == "fail")
               {
                  errjoinpopup.gameObject.SetActive(true);
                  Debug.Log(splitRoomData[1]);
               }
              break;  
            }
            case "LeaveRoom":
            {
               EventCallbackGeneral receiveEventGeneral = JsonUtility.FromJson<EventCallbackGeneral>(callbackData);
               var splitRoomData = receiveEventGeneral.data.Split('#');
               var status = splitRoomData[0];
               if(status == "success")
               {
                  Debug.Log(splitRoomData[1]);
               }
               else if(status == "fail")
               {
                  Debug.Log(splitRoomData[1]);
               }
               break;
            }
            case "Message":
            {
               EventCallbackGeneral receiveEventGeneral = JsonUtility.FromJson<EventCallbackGeneral>(callbackData);
               var splitData = receiveEventGeneral.data.Split('#');
               //splitData[0] = Room name
               //splitData[1] = userID
               //splitData[2] = name
               //splitData[3] = message
               if(splitData[1] == myID)
               {
                  sendText.text += "<color=red>" + splitData[2] +  "</color> : " + splitData[3] + "\n";
                  sendText.text += "\n";
                  receiveText.text += "\n";

               }
               else 
               {
                  receiveText.text += "<color=blue>" + splitData[2] +  "</color> : " + splitData[3] + "\n";
                  sendText.text += "\n";
                  receiveText.text += "\n";
               }
               break;
            }
         }
      }

      // Create Room
      public void CreateRoom()
      {
         var message = new EventCallbackGeneral()
         {
            eventName = "CreateRoom",
            data = RoomNameInputfield.text
         };

         var toJson = JsonUtility.ToJson(message);
         webSocket.Send(toJson);
      }

      // Join Room
      public void JoinRoom()
      {
         var message = new EventCallbackGeneral()
         {
            eventName = "JoinRoom",
            data = RoomNameInputfield.text
         };

         var toJson = JsonUtility.ToJson(message);
         webSocket.Send(toJson);

      }

      public void LeaveRoom()
      {
         var message = new EventCallbackGeneral()
         {
            eventName = "LeaveRoom",
            data = "leave"
         };

         var toJson = JsonUtility.ToJson(message);
         webSocket.Send(toJson);
      }

      public void Register()
      {
         Debug.Log("Registering..");
         loginPanel.gameObject.SetActive(false);
         registerPanel.gameObject.SetActive(true);
      }

      public void RegisterToServer()
      {
         if(regUserID.text != "" && regName.text != "" && regPassword.text != "" && regRepassword.text != "")
         {
            if( regPassword.text == regRepassword.text)
            {
               var message = new EventCallbackGeneral()
                {
                  eventName = "Register",
                  data = regUserID.text + "#" + regPassword.text + "#" + regName.text
                };

               string toJson = JsonUtility.ToJson(message);
               webSocket.Send(toJson);
            }
            else if ( regPassword.text != regRepassword.text)
            {
               pwNotMatchPopup.gameObject.SetActive(true);
            }
         }
         else 
         {
            errorInputPopup.gameObject.SetActive(true);
         }
         
      }

      public void Login()
      {
         if(userID.text != "" && password.text != "")
         {
            var message = new EventCallbackGeneral()
            {
               eventName = "Login",
               data = userID.text + "#" + password.text
            };

            var toJson = JsonUtility.ToJson(message);
            webSocket.Send(toJson);
         }
           
      }

      private void OnMessage(object sender, MessageEventArgs e)
      {
         messageQueue.Add(e.Data);
      }

      private void OnDestroy()
      {
         if (webSocket != null)
         {
            webSocket.Close();
         }
      }


      public void Disconnect()


      {
         if (webSocket != null)
         {
            webSocket.Close();
         }

         chatPanel.gameObject.SetActive(false);
         connectPanel.gameObject.SetActive(true);

         //sendText.text = string.Empty;
         //receiveText.text = string.Empty;

      }

      public void OnOpenEmotion()
      {
         isEmotionOpen = !isEmotionOpen;
         emotionPanel.gameObject.SetActive(isEmotionOpen);
      }

      public void OnExitChat()
      {
         Application.Quit();
      }

   }
}
