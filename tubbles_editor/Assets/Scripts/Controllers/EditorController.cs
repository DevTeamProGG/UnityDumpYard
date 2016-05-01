using System;
using System.Collections;

using UnityEngine;

public class EditorController : MonoBehaviour
{
	public SpriteAtlasController spriteAtlasController;
	public InputController inputController;
	public MapController mapController;
	public UIController mUIController;

	[HideInInspector]
	public bool dialogIsActive = false;

	void Start() 
	{
		//  OK start of the editor!! =)
		spriteAtlasController = new SpriteAtlasController(this);
		mapController = new MapController(this);
		inputController = new InputController(this, Camera.main);
		mUIController = (UIController)gameObject.AddComponent<UIController>();
		mUIController.setEditorController(this);
	}

	void Update() 
	{
		inputController.Update();
	}
	
	// PUBLIC FUNCTIONS
	// These can be used from wherever in the codebase
	public void quitGame()
	{
		#if UNITY_EDITOR || UNITY_EDITOR_64
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
