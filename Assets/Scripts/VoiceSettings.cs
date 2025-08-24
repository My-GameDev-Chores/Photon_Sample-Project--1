using Photon.Voice.PUN;
using UnityEngine;

public class VoiceSettings : MonoBehaviour
{
    private void Start()
    {
        ConfigureVoice();
    }

    private void ConfigureVoice()
    {
        if (PunVoiceClient.Instance != null)
        {
            PunVoiceClient.Instance.AutoRemovePlayer = true;
            Debug.Log("Voice: AutoRemove enabled");
        }
    }
}