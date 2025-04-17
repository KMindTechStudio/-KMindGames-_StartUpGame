using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Dữ liệu hội thoại
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("Không tìm thấy DialogueManager trong Scene!");
        }
        StartDialogue();
    }


    public void StartDialogue()
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }
}