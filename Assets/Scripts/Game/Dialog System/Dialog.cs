using Codice.Client.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogOnTrigger;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image charImag;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI textMesh;

    [Space(20)]
    [Header("Player defaults")]
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private string playerName;

    private Queue<SpeakerInfo[]> dialogsQueued = new Queue<SpeakerInfo[]>();
    private bool showingText = false;

    private void Awake()
    {
        canvas.SetActive(false);
        if(playerSprite == null)
        {
            Debug.LogWarning("Player sprite is not assigned!");
        }
        if(playerName == string.Empty)
        {
            Debug.LogWarning("Player name is not set");
        }
    }
    private void OnEnable()
    {
        OnTriggerDialog += StartDialog;
    }
    private void OnDisable()
    {
        OnTriggerDialog -= StartDialog;
    }

    public void StartDialog(SpeakerInfo[] dialInfo, bool highPriority)
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
                dialogsQueued = new Queue<SpeakerInfo[]>();
                Debug.Log("Clearing queue!");
            }
            else
            {
                Debug.Log("Queueng..");
                dialogsQueued.Enqueue(dialInfo);
                return;
            }
        }
        
        showingText = true;
        canvas.SetActive(true);
        StartCoroutine(ShowDialog(dialInfo));
    }

    private IEnumerator ShowDialog(SpeakerInfo[] dialogs)
    {
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
                float dur = 0;

                foreach (DialogOnTrigger.Text t in speaker.text)
                {
                    if (t.read)
                    {
                        continue;
                    }
                    t.read = true;
                    txt = t.text;
                    dur = t.msgDuration;
                    break;
                }
                if (txt != string.Empty)
                {
                    textMesh.text = txt;
                    if (speaker.isPlayerOnly)
                    {
                        nameText.text = playerName;
                        charImag.sprite = playerSprite;
                    }
                    else
                    {
                        nameText.text = speaker.name;
                        charImag.sprite = speaker.charImage;
                    }
                        
                    yield return new WaitForSeconds(dur);
                }
                if(dialogs.Length >1) index = index == 0 ? 1 : 0;

                yield return null;
            }
        }

        showingText = false;
        if ( dialogsQueued.Count > 0 )
        {
            Debug.Log("Dequeueing!");
            StartDialog(dialogsQueued.Dequeue(), false);
        }
        else
        {
            canvas.SetActive(false);
        }
        
    }
    private bool DialogFinished(SpeakerInfo[] dialogs)
    {
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
