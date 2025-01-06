using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRayCastable
    {
        [SerializeField] string conversantName;
        [SerializeField] private Dialogue dialogue = null;
        public ECursorType eCursorType => ECursorType.DIALOGUE;

        public bool HandleRayCast(PlayerController playerController)
        {
            if (dialogue == null) return false;

            if (Input.GetMouseButtonDown(0))
            {
                playerController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            }
            return true;
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}