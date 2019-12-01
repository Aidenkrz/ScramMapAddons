using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Jankworks;

public class VoiceChat : MonoBehaviour {

	bool isRecording = false;
    GameObject voiceHandler;

    void Start()
    {
        GameObject gui = GameObject.Find("GUI");
        gui.transform.Find("Client").Find("Game Canvas").Find("Pause Menu").Find("Options Menu").Find("Game").Find("VoIP Volume").gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isRecording = true;
            JankClient.StartVoiceRecording();
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            isRecording = false;

            JankClient.StopVoiceRecording();
        }

        if (isRecording)
        {
            if (voiceHandler == null)
            {
                voiceHandler = GameObject.Find("BoltBehaviours");
            }
            else
            {
                voiceHandler.BroadcastMessage("CheckForVoice", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
