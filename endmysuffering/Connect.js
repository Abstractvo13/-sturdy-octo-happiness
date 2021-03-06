﻿//Default Ip and Port
var connectToIP : String = "127.0.0.1";
var connectPort : int = 25001;


function OnGUI ()
    //This is the GUI to start server or to connect to a server as a client

{

    if (Network.peerType == NetworkPeerType.Disconnected){
	
        GUILayout.Label("Connection status: Disconnected");
		
        connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
        connectPort = parseInt(GUILayout.TextField(connectPort.ToString()));
		
        GUILayout.BeginVertical();
        if (GUILayout.Button ("Connect as client"))
        {

            //Network.useNat = false;
            Network.Connect(connectToIP, connectPort);
        }
		
        if (GUILayout.Button ("Start Server"))
        {
	
            //Network.useNat = false;
            Network.InitializeServer(4, connectPort, false);
        }
        GUILayout.EndVertical();
		
		
    }else{			

        if (Network.peerType == NetworkPeerType.Connecting){
		
            GUILayout.Label("Connection status: Connecting");
			
        } else if (Network.peerType == NetworkPeerType.Client){
			
            GUILayout.Label("Connection status: Client!");
            GUILayout.Label("Ping to server: "+Network.GetAveragePing(  Network.connections[0] ) );		
            //Tells player if they are Client or Server
        } else if (Network.peerType == NetworkPeerType.Server){
			
            GUILayout.Label("Connection status: Server!");
            GUILayout.Label("Connections: "+Network.connections.length);
            if(Network.connections.length>=1){
                GUILayout.Label("Ping to first player: "+Network.GetAveragePing(  Network.connections[0] ) );
            }			
        }

        if (GUILayout.Button ("Disconnect"))
        {
            //Disconnect Button
            Network.Disconnect(200);
        }
    }
}
