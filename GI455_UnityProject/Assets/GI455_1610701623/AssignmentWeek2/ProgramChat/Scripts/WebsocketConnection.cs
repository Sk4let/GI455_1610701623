using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace BoardCast
{
   public class WebsocketConnection : MonoBehaviour
   {
      private WebSocket websocket;
      public List<string> myMessages;
      public List<string> otherMessages;

      public Text ipText;
      public Text portText;
      public Text errorText;
      public GameObject boardChat;
      public GameObject loginPanel;

      public string id;
      //public InputField nameField;
      //public InputField messageField;
      public InputField chatInputField;
      public Button sendButton;

      public Transform chatContent;
      public VerticalLayoutGroup messagePrefab;

      public List<string> messages = new List<string>();


      void Start()
      {
         websocket = new WebSocket($"ws://127.0.0.1:9523/");
         websocket.OnMessage += RecieveMessage;

         // ส่งข้อมูล

         //sendButton.onClick.AddListener(SendAction);
      }

      private void FixedUpdate()
      {
         if (myMessages.Count > 0)
         {
            var message = myMessages[0];
            myMessages.Clear();

            StartCoroutine(WaitAndRender(0.3f, message, TextAnchor.MiddleRight));
         }

         if (otherMessages.Count > 0)
         {
            var message = otherMessages[0];
            otherMessages.Clear();

            StartCoroutine(WaitAndRender(0.4f, message, TextAnchor.MiddleLeft));
         }
      }

      public void SendAction()
      {
         string type = "message";
         string message = chatInputField.text;

         websocket.Send($"{id}#{type}#{message}");
      }

      public void OnDestroy()
      {
         if (websocket != null)
         {
            websocket.Close();
         }
      }

      public void RecieveMessage(object sender, MessageEventArgs e)
      {
         // จัดการข้อมูล id#type#data ex. 5568#register#Register Completed
         string[] ext = e.Data.Split('#');

         switch (ext[1])
         {
            case "register":
               if (string.IsNullOrEmpty(id)) // ถ้า client ยังไม่มี id ให้ assign id ของ client
                  id = ext[0];
               // ต่อไปก็อัพเดท UI
               break;
            case "message":
               //1 Who send
               if (ext[0] == id)
               {
                  myMessages.Add(ext[2]);
               }
               else
               {
                  otherMessages.Add(ext[2]);
               }
               break;
            default:
               break;
         }
         Debug.Log(string.Join("\n", ext));

      }
      public IEnumerator WaitAndRender(float delay, string message, TextAnchor align)
      {
         yield return new WaitForSeconds(delay);

         if (!string.IsNullOrEmpty(message))
         {
            var prefab = Instantiate(messagePrefab, chatContent);
            prefab.childAlignment = align;
            Text child = prefab.GetComponentInChildren<Text>();
            child.text = message;
         }
      }

      public void OnConnectToServer()
      {
         if (ipText.text == "127.0.0.1" && portText.text == "9523")
         {
            errorText.gameObject.SetActive(false);
            websocket.Connect();
            boardChat.gameObject.SetActive(true);
            loginPanel.gameObject.SetActive(false);
            
            if (string.IsNullOrEmpty(id))
            {
               var type = "register";
               var data = "I want my Id";
               websocket.Send($"{id}#{type}#{data}"); // #register#I want my Id
            }
         }
         else
         {
            errorText.gameObject.SetActive(true);
         }
      }
   }
}