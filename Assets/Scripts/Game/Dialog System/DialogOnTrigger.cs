using System;
using UnityEngine;
using UnityEngine.Events;
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

    [Tooltip("Must dialog be triggered by on collide?")]
    [SerializeField] private bool ontriggerEnterDialog = true;

    //Call this anywhere if you want to make a dialog. Dont search for the dialog class with find object by type
    public static Action<DialogOnTrigger, SpeakerInfo[], bool> OnTriggerDialog;

    [SerializeField] SpeakerInfo[] dialogs;
    [SerializeField] bool isHighPriority = false;

    [SerializeField] private UnityEvent OnDialogBegin;
    [SerializeField] private UnityEvent OnDialogFinish;

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
        if (ontriggerEnterDialog == false) return;
        bool player = collision.gameObject.CompareTag("Player");
        if (player && debounce == false)
        {
            debounce = true;
            TriggerDialog();
        }
    }

    public void TriggerDialog()
    {
        OnDialogBegin?.Invoke();
        OnTriggerDialog?.Invoke(this, dialogs, isHighPriority);
    }
    public void OnDialogEnd(bool succ)
    {
        OnDialogFinish?.Invoke();
    }
}
