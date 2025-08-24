using Photon.Voice.Unity;
using Photon.Pun;
using UnityEngine;

public class VoiceSetup : MonoBehaviourPun
{
    private Recorder recorder;

    private void Start()
    {
        recorder = GetComponent<Recorder>();

        if (photonView.IsMine)
        {
            // Local player - enable voice after delay
            Invoke(nameof(EnableVoice), 2.0f);
        }
        else
        {
            // Remote player - disable recorder (only need speaker)
            if (recorder != null)
                recorder.enabled = false;
        }
    }

    private void EnableVoice()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = true;
            Debug.Log("Voice enabled for local player");
        }
    }

    private void OnDestroy()
    {
        // Cleanup
        if (recorder != null && recorder.TransmitEnabled)
        {
            recorder.TransmitEnabled = false;
        }
    }
}