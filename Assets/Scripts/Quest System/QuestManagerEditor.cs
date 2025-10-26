using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
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
#endif