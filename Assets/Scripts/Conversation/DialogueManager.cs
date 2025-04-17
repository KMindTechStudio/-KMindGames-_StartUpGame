using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterNameText; // Text hiển thị tên nhân vật
    public TextMeshProUGUI dialogueText;      // Text hiển thị nội dung hội thoại
    public Image characterIcon;               // Image hiển thị icon nhân vật
    public Button continueButton;             // Nút để chuyển dòng hội thoại
    public GameObject dialoguePanel;          // Panel chứa UI hội thoại

    private Dialogue currentDialogue;         // Hội thoại hiện tại
    private int currentLine = 0;              // Dòng hội thoại hiện tại

    void Start()
    {
        continueButton.onClick.AddListener(DisplayNextLine); // Gán sự kiện cho nút Continue
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentLine = 0;
        dialoguePanel.SetActive(true); // Hiển thị UI hội thoại
        DisplayLine();
    }

    void DisplayLine()
    {
        if (currentLine < currentDialogue.dialogueLines.Count)
        {
            DialogueCharacter line = currentDialogue.dialogueLines[currentLine];
            characterNameText.text = line.character.name;
            dialogueText.text = line.line;
            characterIcon.sprite = line.character.icon;
        }
        else
        {
            EndDialogue();
        }
    }

    public void DisplayNextLine()
    {
        currentLine++;
        DisplayLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Ẩn UI hội thoại khi kết thúc
        Debug.Log("Kết thúc hội thoại");
    }
}