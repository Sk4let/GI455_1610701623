using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

namespace BoardChat
{
    public class WebsocketConnection : MonoBehaviour
    {
        // connect
        [SerializeField] InputField ipInput;
        [SerializeField] InputField portInput;
        private WebSocket websocket;

        void Start()
        {
            websocket = new WebSocket("ws://127.0.0.1:25565/");

            websocket.OnMessage += OnMessage;

        }
        
        void Update()
        {
                //websocket.Send("Test");
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
            Debug.Log("Message from server :" +messageEventArgs.Data);
        }

        public void ConnectButton()
        {
            if(ipInput.text == "127.0.0.1" && portInput.text == "25565")
            {
                websocket.Connect();
            }
        }

    }
}