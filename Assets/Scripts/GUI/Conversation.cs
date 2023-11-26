using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation/Conversation Data", order = 0)]
public class Conversation : ScriptableObject
{
    public ConversationData[] conversation;
}

[System.Serializable]
public class ConversationData
{
    public string Name;
    public string[] Sentences;
}

