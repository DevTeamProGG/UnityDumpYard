using System.Collections;

using UnityEngine;

public class InputController 
{
	private EditorController mEditor;

	private Camera mMainCam;

	private Vector3 mPrevPoint;
	private IntVector2 mMapSize;

	public InputController(EditorController editor, Camera mainCamera, IntVector2 mapSize)
	{
		mEditor = editor;

		mMainCam = mainCamera;
		mMapSize = mapSize;

		mMainCam.transform.position = new Vector3((mMapSize.x-1.0f)/2.0f, (mMapSize.y-1.0f)/2.0f, -10);
	}

	public void Update () 
	{
		// ENTERING ROUTINE FOR CAMERA
		Vector3 currPoint = mMainCam.ScreenToWorldPoint(Input.mousePosition);
		if(Input.GetMouseButton(1))
		{
			Vector3 diff = mPrevPoint - currPoint;
			mMainCam.transform.transform.Translate(diff);
		}


		// MAKE THE USER ABLE TO EXIT THE GAME WITH ESC-BUTTON
		if(Input.GetKey(KeyCode.Escape)) mEditor.quitGame();


		// MAKE THE USER ABLE TO LEFT-CLICK TOGGLE SPRITES
		if(Input.GetMouseButtonDown(0))
		{
			Cell c = mEditor.mapController.getCellAtWorldCoord(currPoint);
			if(mEditor.spriteAtlasController.getSpriteAtlas("grass").spriteBelongsToAtlas(c.getSprite()))
				c.setSprite(mEditor.spriteAtlasController.getRandomizedSprite("empty"));
			else
				c.setSprite(mEditor.spriteAtlasController.getRandomizedSprite("grass"));
		}

		// MAKE THE USER ABLE TO SCROLL-ZOOM
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			mMainCam.orthographicSize = Mathf.Min(mMainCam.orthographicSize + 1, 20);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			mMainCam.orthographicSize = Mathf.Max(mMainCam.orthographicSize - 1, 2);
		}


		// EXITING ROUTINE FOR CAMERA
		Vector3 clampedCamPos = new Vector3(
			Mathf.Clamp(mMainCam.transform.position.x, -mMainCam.orthographicSize/2, mMapSize.x + mMainCam.orthographicSize/2),
			Mathf.Clamp(mMainCam.transform.position.y, -mMainCam.orthographicSize/2, mMapSize.y + mMainCam.orthographicSize/2),
			mMainCam.transform.position.z);
		mMainCam.transform.position = clampedCamPos;
		mPrevPoint = mMainCam.ScreenToWorldPoint(Input.mousePosition);
	}
}
