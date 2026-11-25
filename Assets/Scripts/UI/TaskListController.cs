using UnityEngine;

public class TaskListController : MonoBehaviour
{
    public GameObject TaskPrefab;
    public Transform ContentsParent;
    
    public TaskWidget CreateTask(MinigameTaskData TaskData)
    {
        TaskWidget task = Instantiate(TaskPrefab, ContentsParent).GetComponent<TaskWidget>();
        task.InitializeWidget(TaskData);

        return task;
    }
}
