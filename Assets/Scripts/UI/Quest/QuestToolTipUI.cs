using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestToolTipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        public void SetUp(QuestStatus quest)
        {
            title.text = quest.GetQuest().GetTitle();
            objectiveContainer.DetachChildren();

            foreach (string objective in quest.GetQuest().GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (quest.IsObjectiveComplete(objective))
                {
                    prefab = objectivePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective;
            }
        }
    }
}