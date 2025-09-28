using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NPCDialog", menuName = "Scriptable Objects/NPCDialog")]
public class NPCDialog : ScriptableObject
{
    // First decision parameters
    [Tooltip("List of decision texts for Decision 1")]
    [TextArea(3, 10)]
    public List<string> decision_1_text;  // Multiple texts for Decision 1

    [Tooltip("List of bravery effects for Decision 1")]
    public List<int> decision_1_bravery_effect;  // Multiple bravery effects for Decision 1

    [Tooltip("List of loyalty effects for Decision 1")]
    public List<int> decision_1_loyalty_effect;  // Multiple loyalty effects for Decision 1

    [Tooltip("List of helpfulness effects for Decision 1")]
    public List<int> decision_1_helpfulness_effect;  // Multiple helpfulness effects for Decision 1

    // Second decision parameters
    [Tooltip("List of decision texts for Decision 2")]
    [TextArea(3, 10)]
    public List<string> decision_2_text;  // Multiple texts for Decision 2

    [Tooltip("List of bravery effects for Decision 2")]
    public List<int> decision_2_bravery_effect;  // Multiple bravery effects for Decision 2

    [Tooltip("List of loyalty effects for Decision 2")]
    public List<int> decision_2_loyalty_effect;  // Multiple loyalty effects for Decision 2

    [Tooltip("List of helpfulness effects for Decision 2")]
    public List<int> decision_2_helpfulness_effect;  // Multiple helpfulness effects for Decision 2

    // Message box texts
    [Tooltip("List of message box texts")]
    public List<string> message_box_texts;  // Multiple message box texts for both decisions

    private void OnEnable()
    {
        #if UNITY_EDITOR
                hideFlags = HideFlags.DontSaveInEditor;
        #endif
    }
}
