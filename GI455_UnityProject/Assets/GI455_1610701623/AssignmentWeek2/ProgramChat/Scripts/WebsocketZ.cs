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
       class MessageDataJson
       {
           public string username;
           public string message;
       }

       public GameObject loginPanel;
       public GameObject chatPanel;
       public GameObject emotionPanel;

       public InputField ipInput;
       public InputField portInput;
       public Text sendText;
       public Text receiveText;
       public TextMesh agentnickname;

       public InputField inputText;
       public InputField inputUsername;
       private WebSocket webSocket;

       private string tempMessageString;

       bool isEmotionOpen = false;
       public Text nickname;

       public void Start()
       {
           var id = Random.Range(1, 999);
           nickname.text = "Guest" + id.ToString();
       }

       public void Update() 
       {
           if (tempMessageString != null && tempMessageString != "")
           {
               MessageDataJson receiveMessageData = JsonUtility.FromJson<MessageDataJson>(tempMessageString);
               if (receiveMessageData.username == nickname.text)
               {
                   sendText.text += "<color=red>" + receiveMessageData.username +  "</color> : " + receiveMessageData.message + "\n";
                   receiveText.text += "\n";
                   receiveText.text += "\n";
               }
               else 
               {
                   sendText.text += "\n";
                   receiveText.text += "\n";
                   receiveText.text += "<color=blue>" + receiveMessageData.username + "</color> : "  + receiveMessageData.message + "\n";
               }

               tempMessageString = "";
           }
       }

       public void Connect() 
       {
           if (ipInput.text == "127.0.0.1" && portInput.text == "9523")
           {
                webSocket = new WebSocket($"ws://127.0.0.1:9523/");
                webSocket.OnMessage += OnMessage;
                webSocket.Connect();

                loginPanel.gameObject.SetActive(false);
                chatPanel.gameObject.SetActive(true);

                if (string.IsNullOrEmpty(inputUsername.text))
                {
                    agentnickname.text = nickname.text;
                }
                else 
                {
                    agentnickname.text = inputUsername.text;
                }

                nickname.text = agentnickname.text;
           }
       }

       public void SendMessage()
       {
           if (inputText.text == "" || webSocket.ReadyState != WebSocketState.Open)
            return;

            MessageDataJson messageData = new MessageDataJson();
            messageData.username = nickname.text;
            if(string.IsNullOrEmpty(inputUsername.text))
            {
                messageData.username = nickname.text;
            }
            messageData.message = inputText.text;

            string toJsonStr = JsonUtility.ToJson(messageData);

            webSocket.Send(toJsonStr);
            inputText.text = "";
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
         loginPanel.gameObject.SetActive(true);

         sendText.text = string.Empty;
         receiveText.text = string.Empty;

      }

       private void OnMessage(object sender, MessageEventArgs messageEventArgs)
       {
           tempMessageString = messageEventArgs.Data;
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
