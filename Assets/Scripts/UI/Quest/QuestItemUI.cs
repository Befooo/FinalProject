using RPG.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;

    QuestStatus questStatus;
    public void SetUp(QuestStatus quest)
    {
        this.questStatus = quest;
        title.text = quest.GetQuest().GetTitle();
        progress.text = $"{quest.GetCompleteCount()}/{quest.GetQuest().GetObjectiveCount()}";
    }

    public QuestStatus GetQuest()
    {
        return questStatus;
    }
}