using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScenePlacer))]
public class ScenePlacerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScenePlacer script = (ScenePlacer)target;
        if(GUILayout.Button("Reposition"))
        {
            script.ArrangeScenes();
        }
    }

}
