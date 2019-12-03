using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( EasyFader ) )]
public class EASYFaderEditor : Editor {
	private SerializedObject _so;
	private EasyFader Target {
		get { return (EasyFader)target; }
	}

	void OnEnable() {
		_so = new SerializedObject( target );
	}

	public override void OnInspectorGUI() {
		Target.type = ( EasyFader.EASYFaderType)( EditorGUILayout.EnumPopup( "Type", Target.type ) );
		Target.color = EditorGUILayout.ColorField( "Color", Target.color );

		if ( Target.type == EasyFader.EASYFaderType.Texture ) {
			Target.texture = EditorGUILayout.ObjectField( "Pattern", Target.texture, typeof( Texture ), false ) as Texture2D;
			Target.textureFill = EditorGUILayout.Toggle( "Fill", Target.textureFill );
			Target.textureScale = EditorGUILayout.FloatField( "Scale", Target.textureScale );
		}

		Target.interval = EditorGUILayout.FloatField( "Interval", Target.interval );
		Target.value = EditorGUILayout.Slider( "Value", Target.value, 0.0f, 1.0f );

		GUILayout.Label( "EASY Fader v.1.0.0", EditorStyles.miniBoldLabel );
		EditorUtility.SetDirty( Target );
	}
}