using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        Dictionary<string, DialogueNode> nodeLookUp = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
        }
#endif

        private void OnValidate()
        {
            nodeLookUp.Clear();

            foreach (DialogueNode dialogueNode in GetAllNodes())
            {
                nodeLookUp[dialogueNode.name] = dialogueNode;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookUp.ContainsKey(childID))
                {
                    yield return nodeLookUp[childID];
                }
            }
        }

        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode dialogueNode = MakeNode(parentNode);

            Undo.RegisterCreatedObjectUndo(dialogueNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialog Node");

            AddNode(dialogueNode);
        }

        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            DialogueNode dialogueNode = CreateInstance<DialogueNode>();
            dialogueNode.name = Guid.NewGuid().ToString();
            if (parentNode != null)
            {
                parentNode.AddChild(dialogueNode.name);

                dialogueNode.SetPlayerSpeaking(!parentNode.IsPlayerSpeaking());
                dialogueNode.SetPosition(parentNode.GetRect().position + newNodeOffset);
            }

            return dialogueNode;
        }

        private void AddNode(DialogueNode dialogueNode)
        {
            nodes.Add(dialogueNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Delete Dialog Node");

            nodes.Remove(nodeToDelete);
            OnValidate();

            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToClean)
        {
            foreach (DialogueNode dialogueNode in GetAllNodes())
            {
                dialogueNode.RemoveChild(nodeToClean.name);
            }
        }

        public void OnBeforeSerialize()
        {
            if (nodes.Count == 0)
            {
                DialogueNode dialogueNode = MakeNode(null);
                AddNode(dialogueNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode dialogueNode in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(dialogueNode) == "")
                    {
                        AssetDatabase.AddObjectToAsset(dialogueNode, this);
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {
            // throw new NotImplementedException();
        }
    }
}

