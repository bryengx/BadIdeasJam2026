using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogOnTrigger;

public class Dialog : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvGroup;
    [SerializeField] private bool onTriggerEnterDialog = true;
    [SerializeField] private Image charImag;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float msgDuration = 3f;

    public struct QueuedDialog
    {
        public DialogOnTrigger dialogTrigger;
        public SpeakerInfo[] speakerInfo;
    }
    private Queue<QueuedDialog> dialogsQueued = new Queue<QueuedDialog>();
    private bool showingText = false;

    private void Awake()
    {
        canvGroup.alpha = 0;


    }
    private void OnEnable()
    {
        OnTriggerDialog += StartDialog;
    }
    private void OnDisable()
    {
        OnTriggerDialog -= StartDialog;
    }

    public void StartDialog(DialogOnTrigger sender, SpeakerInfo[] dialInfo, bool highPriority)
    {
        if (dialInfo.Length == 0)
        {
            Debug.LogError("Populate dialog!");
            return;
        }
        if (showingText)
        {
            if (highPriority)
            {
                StopAllCoroutines();
                foreach(QueuedDialog dialog in dialogsQueued)
                {
                    dialog.dialogTrigger?.OnDialogEnd(false);
                }
                dialogsQueued = new Queue<QueuedDialog>();
                Debug.Log("Clearing queue!");
            }
            else
            {
                Debug.Log("Queueng..");
                QueuedDialog que = new QueuedDialog();
                que.dialogTrigger = sender;
                que.speakerInfo = dialInfo;

                dialogsQueued.Enqueue(que);
                return;
            }
        }
        
        showingText = true;
        canvGroup.alpha = 1f;
        StartCoroutine(ShowDialog(sender, dialInfo));
    }

    private IEnumerator ShowDialog(DialogOnTrigger sender, SpeakerInfo[] dialogs)
    {
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
                SpeakerInfo speaker = dialogs[index];

                string txt = string.Empty;
                foreach (DialogOnTrigger.Text t in speaker.text)
                {
                    if (t.read)
                    {
                        continue;
                    }
                    t.read = true;
                    txt = t.text;
                    break;
                }
                if (txt != string.Empty)
                {
                    textMesh.text = txt;
                    nameText.text = speaker.name;
                    charImag.sprite = speaker.charImage;
                    yield return new WaitForSeconds(msgDuration);
                }
                if(dialogs.Length >1) index = index == 0 ? 1 : 0;

                yield return null;
            }
        }
        sender?.OnDialogEnd(true);

        showingText = false;
        if ( dialogsQueued.Count > 0 )
        {
            Debug.Log("Dequeueing!");
            QueuedDialog que = dialogsQueued.Dequeue();
            StartDialog(que.dialogTrigger, que.speakerInfo, false);
        }
        else
        {
            canvGroup.alpha = 0f;
        }
        
    }
    private bool DialogFinished(SpeakerInfo[] dialogs)
    {
        //InventoryUI.instance.Show();
        int finished = 0;

        foreach (SpeakerInfo speaker in dialogs)
        {
            int count = speaker.text.Length;
            int i = 0;

            if(count == 0)
            {
                Debug.LogError("There are no messages found in: " + speaker.name);
                return true;
            }

            foreach (DialogOnTrigger.Text t in speaker.text)
            {
                if (t.read) i++;
            }
            if (i == count) finished++;
        }

        return finished == dialogs.Length;
    }
}
