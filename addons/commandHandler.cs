using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Bolt;
using System.Linq;
using Jankworks;

public class commandHandler : GlobalEventListener
{

    public string[] admins = { };
    public jsonDataClass jsnData;
    private const string serverPrefix = "<color=#CCCCCC>[<color=red>Server<color=#CCCCCC>]: ";
    public bool admin = false;
    public bool adminAttemptFailed = false;
    void Start()
    {
        StartCoroutine(getData());
    }

    public override void OnEvent(ChatEvent chatEvnt)
    {
        if (BoltNetwork.isServer)
        {
            string text = chatEvnt.Text;
            List<BoltEntity> infoEntities = ((BoltEntity[])UnityEngine.Object.FindObjectsOfType<BoltEntity>()).Where(x => x.StateIs(typeof(IPlayerInfoState))).ToList();
            List<BoltEntity> entities = ((BoltEntity[])UnityEngine.Object.FindObjectsOfType<BoltEntity>()).Where(x => x.StateIs(typeof(IPlayerState))).ToList();
            IPlayerInfoState senderstate;
            if (chatEvnt.FromSelf)
            {
                senderstate = infoEntities.Find(x => x.hasControl).GetState<IPlayerInfoState>(); // this is the entity of the sender, aka the server
            }
            else
            {
                senderstate = infoEntities.Find(x => x.controller == chatEvnt.RaisedBy).GetState<IPlayerInfoState>(); // this is the entity of the sender

            }

            if (text.StartsWith("/"))
            {
                //Resets vars to default
                adminAttemptFailed = false;
                admin = false;

                //Splits the message into segments
                string[] args = text.Split(' ');

                //Checks if user is admin
                if (jsnData.admins.Contains(senderstate.SteamID) || admins.Contains(senderstate.SteamID))
                {
                    admin = true;
                }

                /*
                 * █████╗ ██████╗ ███╗   ███╗██╗███╗   ██╗     ██████╗ ██████╗ ███╗   ███╗███╗   ███╗ █████╗ ███╗   ██╗██████╗ ███████╗
                 *██╔══██╗██╔══██╗████╗ ████║██║████╗  ██║    ██╔════╝██╔═══██╗████╗ ████║████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝
                 *███████║██║  ██║██╔████╔██║██║██╔██╗ ██║    ██║     ██║   ██║██╔████╔██║██╔████╔██║███████║██╔██╗ ██║██║  ██║███████╗
                 *██╔══██║██║  ██║██║╚██╔╝██║██║██║╚██╗██║    ██║     ██║   ██║██║╚██╔╝██║██║╚██╔╝██║██╔══██║██║╚██╗██║██║  ██║╚════██║
                 *██║  ██║██████╔╝██║ ╚═╝ ██║██║██║ ╚████║    ╚██████╗╚██████╔╝██║ ╚═╝ ██║██║ ╚═╝ ██║██║  ██║██║ ╚████║██████╔╝███████║
                 *╚═╝  ╚═╝╚═════╝ ╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝     ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝ ╚══════╝                                                                                                
                 *┌───────────────────────────────────────────────────────────────────────────────────┐ 
                 *│ =-------------------------------------NOTICE------------------------------------= │
                 *│ Admin is set per map, not per server. So if you set yourself to admin on your     │
                 *│ own map and join someone else's server hosting the map, you would be admin. Not   │
                 *│ the server host.                                                                  │
                 *│                                                                                   │
                 *│ =-------------------------------HOW TO ADD ADMINS-------------------------------= │
                 *│ At the top of the script add your Steam64 to the Admins array. You can find your  │
                 *│ Steam64 by coping your profile link and putting it in the website "steamid.io".   │
                 *│ The Steam64s should be seperated by commas. Here is an example:                   │
                 *│ public string[] admins = {76561198352867973, 76561198199059546};                  │
                 *│                                                                                   │
                 *│ =------------------------------------COMMANDS-----------------------------------= │
                 *│ "/maxmoney {name}" Sets the player's money to 8,000.                              │
                 *│ "/health {name} {active} {temp} {max}" Enter NULL for any value to not use it. │
                 *│ "/unlimitedammo {name} {true/false}" Giving the player unlimited ammo.            │
                 *│ "/god {name}" or "/ungod {name}" Makes the player invincible.                     │
                 *│ "/freeze {name}" or "/unfreeze {name}" Freezes the player.                        │
                 *│ "/gravity {name} {value}" Sets the players gravity.                               │
                 *│ "/speed {name} {value}" Sets the players speed.                                   │
                 *│ "/size {name} {value}" Sets the players size.                                     │
                 *│                                                                                   │
                 *│ =----------------------------MAKE A COMMAND ADMIN ONLY--------------------------= │
                 *│ Inside the if statement add "&& admin" to check if player is an admin.            │
                 *│ At the end of the if statement add the following line.                            │
                 *│ "else if (args[0] == "/maxmoney" && !admin) { adminAttemptFailed = true; }"       │
                 *│ This will let the server know that an user attempted to use a admin command,      │
                 *│ and then notify said player they can not use the command.                         │
                 *└───────────────────────────────────────────────────────────────────────────────────┘
                 */

                if (args[0] == "/maxmoney" && admin)
                    {
                        for (int i = 0; i < infoEntities.Count; i++)
                        {
                            IPlayerInfoState state = infoEntities[i].GetState<IPlayerInfoState>();
                            if (args[1] != "all")
                            {
                                if (state.PenName.ToLower().Contains(args[1].ToLower()))
                                {
                                    SendPrivateMessage(serverPrefix + "Giving max money to " + state.PenName, Color.white, chatEvnt.RaisedBy);
                                    state.Bread = 10000;
                                }
                            }
                            else
                            {
                                SendPrivateMessage(serverPrefix + "Giving max money to " + state.PenName, Color.white, chatEvnt.RaisedBy);
                                state.Bread = 10000;
                            }
                        }
                } else if (args[0] == "/maxmoney" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/health" && admin)
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {

                                if (args[2] != null && args[2] != "null")
                                {
                                    state.ActiveHealth = int.Parse(args[2]);
                                    SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s active: " + args[2], Color.white, chatEvnt.RaisedBy);
                                }
                                if (args[3] != null && args[3] != "null")
                                {
                                    state.BriefHealth = int.Parse(args[3]);
                                    SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s temp: " + args[3], Color.white, chatEvnt.RaisedBy);
                                }
                                if (args[4] != null && args[4] != "null")
                                {
                                    state.MaxHealth = int.Parse(args[4]);
                                    SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s max: " + args[4], Color.white, chatEvnt.RaisedBy);
                                }
                            }
                        }
                        else
                        {
                            if (args[2] != null && args[2] != "null")
                            {
                                state.ActiveHealth = int.Parse(args[2]);
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s active: " + args[2], Color.white, chatEvnt.RaisedBy);
                            }
                            if (args[3] != null && args[3] != "null")
                            {
                                state.BriefHealth = int.Parse(args[3]);
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s temp: " + args[3], Color.white, chatEvnt.RaisedBy);
                            }
                            if (args[4] != null && args[4] != "null")
                            {
                                state.MaxHealth = int.Parse(args[4]);
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s max: " + args[4], Color.white, chatEvnt.RaisedBy);
                            }
                        }
                    }
                }else if (args[0] == "/health" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/god" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Giving god to " + state.PenName, Color.white, chatEvnt.RaisedBy);
                                state.IsInvincible = true;
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Giving god to " + state.PenName, Color.white, chatEvnt.RaisedBy);
                            state.IsInvincible = true;
                        }
                    }
                }else if (args[0] == "/god" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/ungod" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Removing god for " + state.PenName, Color.white, chatEvnt.RaisedBy);
                                state.IsInvincible = false;
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Removing god for " + state.PenName, Color.white, chatEvnt.RaisedBy);
                            state.IsInvincible = false;
                            
                        }
                    }
                }else if (args[0] == "/ungod" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/freeze" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Freezing " + state.PenName, Color.white, chatEvnt.RaisedBy);
                                state.isFrozen = true;
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Freezing " + state.PenName, Color.white, chatEvnt.RaisedBy);
                            state.isFrozen = true;

                        }
                    }
                } else if (args[0] == "/freeze" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/unfreeze" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Unfreezing " + state.PenName, Color.white, chatEvnt.RaisedBy);
                                state.isFrozen = false;
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Unfreezing " + state.PenName, Color.white, chatEvnt.RaisedBy);
                            state.isFrozen = false;
                        }
                    }
                } else if (args[0] == "/unfreeze" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/unlimitedammo" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s unlimited ammo to " + args[2], Color.white, chatEvnt.RaisedBy);
                                state.unlimitedAmmo = bool.Parse(args[2]);
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s unlimited ammo to " + args[2], Color.white, chatEvnt.RaisedBy);
                            state.unlimitedAmmo = bool.Parse(args[2]);

                        }
                    }
                }else if (args[0] == "/unlimitedammo" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/gravity" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s gravity to " + args[2], Color.white, chatEvnt.RaisedBy);
                                state.playerGravity = float.Parse(args[2]);
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s gravity to " + args[2], Color.white, chatEvnt.RaisedBy);
                            state.playerGravity = float.Parse(args[2]);

                        }
                    }
                } else if (args[0] == "/gravity" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/speed" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s speed to " + args[2], Color.white, chatEvnt.RaisedBy);
                                state.playerSpeedModifier = float.Parse(args[2]);
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s speed to " + args[2], Color.white, chatEvnt.RaisedBy);
                            state.playerSpeedModifier = float.Parse(args[2]);
                        }
                    }
                }else if (args[0] == "/speed" && !admin) { adminAttemptFailed = true; }

                if (args[0] == "/size" && admin)
                {
                    for (int i = 0; i < infoEntities.Count; i++)
                    {
                        IPlayerState state = entities[i].GetState<IPlayerState>();
                        if (args[1] != "all")
                        {
                            if (state.PenName.ToLower().Contains(args[1].ToLower()))
                            {
                                SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s scale to " + args[2], Color.white, chatEvnt.RaisedBy);
                                state.playerScale = float.Parse(args[2]);
                            }
                        }
                        else
                        {
                            SendPrivateMessage(serverPrefix + "Setting " + state.PenName + "'s scale to " + args[2], Color.white, chatEvnt.RaisedBy);
                            state.playerScale = float.Parse(args[2]);
                        }
                    }
                }else if (args[0] == "/size" && !admin) { adminAttemptFailed = true; }

                //If admin command was attempted and the user was not admin.
                if (adminAttemptFailed==true)
                {
                    SendPrivateMessage(serverPrefix + "Only admins can run this command.", Color.white, chatEvnt.RaisedBy);
                }


                /*
                 *██╗   ██╗███████╗███████╗██████╗      ██████╗ ██████╗ ███╗   ███╗███╗   ███╗ █████╗ ███╗   ██╗██████╗ ███████╗
                 *██║   ██║██╔════╝██╔════╝██╔══██╗    ██╔════╝██╔═══██╗████╗ ████║████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝
                 *██║   ██║███████╗█████╗  ██████╔╝    ██║     ██║   ██║██╔████╔██║██╔████╔██║███████║██╔██╗ ██║██║  ██║███████╗
                 *██║   ██║╚════██║██╔══╝  ██╔══██╗    ██║     ██║   ██║██║╚██╔╝██║██║╚██╔╝██║██╔══██║██║╚██╗██║██║  ██║╚════██║
                 *╚██████╔╝███████║███████╗██║  ██║    ╚██████╗╚██████╔╝██║ ╚═╝ ██║██║ ╚═╝ ██║██║  ██║██║ ╚████║██████╔╝███████║
                 * ╚═════╝ ╚══════╝╚══════╝╚═╝  ╚═╝     ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝ ╚══════╝
                 *┌───────────────────────────────────────────────────────────────────────────────────┐
                 *│ =----------------------------MAKE A COMMAND ADMIN ONLY--------------------------= │
                 *│ "/updatejson" Gets makes a web request to download and update the JSON.           │
                 *└───────────────────────────────────────────────────────────────────────────────────┘                                                                                                             
                 */

                if (args[0] == "/updatejson")
                {
                    StartCoroutine(getData());
                    SendPrivateMessage(serverPrefix + "Updating JSON", Color.white, chatEvnt.RaisedBy);
                    foreach (string adminID in jsnData.admins)
                    {
                        SendPrivateMessage(serverPrefix + "JSON: " + adminID, Color.white, chatEvnt.RaisedBy);
                    }
                }
            }
        }
    }



    public static void SendPrivateMessage(string text, Color32 color, BoltConnection connection)
    {
        MessageEvent evnt;

        if (connection == null)
        {
            evnt = MessageEvent.Create(GlobalTargets.OnlySelf, ReliabilityModes.ReliableOrdered);
        }
        else
        {
            evnt = MessageEvent.Create(connection, ReliabilityModes.ReliableOrdered);
        }

        evnt.Text = text;
        evnt.Color = color;
        evnt.Send();
    }



    //JSON SERIALIZATION AND DOWNLOADS AND SHITS ***DONT TOUCH THIS SHIT***
    public IEnumerator getData()
    {
        Debug.Log("Processing Data");
        WWW _www = new WWW("https://raw.githubusercontent.com/Aidenkrz/ScramMapAddons/master/admins");
        yield return _www;
        if (_www.error == null)
        {
            processJsonData(_www.text);
        }
        else
        {
            Debug.Log("_www Error: " + _www.error);
        }
    }
    private void processJsonData(string _url)
    {
        jsnData = JsonUtility.FromJson<jsonDataClass>(_url);
        foreach(string s in jsnData.admins){
            Debug.Log(s);
        }
    }

    public class jsonDataClass
    {
        public string[] admins;
    }
}