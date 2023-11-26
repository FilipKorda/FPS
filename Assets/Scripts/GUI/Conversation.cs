#if UNITY_EDITOR
using UnityEditor;
#endif
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

    public bool isAskingQuestion = false;

    [Header("========================================================================")]
    [Space(8)]
    [HideInInspector] public string OptionOne;
    [HideInInspector] public Conversation answerOne;
    [Header("========================================================================")]
    [Space(8)]
    [HideInInspector] public string OptionTwo;  
    [HideInInspector] public Conversation answerTwo;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Conversation))]
public class ConversationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var conversationData = serializedObject.FindProperty("conversation");

        EditorGUILayout.PropertyField(conversationData, true);

        if (conversationData.isExpanded)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < conversationData.arraySize; i++)
            {
                var data = conversationData.GetArrayElementAtIndex(i);
                var isAskingQuestion = data.FindPropertyRelative("isAskingQuestion");

                EditorGUILayout.PropertyField(isAskingQuestion);

                if (isAskingQuestion.boolValue)
                {
                    EditorGUILayout.PropertyField(data.FindPropertyRelative("OptionOne"));
                    EditorGUILayout.PropertyField(data.FindPropertyRelative("answerOne"));
                    EditorGUILayout.PropertyField(data.FindPropertyRelative("OptionTwo"));                   
                    EditorGUILayout.PropertyField(data.FindPropertyRelative("answerTwo"));
                }
            }
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
