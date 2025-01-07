using RPG.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;
    public void SetUp(Quest quest)
    {
        title.text = quest.GetTitle();
        progress.text = $"0/{quest.GetObjectiveCount()}";
    }
}