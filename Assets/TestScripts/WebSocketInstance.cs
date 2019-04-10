using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using QFramework;
using Newtonsoft.Json;
using TDE;

public class WebSocketInstance : MonoSingleton<WebSocketInstance>
{
    private static WebSocket upDateWebSocket;

    private string lastSpaceId = string.Empty;
    private string lastCatalog = string.Empty;

    public void InitWebSocket(string ipUrl)
    {
        // ipUrl 192.168.1.201:8080/vibe-web
        upDateWebSocket = new WebSocket("ws://" + ipUrl + "/websocket");
        Debug.Log("ws://" + ipUrl + "/websocket");
        upDateWebSocket.OnMessage += UpDateWebSocket_OnMessage;
        upDateWebSocket.OnOpen += (sender, e) => { Debug.Log("WebSocket IS OPEN"); };

        upDateWebSocket.OnError += (sender, e) =>
        {
            upDateWebSocket.Close();
            Debug.LogFormat("<color=red>WebSocket IS ERROR</color>");
            InitWebSocket(ipUrl);
            upDateWebSocket.Send(1 + "," + 0 + "|1");
        };

        upDateWebSocket.Connect();

        upDateWebSocket.Send(1 + "," + 0 + "|1");
    }

    private void UpDateWebSocket_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("收到推送消息" + e.Data);
        WebSocketMessage message= JsonConvert.DeserializeObject<WebSocketMessage>(e.Data);
       if (message != null) Global.UpdataBindData(message);
    }

    

    public void Subscription(string spaceId, string catalog)
    {
        if (!string.IsNullOrEmpty(lastSpaceId) && !string.IsNullOrEmpty(lastCatalog))
        {
            Unsubscribe(lastSpaceId, lastCatalog);
        }

        upDateWebSocket.Send(spaceId + "," + catalog + "|1");

        Debug.Log(spaceId + "," + catalog + "|1");

        lastSpaceId = spaceId;
        lastCatalog = catalog;
    }

    public void Unsubscribe(string spaceId, string catalog)
    {
        upDateWebSocket.Send(spaceId + "," + catalog + "|2");
    }
}
public class WebSocketMessage
{
    
    public string Id { set; get; }
    public string Data { set; get; }//valueStr
    public string State { set; get; }
}
