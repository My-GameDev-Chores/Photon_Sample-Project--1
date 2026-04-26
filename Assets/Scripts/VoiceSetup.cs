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
            Invoke(nameof(EnableVoice), 2.0f);
        }
        else
        {
            if (recorder != null) recorder.enabled = false;
        }
    }

    private void EnableVoice()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = false;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine || recorder == null) return;
        if (UIManager.Instance == null || UIManager.Instance.muteToggle == null) return;

        // Push-to-Talk: Hold V to speak (only when not muted)
        recorder.TransmitEnabled = Input.GetKey(KeyCode.V) && !UIManager.Instance.muteToggle.isOn;

        // M key toggles the UI toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            UIManager.Instance.muteToggle.isOn = !UIManager.Instance.muteToggle.isOn;
        }
    }

    private void OnDestroy()
    {
        if (recorder != null && recorder.TransmitEnabled)
        {
            recorder.TransmitEnabled = false;
        }
    }
}