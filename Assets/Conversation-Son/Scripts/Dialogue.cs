using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public Character character; // Tham chiếu đến nhân vật
    public string line;         // Dòng hội thoại
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueCharacter> dialogueLines = new List<DialogueCharacter>(); // Danh sách dòng hội thoại
}