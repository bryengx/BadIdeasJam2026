using System;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class DialogOnTrigger : MonoBehaviour
{
    [Serializable]
    public class Text
    {
        public float msgDuration = 3;

        [TextArea]
        public string text;
        [HideInInspector] public bool read;
    }
    [Serializable]
    public class SpeakerInfo
    {
        public string name;
        [Tooltip("Will this dialog only be for the player? (If so name or char image is not required")]
        public bool isPlayerOnly;
        public Sprite charImage;
        public Text[] text;
    }

    public static Action<SpeakerInfo[], bool> OnTriggerDialog;
    [SerializeField] SpeakerInfo[] dialogs;
    [SerializeField] bool isHighPriority = false;

    private bool debounce = false;

    private void Awake()
    {
        if(dialogs.Length > 0)
        {
            for (int i = 0; i < dialogs.Length; i++)
            {
                SpeakerInfo speaker = dialogs[i];
                if (speaker.isPlayerOnly == false && (speaker.charImage == null || speaker.name == string.Empty))
                {
                    Debug.LogWarning("Image required for dialog index " + i);
                }
            }
        }
        else
        {
            Debug.LogWarning("Dialog is empty!");
        }
    }
    private void OnValidate()
    {
        if (dialogs.Length > 2)
        {
            Array.Resize(ref dialogs, 2);
            Debug.LogWarning("2 arrays only!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool player = collision.gameObject.CompareTag("Player");
        if (player && debounce == false)
        {
            debounce = true;
            OnTriggerDialog?.Invoke(dialogs, isHighPriority);
        }
    }
}
