using System;
using PixelCrew.Utils.Editor;
using UnityEditor;

namespace PixelCrew.Components.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    
    public class ShowDialogComponentEditor: UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        private SerializedProperty _onCompleteProperty;
        
        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");//serializedObject - сущ только в редакторе
            _onCompleteProperty = serializedObject.FindProperty("_onComplete");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);//теперь будем отрисовывать мод проперти
            //снаружи переменная тоже поменяется благодаря out
            
            if (_modeProperty.GetEnum(out ShowDialogComponent.Mode mode)) //если мы сможем забрать нашу переменную mode
            {
                switch (mode)
                {
                    case ShowDialogComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound")); 
                        break;
                    case ShowDialogComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }
            }
            EditorGUILayout.PropertyField(_onCompleteProperty);//отрисуем, чтобы отображался в инспекторе 
            serializedObject.ApplyModifiedProperties(); //теперь мы можем изменять в инспекторе Mode
        }
    }
}