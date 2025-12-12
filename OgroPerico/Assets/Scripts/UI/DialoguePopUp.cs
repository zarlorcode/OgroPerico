using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialoguePopup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterName;
    private CanvasGroup canvasGroup;

    // transparent button inside panel
    public Button clickArea;

    [Header("Settings")]
    private float autoAdvanceTime = 4f; // 0 = deactivated
    private float fadeDuration = 1f;

    private int index = 0;
    private bool canClick = false;

    public DialogueLine[] dialogueLines;

    void Awake()
    {
        // Comienza invisible por si acaso
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    /*public void Initialize(DialogueLine[] newSentences)
    {
        dialogueLines = newSentences;
        dialogueText.text = sentences[0];
        StartCoroutine(FadeIn());
    }*/

    public void ShowDialogue(DialogueLine[] newSentences)
    {
        Debug.Log("ShowDialogue");
        dialogueLines = newSentences;
        index = 0;

        ShowLine();
        gameObject.SetActive(true);

        clickArea.onClick.RemoveAllListeners();
        clickArea.onClick.AddListener(NextSentence);

        StartCoroutine(FadeIn());
    }

    void ShowLine()
    {
        Debug.Log("ShowLine");
        DialogueLine line = dialogueLines[index];
        dialogueText.text = line.sentence;
        characterName.text = line.characterName;
    }

    void NextSentence()
    {
        Debug.Log("NextSentence");
        if (!canClick) return;

        index++;
        

        if (index < dialogueLines.Length)
        {
            ShowLine();

            if (autoAdvanceTime > 0)
                StartCoroutine(AutoAdvance());
        }
        else
        {
            StartCoroutine(FadeOutAndDisable());
        }
    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
        canClick = false;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;

        canClick = true;
        Debug.Log("Start autoadvance: " + autoAdvanceTime);
        if (autoAdvanceTime > 0)
        {
            Debug.Log("call autoadvance");
            StartCoroutine(AutoAdvance());
        }
    }

        IEnumerator FadeOutAndDisable()
    {
        Debug.Log("FadeOutAndSetActive");
        canClick = false;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        gameObject.SetActive(false);
    }

    IEnumerator AutoAdvance()
    {
        Debug.Log("AutoAdvance, time " + autoAdvanceTime);
        yield return new WaitForSeconds(autoAdvanceTime);
        NextSentence();
    }
}
