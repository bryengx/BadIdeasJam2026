using System;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class DialogOnTrigger : MonoBehaviour
{
    [Serializable]
    public class Text
    {
        [TextArea]
        public string text;
        [HideInInspector] public bool read;
    }
    [Serializable]
    public class SpeakerInfo
    {
        public string name;
        public Sprite charImage;
        public Text[] text;
    }

    public static Action<SpeakerInfo[], bool> OnTriggerDialog;
    [SerializeField] SpeakerInfo[] dialogs;
    [SerializeField] bool isHighPriority = false;

    private bool debounce = false;
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
