using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false;
        string text;
        List<string> children = new List<string>();
        Rect rect = new Rect(0, 0, 200, 100);

        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return children;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = position;

            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText == text) return;

            Undo.RecordObject(this, "Update Dialogue Text");
            this.text = newText;

            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childName)
        {
            Undo.RecordObject(this, "Add Dialog Link");
            children.Add(childName);

            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childName)
        {
            Undo.RecordObject(this, "Remove Dialog Link");
            children.Remove(childName);

            EditorUtility.SetDirty(this);
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        internal void SetPlayerSpeaking(bool isPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change dialogue speaker");
            this.isPlayerSpeaking = isPlayerSpeaking;

            EditorUtility.SetDirty(this);
        }
#endif
    }
}

