using Firebase.Messaging;
using UnityEngine;

public class FireBaseMessager : MonoBehaviour
{
    //notificação 

  public void Start()
    {
#if UNITY_EDITOR
        Debug.Log("Unity Editor");
#else
    Debug.Log("Any other platform");
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
#endif

    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }


}
