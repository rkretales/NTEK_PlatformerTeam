using System.IO;
using Lowscope.Saving.Data;
using UnityEditor;
using UnityEngine;
using FileMode = System.IO.FileMode;

public class SimpleJsonFileEditor : EditorWindow
{
	string filePath = "";
	private string fileContents = "";
	private string searchForExtension = "";
	private Vector2 scroll;
	private bool legacySave;

	[MenuItem ("Window/Saving/Tools/Simple Json File Editor")]
	public static void  ShowWindow () {
		GetWindow(typeof(SimpleJsonFileEditor), false, "Simple Json File Editor");
	}

	private void OnBecameVisible()
	{
		if (string.IsNullOrEmpty(searchForExtension))
		{
			searchForExtension = SaveSettings.Get().fileExtensionName.Substring(1);
		}

		string p = EditorPrefs.GetString("SimpleSaveFileEditor_LastFile", "");
		if (string.IsNullOrEmpty(p) || !File.Exists(p)) 
			return;
		
		fileContents = File.ReadAllText(p);
		filePath = p;
	}

	void OnGUI ()
	{
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Select File"))
		{
			string dir = Path.Combine(Application.persistentDataPath, SaveSettings.Get().fileFolderName);
			string path = EditorUtility.OpenFilePanel("Open save file", dir , searchForExtension);
			if (path.Length != 0)
			{
				GUI.FocusControl(null);
				fileContents = File.ReadAllText(path);
				filePath = path;

				EditorPrefs.SetString("SimpleSaveFileEditor_LastFile", filePath);
			}
		}

		GUI.enabled = !string.IsNullOrEmpty(filePath);
		if (GUILayout.Button("Reload File", GUILayout.MaxWidth(100)))
		{
			GUI.FocusControl(null);
			fileContents = File.ReadAllText(filePath);
		}
		GUI.enabled = true;
		
		GUILayout.EndHorizontal();
		
		GUI.enabled = false;
		EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent("File Path")).x + 20;
		filePath = EditorGUILayout.TextField ("File Path", filePath);
		GUI.enabled = true;
		
		
		GUI.enabled = !string.IsNullOrEmpty(filePath);
		scroll = EditorGUILayout.BeginScrollView(scroll);
		fileContents = EditorGUILayout.TextArea(fileContents, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();
		
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Write Changes"))
		{
			GUI.FocusControl(null);
			// Automatically remove anything before json starts.
			fileContents = fileContents.Substring(fileContents.IndexOf('{'));
			
			if (legacySave)
			{
				using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
				{
					writer.Write(fileContents);
				}
			}
			else
			{
				File.WriteAllText(filePath, fileContents);
			}
			
			fileContents = File.ReadAllText(filePath);
			
			Debug.Log("[SimpleJsonFileEditor] Saved File!");
		}

		EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent("Legacy Save")).x;
		legacySave = EditorGUILayout.Toggle("Legacy Save", legacySave, GUILayout.MaxWidth(90));

		GUILayout.EndHorizontal();
		
		GUI.enabled = true;
	}
}
