/*
*
* EASYFadeSample.cs
*
* Version 1.0.0
*
* Developed by Vortex Game Studios LTDA ME. (http://www.vortexstudios.com)
* Authors:		Alexandre Ribeiro de Sa (@alexribeirodesa)
*
*/

using UnityEngine;
using System.Collections;

public class EASYFadeSample : MonoBehaviour {
	private EasyFader _easyFader;

	void Start() {
		_easyFader = MonoBehaviour.FindObjectOfType<EasyFader>();
		// Set the EASY Fader events
		_easyFader.OnFadeInComplete( this.OnFadeInComplete );
		_easyFader.OnFadeOutComplete( this.OnFadeOutComplete );
	}

	public void OnClickFadeIn() {
		_easyFader.DoFadeIn();
	}

	public void OnClickFadeOut() {
		_easyFader.DoFadeOut();
	}

	public void OnFadeInComplete() {
		Debug.Log( "EASFader Event => Fade-In Completed!" );
	}

	public void OnFadeOutComplete() {
		Debug.Log( "EASFader Event => Fade-Out Completed!" );
	}
}
