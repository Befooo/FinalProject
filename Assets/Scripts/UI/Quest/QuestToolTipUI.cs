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
        [SerializeField] TextMeshProUGUI rewardText;
        public void SetUp(QuestStatus quest)
        {
            title.text = quest.GetQuest().GetTitle();
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }

            foreach (Quest.Objective objective in quest.GetQuest().GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (quest.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }

            rewardText.text = GetRewardText(quest.GetQuest());
        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";

            foreach (Quest.Reward reward in quest.GetRewards())
            {
                if (rewardText != "")
                {
                    rewardText += ", ";
                }
                if (reward.number > 1)
                {
                    rewardText += reward.number.ToString("");
                }

                rewardText += reward.item.GetDisplayName();
            }
            if (rewardText == "")
            {
                rewardText = "No reward";
            }
            rewardText += ".";
            return rewardText;
        }
    }
}