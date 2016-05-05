using System;
using System.IO;
using System.Collections;

using UnityEngine;

public class EditorController : MonoBehaviour
{
	private static EditorController mEditor;
	public static EditorController Instance {
		get {
			return mEditor;
		}
	}

	public SpriteAtlasController spriteAtlasController;
	public InputController inputController;
	public MapController mapController;

	[HideInInspector]
	public UIController mUIController;

	[HideInInspector]
	public PrefabSelector mPrefabSelector;

	private bool doLateStart = true;

	void Start() 
	{
		mEditor = this;

		//  OK start of the editor!! =)
		spriteAtlasController = new SpriteAtlasController();
		mapController = new MapController();
		inputController = new InputController(Camera.main);
		mUIController = gameObject.AddComponent<UIController>();
		mPrefabSelector = FindObjectOfType<PrefabSelector>();
	}

	void LateStart()
	{
		mUIController.LateStart();
	}

	void Update() 
	{
		if(doLateStart)
		{
			doLateStart = false;
			LateStart();
		}

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
