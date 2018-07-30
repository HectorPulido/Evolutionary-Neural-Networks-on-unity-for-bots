using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryPerceptron.MendelMachine.Editor
{
	using UnityEditor;
	using EvolutionaryPerceptron.MendelMachine;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;

    [CanEditMultipleObjects]
	[CustomEditor(typeof(Brain))]
	public class BrainEditor : Editor 
	{

		Brain b ;
		void OnEnable()
		{
			b = (Brain) target;
		}
		public override void OnInspectorGUI()
		{       
			if(b == null)
				b = (Brain) target;

			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("learningPhase"));
			if(b.learningPhase)
			{
				EditorGUILayout.HelpBox("Desactive learning phase if you want to deploy, ", MessageType.Warning);
			}
			EditorGUILayout.PropertyField(serializedObject.FindProperty("activationFunction"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("brainPath"));
			if(b.perceptron.W.Length != 0)
			{
				EditorGUILayout.HelpBox("You can save this neural network", MessageType.Info);
				if (GUILayout.Button("Save", GUILayout.MaxWidth(Screen.width)))
				{
					SaveFile();
				}
			}
			else
			{
				var dropArea = GUILayoutUtility.GetRect(Screen.width, 35, GUILayout.MaxWidth(Screen.width - 40));
				if (string.IsNullOrEmpty(b.brainPath))
				{
					GUI.Box(dropArea, "Drag an neural network");								
				}
				else
				{
					GUI.Box(dropArea, "Perceptron loaded " + Path.GetFileName(b.brainPath));
				}
				DragAndDropFile(dropArea);
				
			}
			
			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();

			if(GUI.changed)
              EditorUtility.SetDirty(b);
		}
		private void SaveFile()
		{
			string filepath = EditorUtility.SaveFilePanel("Create neural network file", "Assets", this.name, "nn");

			if (!string.IsNullOrEmpty(filepath) )
			{
				try
                {
                    FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, b.perceptron.W);
                    fs.Close();
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e.Message);
                }
			}
		}
		private void DragAndDropFile(Rect DropArea)
		{
			Event current = Event.current;

			switch (current.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:

					if (DropArea.Contains(current.mousePosition))
					{
						if(DragAndDrop.paths.Length == 0)
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						}
						else
						{
							if(DragAndDrop.paths[0].EndsWith(".nn"))
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
							}
							else
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
							}

						}
						if (current.type == EventType.DragPerform)
						{

							b.brainPath = DragAndDrop.paths[0];
							DragAndDrop.AcceptDrag();
							current.Use();							
						}
					}
					break;
			}
		}
	

	}
}
