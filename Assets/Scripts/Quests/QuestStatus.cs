using System;
using System.Collections.Generic;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            completedObjectives = state.completedObjectives;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompleteCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string obj)
        {
            return completedObjectives.Contains(obj);
        }

        public void CompleteObjective(string objective)
        {
            if (!quest.HasObjective(objective)) return;
            completedObjectives.Add(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();

            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
        }

        internal bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                if (!completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }
            return true;
        }
    }
}