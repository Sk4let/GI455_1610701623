using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;

namespace BoardCast
{
   public class web2 : MonoBehaviour
   {
      //Assign websocket
      private WebSocket websocket;

      //Arrays Chat
      public List<string> myMessages;
      public List<string> otherMessages;
      public List<string> messages = new List<string>();

      //Assign
      public Text ipText;
      public Text portText;
      public Text errorText;
      public Text listText;
      public Text nameInput;
      public Text nickname;
      public TextMesh agentnickname;
      public GameObject boardChat;
      public GameObject loginPanel;
      public Transform chatContent;
      public VerticalLayoutGroup messagePrefab;
      public InputField chatInputField;
      public Button sendButton;
      public GameObject emotionPanel;

      bool isEmotionOpen = false;

      //Client id
      public string id;
      public string list;
      public string username;

      void Start()
      {
         websocket = new WebSocket($"ws://127.0.0.1:9523/");
         websocket.OnMessage += RecieveMessage;
         var i = Random.Range(1,999);
         agentnickname.text = "Guest"+i.ToString();
      }

      public void FixedUpdate()
      {
         //If my message has updated.
         if (myMessages.Count > 0)
         {
            var message = myMessages[0];
            myMessages.Clear();

            StartCoroutine(WaitAndRender(0.3f, message, TextAnchor.MiddleRight));
            chatInputField.text = string.Empty;
         }

         //If other message has updated.
         if (otherMessages.Count > 0)
         {
            var message = otherMessages[0];
            otherMessages.Clear();

            StartCoroutine(WaitAndRender(0.4f, message, TextAnchor.MiddleLeft));
            chatInputField.text = string.Empty;
         }

      }

      //Send data to Server
      public void SendAction()
      {
         //send {} to Server at {id}
         string type = "message"; //send {type}
         string message = chatInputField.text; //send {msg_data}

         websocket.Send($"{id}#{type}#{message}");
      }
      // Leave Chatroom
      public void Disconnect()
      {
         if (websocket != null)
         {
            websocket.Close();
            chatInputField.text = string.Empty;
         }

         boardChat.gameObject.SetActive(false);
         loginPanel.gameObject.SetActive(true);

      }
      // Recieve data from Server
      public void RecieveMessage(object sender, MessageEventArgs e)
      {
         // Split data TO id#type#data ex. 1#register#Login as guest
         string[] ext = e.Data.Split('#');
         // Array after split data
         //after split data in array are [{1},{register},{Login as guest}]
         //ext[0] = id , ext[1] = type , ext[2] = data

         //Check type 
         switch (ext[1]) 
         {
            case "register": 
               if (string.IsNullOrEmpty(id)) // If client has no id then assign ext[0] to id 
                  id = ext[0]; 
               // Then update UI in WaitAndRender().
               break;
            case "message": //Type ext[1]
               // Check ID
               // If ext[0] = id then use mymessage if not then use othermessage.
               if (ext[0] == id)
               {
                  //Add data to myMessages[]
                  myMessages.Add(ext[2]);
               }
               else
               {
                  //Add data to otherMessages[]
                  otherMessages.Add(ext[2]);
               }
               break;
            default:
               break;
         }
         Debug.Log(string.Join("\n", ext)); //Show Id and type on Client Console.
         Debug.Log(list + " In server");

      }
      //Delay before Instantiate Textbox To Chatroom
      public IEnumerator WaitAndRender(float delay, string message, TextAnchor align)
      {
         yield return new WaitForSeconds(delay);

         // If string not empty
         if (!string.IsNullOrEmpty(message))
         {
            //Instantiate Prefab at Position chatContent.
            var prefab = Instantiate(messagePrefab, chatContent);
            // Assign child align for prefab
            prefab.childAlignment = align;
            // Assign Text to Prefab
            Text child = prefab.GetComponentInChildren<Text>();
            // Text = message
            child.text = message;
         }
      }
      
      public void NicknameAssign()
      {
         agentnickname.text = nameInput.text;
         var mynickname = agentnickname.text;
      }
      // Connect to server
      public void OnConnectToServer()
      {
         // Check Ip and Port
         if (ipText.text == "127.0.0.1" && portText.text == "9523")
         {
            errorText.gameObject.SetActive(false);
            websocket.Connect();
            boardChat.gameObject.SetActive(true);
            loginPanel.gameObject.SetActive(false);
            
            //if id is empty send data to server
            if (string.IsNullOrEmpty(id))
            {
               var type = "register";
               var data = "Login as guest";
               websocket.Send($"{id}#{type}#{data}"); // #register#Login as guest
            }
         }
         else
         {
            //If wrong ip or port then active error message!
            errorText.gameObject.SetActive(true);
         }
      }

      public void Quit()
      {
         Application.Quit();
      }

      public void OnOpenEmotion()
      {
         isEmotionOpen = !isEmotionOpen;  
         emotionPanel.gameObject.SetActive(isEmotionOpen);
      }
   }
}