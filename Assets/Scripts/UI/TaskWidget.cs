using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskWidget : MonoBehaviour
{
    public TextMeshProUGUI TaskText;
    public Toggle CheckBox;
    
    public void InitializeWidget(MinigameTaskData TaskData)
    {
        TaskText.text = TaskData.TaskTitle;
    }

    public void MarkComplete(bool Complete)
    {
        CheckBox.isOn = Complete;
    }
}
