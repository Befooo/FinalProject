using System;
using Codice.Client.Commands;
using Codice.Client.Common.TreeGrouper;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle;
        [NonSerialized]
        GUIStyle playerNodeStyle;
        [NonSerialized]
        DialogueNode draggingNode;
        [NonSerialized]
        DialogueNode creatingNode;
        [NonSerialized]
        DialogueNode deletingNode;
        [NonSerialized]
        DialogueNode linkingParentNode;
        [NonSerialized]
        Vector2 draggingOffset = Vector2.zero;
        Vector2 scrollPos = Vector2.zero;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset = Vector2.zero;

        private const float CANVAS_SIZE = 4000.0f;
        private const float BACKGROUND_SIZE = 50.0f;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }
        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectedChange;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectedChange()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;

            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                Rect canvas = GUILayoutUtility.GetRect(CANVAS_SIZE, CANVAS_SIZE);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoords = new Rect(0, 0, CANVAS_SIZE / BACKGROUND_SIZE, CANVAS_SIZE / BACKGROUND_SIZE);

                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoords);

                foreach (DialogueNode dialogue in selectedDialogue.GetAllNodes())
                {
                    DrawConnection(dialogue);
                }
                foreach (DialogueNode dialogue in selectedDialogue.GetAllNodes())
                {
                    DrawNode(dialogue);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        private void DrawConnection(DialogueNode dialogue)
        {
            Vector3 startPos = new Vector2(dialogue.GetRect().xMax, dialogue.GetRect().center.y);
            foreach (DialogueNode dialogueNode in selectedDialogue.GetAllChildren(dialogue))
            {
                Vector3 endPos = new Vector2(dialogueNode.GetRect().xMin, dialogueNode.GetRect().center.y);
                Vector3 controlPointOffset = endPos - startPos;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;

                Handles.DrawBezier(startPos, endPos, startPos + controlPointOffset, endPos - controlPointOffset, Color.white, null, 4f);
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPos);

                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPos;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move dialogue node");
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPos = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point)) foundNode = node;
            }

            return foundNode;
        }

        private void DrawNode(DialogueNode dialogue)
        {
            GUIStyle style = nodeStyle;

            if(dialogue.IsPlayerSpeaking()) style = playerNodeStyle;

            GUILayout.BeginArea(dialogue.GetRect(), style);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node: ", EditorStyles.boldLabel);

            dialogue.SetText(EditorGUILayout.TextField(dialogue.GetText()));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-"))
            {
                deletingNode = dialogue;
            }

            DrawLinkButton(dialogue);

            if (GUILayout.Button("+"))
            {
                creatingNode = dialogue;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButton(DialogueNode dialogue)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = dialogue;
                }
            }
            else if (linkingParentNode == dialogue)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetChildren().Contains(dialogue.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    linkingParentNode.RemoveChild(dialogue.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    linkingParentNode.AddChild(dialogue.name);
                    linkingParentNode = null;
                }
            }
        }
    }
}

