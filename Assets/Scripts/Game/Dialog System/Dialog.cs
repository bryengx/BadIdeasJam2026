using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static DialogOnTrigger;

public interface IDialogAdditionalActions
{
    void BeforeDialog();
    void AfterDialog();
}

public class Dialog : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Dialog delagate.
    /// </summary>
    /// <param name="OnDialogEndFunction">Pass in a function you would like to be called wgen dialog is finished</param>
    /// <param name="OnDialogStartFunction">Pass in a function you would like to be called is starting</param>
    /// <param name="triggerOnFailFunction">Must the on end function still be called if the dialog was interupted or failed?</param>
    /// <param name="highPriority">Is dialog of high priority or low. Higher removes any low qeued dialogs to show this one. and also higher priority dialogs can overtake each other. so..</param>
    /// <param name="dialog">An array of text to be shown for the player to read.</param>
    public delegate void OnTriggerDialogs(DialogInfo[] dialog,bool highPriority = false, Action OnDialogStartFunction = null,
        Action OnDialogEndFunction = null, bool triggerOnFailFunction = false);
    /// <summary>
    /// Dialog delagate.
    /// </summary>
    /// <param name="OnDialogEndFunction">Pass in a function you would like to be called wgen dialog is finished</param>
    /// <param name="OnDialogStartFunction">Pass in a function you would like to be called is starting</param>
    /// <param name="triggerOnFailFunction">Must the on end function still be called if the dialog was interupted or failed?</param>
    /// <param name="highPriority">Is dialog of high priority or low. Higher removes any low qeued dialogs to show this one. and also higher priority dialogs can overtake each other. so..</param>
    /// <param name="dialog">Text to be shown for the player to read.</param>
    public delegate void OnTriggerDialog(DialogInfo dialog,bool highPriority = false, Action OnDialogStartFunction = null,
        Action OnDialogEndFunction = null, bool triggerOnFailFunction = false);

    /// <summary>
    /// Call an array dialog to be shown
    /// </summary>
    public static OnTriggerDialogs CallDialogs;
    /// <summary>
    /// Call dialog to be shown
    /// </summary>
    public static OnTriggerDialog CallDialog;

    /// <summary>
    /// When dialog is stopped/interupted/cancled or finished
    /// passes the method to be called, if it can be called on end when failed, and if it actually failed
    /// </summary>
    private static event Action<Action,bool,bool> OnDialogEnd;
    #endregion

    #region Variables
    [SerializeField] private CanvasGroup canvGroup;
    [SerializeField] private Image charImag;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float typingSpeed = 0.05f;

    [Space(20)]
    [Header("Player")]
    [SerializeField] private Sprite playerImage;
    [SerializeField] private string playerName;

    private IDialogAdditionalActions additionalActions;

    private QueuedDialog currentDialog = new QueuedDialog();
    private Queue<QueuedDialog> dialogsQueued = new Queue<QueuedDialog>();
    private bool showingText = false;
    #endregion

    #region Classes and Structs
    [Serializable]
    public class DialogText
    {
        public float duration = 3f;

        [TextArea]
        public string text;

        [HideInInspector] public bool read;
    }
    [Serializable]
    public class DialogInfo
    {
        public string name;
        public bool repeatable;

        [Tooltip("If is player, theres no need to fill in images and name.")]
        public bool isPlayer;
        public Sprite charImage;
        public List<DialogText> text;
        public UnityEvent onSpeakEnded;
    }

    public struct QueuedDialog
    {
        public bool triggerOnFail;
        public Action onStartFunction;
        public Action onEndFunction;
        public DialogInfo[] speakerInfo;
    }
    #endregion


    private void Awake()
    {
        if(canvGroup == null)
        {
            Debug.LogError("Add canvas group to main!");
            enabled = false;
            return;
        }
        canvGroup.alpha = 0;
    }
    private void OnEnable()
    {
        CallDialogs += StartDialog;
        CallDialog += StartDialog;
        OnDialogEnd += OnEndDialogFunction;
    }
    private void OnDisable()
    {
        CallDialogs -= StartDialog;
        CallDialog -= StartDialog;
        OnDialogEnd -= OnEndDialogFunction;
    }
    public void StartDialog(IDialogAdditionalActions actions, DialogInfo[] dialInfo, bool highPriority)
    {
        additionalActions = actions;
        CallDialogs?.Invoke(dialInfo, highPriority);
    }
    private void Update()
    {
        if (showingText) canvGroup.alpha = 1f;
        else canvGroup.alpha = 0f;
    }

    //Call start dialog without an array
    private void StartDialog(DialogInfo dialInfo, bool highPriority, Action OnDialogStartFunc, Action OnDialogEndFunc, bool triggerOnFail)
    {
        DialogInfo[] toArray = new DialogInfo[] { dialInfo };
        StartDialog(toArray,highPriority,OnDialogStartFunc,OnDialogEndFunc,triggerOnFail);
    }
    //Call start dialog with an array
    private void StartDialog(DialogInfo[] dialInfo, bool highPriority, Action OnDialogStartFunc,Action OnDialogEndFunc, bool triggerOnFail)
    {
        if (dialInfo.Length == 0)
        {
            Debug.LogError("Populate dialog!");
            OnDialogEnd?.Invoke(OnDialogEndFunc, triggerOnFail, false);
            return;
        }
        if (showingText)
        {
            if (highPriority)
            {
                StopAllCoroutines();
                if(currentDialog.triggerOnFail)
                {
                    currentDialog.onEndFunction?.Invoke();
                    currentDialog.onEndFunction = null;
                }
                if(dialogsQueued.Count > 0)
                {
                    foreach (QueuedDialog dialog in dialogsQueued)
                    {
                        OnDialogEnd?.Invoke(dialog.onEndFunction, dialog.triggerOnFail, false);
                    }
                    dialogsQueued = new Queue<QueuedDialog>();
                    Debug.Log("Clearing queue!");
                }
                
            }
            else
            {
                Debug.Log("Queueng..");
                QueuedDialog que = new QueuedDialog();
                que.onStartFunction = OnDialogStartFunc;
                que.onEndFunction = OnDialogEndFunc;
                que.speakerInfo = dialInfo;
                que.triggerOnFail = triggerOnFail;

                dialogsQueued.Enqueue(que);
                return;
            }
        }
        OnDialogStartFunc?.Invoke();
        showingText = true;

        currentDialog. onEndFunction = OnDialogEndFunc;
        currentDialog. triggerOnFail = triggerOnFail;

        StartCoroutine(ShowDialog(dialInfo, OnDialogEndFunc, triggerOnFail));
    }

    private IEnumerator ShowDialog(DialogInfo[] dialogs,Action OnDialogEndFunc, bool triggerOnFail)
    {
        additionalActions?.BeforeDialog();

        if (dialogs[0].repeatable)
        {
            foreach(DialogInfo dialog in dialogs)
            {
                foreach(DialogText text in dialog.text)
                {
                    text.read = false;
                }
            }
        }
        //InventoryUI.instance.Hide();
        if (dialogs.Length == 0)
        {
            Debug.LogError("Populate dialog!");
        }
        else
        {
            int index = 0;
            while (DialogFinished(dialogs) == false)
            {
                DialogInfo speaker = dialogs[index];

                string txt = string.Empty;
                float waitDuration = 0f;
                foreach (DialogText t in speaker.text)
                {
                    if (t.read)
                    {
                        continue;
                    }
                    t.read = true;
                    txt = t.text;
                    waitDuration = t.duration;
                    break;
                }
                if (txt != string.Empty)
                {
                    nameText.text = speaker.isPlayer ? playerName : speaker.name;
                    charImag.sprite = speaker.isPlayer ? playerImage : speaker.charImage;

                    textMesh.text = txt;
                    textMesh.maxVisibleCharacters = 0;

                    textMesh.ForceMeshUpdate();
                    int totalCharcters = textMesh.textInfo.characterCount;

                    for (int i = 0; i <= totalCharcters; i++)
                    {
                        //ytexting sound or clicking sound, or wahtever the sound guys add. but here when words appear nscreen
                        textMesh.maxVisibleCharacters = i;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    yield return new WaitForSeconds(waitDuration);
                    speaker.onSpeakEnded?.Invoke();
                }
                if(dialogs.Length >1) index = index == 0 ? 1 : 0;

                yield return null;
            }
        }
        additionalActions?.AfterDialog();

        showingText = false;
        OnDialogEnd?.Invoke(OnDialogEndFunc, triggerOnFail, true);

        if ( dialogsQueued.Count > 0 )
        {
            Debug.Log("Dequeueing!");
            QueuedDialog que = dialogsQueued.Dequeue();
            StartDialog(que.speakerInfo, false, que.onStartFunction,que.onEndFunction, que.triggerOnFail);
        }

        additionalActions = null;
    }
    
    private bool DialogFinished(DialogInfo[] dialogs)
    {
        //InventoryUI.instance.Show();
        int finished = 0;

        foreach (DialogInfo speaker in dialogs)
        {
            int count = speaker.text.Count;
            int i = 0;

            if(count == 0)
            {
                Debug.LogError("There are no messages found in: " + speaker.name);
                return true;
            }

            foreach (DialogText text in speaker.text)
            {
                if (text.read) i++;
            }

            if (i == count) finished++;
        }

        return finished == dialogs.Length;
    }

    /// <summary>
    /// yara yara, when dialog ends
    /// </summary>
    /// <param name="method">Method to be called</param>
    /// <param name="canTriggerOnFail">can Method Be Called If Not Succesful</param>
    /// <param name="succes">was the dialog shown succesfuly or not</param>
    private void OnEndDialogFunction(Action method, bool canTriggerOnFail, bool succes)
    {
        //If dialog didnt fail or can be called even if it failed
        if (succes || canTriggerOnFail)
        {
            method?.Invoke();
        }
    }
}
