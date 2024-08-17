using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestManager questManager = (QuestManager)target;

        if (GUILayout.Button("Reset All Completion Quest"))
        {
            questManager.ResetAllCompletionQuest();
        }
    }
}