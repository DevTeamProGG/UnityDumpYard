using System;
using System.Collections;

using UnityEngine;

public class EditorController : MonoBehaviour
{
	public SpriteAtlasController spriteAtlasController;
	public InputController inputController;
	public MapController mapController;

	private IntVector2 mMapSize;

	void Start() 
	{
		//  OK start of the editor!! =)
		mMapSize = new IntVector2(10, 10);

		spriteAtlasController = new SpriteAtlasController(this);
		inputController = new InputController(this, Camera.main, mMapSize);
		mapController = new MapController(this, mMapSize);
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
