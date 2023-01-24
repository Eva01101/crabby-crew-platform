using UnityEditor;
using UnityEditor.UI;

namespace PixelCrew.UI.Widgets.Editor
{
    [CustomEditor(typeof(CustomButton), true)] //указываем для какого комп будем использ данный эдитор
    [CanEditMultipleObjects] //может выбрать неск кнопок и редактировать их вместе
    
    public class CustomButtonEditor: ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            serializedObject.ApplyModifiedProperties(); //сохранить изменения, если кнопки переназначили
            
            base.OnInspectorGUI();
        }
    }
}