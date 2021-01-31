using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ChatManager : MonoBehaviour
{

    public int massagesMax = 20;
    public GameObject textObject, chatRoom;
    public InputField chatField;
    public Text textFromServer;
    public Color playerMessage, info;

    public InputField ipInput;
    public InputField portInput;
    public Text test;

    private WebSocket websocket;
    [SerializeField]
    List<Message> messageList = new List<Message>();
    // Start is called before the first frame update
    void Start()
    {
            websocket = new WebSocket("ws://127.0.0.1:25565/");

            websocket.OnMessage += OnMessage;
            websocket.Connect();
        
    }

    // Update is called once per frame
    void Update()
    {
       /* if(chatField.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessage(chatField.text);
                websocket.Send(chatField.text);
            }
        }
        else 
        {
             if (!chatField.isFocused && Input.GetKeyDown(KeyCode.Return))
             {
                 chatField.ActivateInputField();
             }
        }

        if (!chatField.isFocused)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SendMessage("Hello");
                Debug.Log("Space");
            }
        }*/
        
    }
         public void OnDestroy() 
        {
            if (websocket != null)
            {
                websocket.Close();
            }
        }
     public void OnMessage(object sender, MessageEventArgs messageEventArgs)
   {
      Debug.Log("Message from server :" + messageEventArgs.Data);
      textFromServer.text = messageEventArgs.Data;
      test.text = messageEventArgs.Data;
   }

   public void ConnectButton()
        {
            websocket.Connect();
            /*if(ipInput.text == "127.0.0.1" && portInput.text == "25565")
            {
                websocket.Connect();
            }*/
        }

    public void SendMessage(string text)
    {
        if (messageList.Count >= massagesMax)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
         Message newMessage = new Message();
         newMessage.text = text;

         GameObject newText = Instantiate(textObject, chatRoom.transform);

         newMessage.textObject = newText.GetComponent<Text>();

         newMessage.textObject.text = newMessage.text;
         messageList.Add(newMessage);
    }

    [System.Serializable]
    public class Message
    {
        public string text;
        public Text textObject;
    }

    public void OnClickSend()
    {
        SendMessage(chatField.text);
        websocket.Send(chatField.text);
    }
}
