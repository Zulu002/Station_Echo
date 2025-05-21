using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public string[] dialogueLines;
    public float textSpeed = 0.05f;

    public GameObject interactIcon;
    public TextMeshPro textMeshPro;

    public AudioClip textSound; 
    private AudioSource audioSource;

    private bool isPlayerNear = false;
    private bool isDialogueActive = false;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        interactIcon.SetActive(false);
        textMeshPro.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else if (isTyping)
            {
                ShowFullText();
            }
            else
            {
                NextDialogueLine();
            }
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        textMeshPro.gameObject.SetActive(true);
        interactIcon.SetActive(false);
        currentLineIndex = 0;
        PlayTextSound(); 
        StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textMeshPro.text = "";

        foreach (char letter in line)
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void ShowFullText()
    {
        StopAllCoroutines();
        textMeshPro.text = dialogueLines[currentLineIndex];
        isTyping = false;
    }

    private void NextDialogueLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            PlayTextSound(); 
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        textMeshPro.gameObject.SetActive(false);
        interactIcon.SetActive(true);
    }

    private void PlayTextSound()
    {
        if (textSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(textSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactIcon.SetActive(false);
            if (isDialogueActive) EndDialogue();
        }
    }
}
