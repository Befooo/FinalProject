using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] Quest[] tempQuests;
    [SerializeField] QuestItemUI questPrefab;
    QuestList questList;

    private void Start()
    {
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.onUpdate += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        foreach (QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            uiInstance.SetUp(status);
        }
    }
}