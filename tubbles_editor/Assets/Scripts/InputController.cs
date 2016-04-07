using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InputController 
{
	private EditorController mEditor;

	private Camera mMainCam;
	private GameObject mCursor;

	private Vector3 mPrevPoint;

	public InputController(EditorController editor, Camera mainCamera)
	{
		mEditor = editor;

		mMainCam = mainCamera;

		mCursor = new GameObject();
		var sr = mCursor.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/cursor");

		resetCamera();
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


		// MAKE THE USER ABLE TO SEE THE CURSOR, AND LET IT FOLLOW THE MOUSE
		mCursor.transform.position = new Vector3(Mathf.Round(currPoint.x), Mathf.Round(currPoint.y), -1);


		// MAKE THE USER ABLE TO EXIT THE GAME WITH ESC-BUTTON
		if(Input.GetKey(KeyCode.Escape)) mEditor.quitGame();


		// MAKE THE USER ABLE TO LEFT-CLICK TO TOGGLE SPRITES
		if(Input.GetMouseButton(0))
		{
			Cell c = mEditor.mapController.getCellAtWorldCoord(currPoint);
			if(c != null)
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					if(!mEditor.spriteAtlasController.spriteBelongsToAtlas(c.getSprite(), "empty"))
						c.setSprite(mEditor.spriteAtlasController.getRandomizedSprite("empty"));
				}
				else
				{
					if(!mEditor.spriteAtlasController.spriteBelongsToAtlas(c.getSprite(), "grass"))
						c.setSprite(mEditor.spriteAtlasController.getRandomizedSprite("grass"));
				}
			}
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
			Mathf.Clamp(mMainCam.transform.position.x, -mMainCam.orthographicSize/2, mEditor.mapController.getCurrentMapSize().x + mMainCam.orthographicSize/2),
			Mathf.Clamp(mMainCam.transform.position.y, -mMainCam.orthographicSize/2, mEditor.mapController.getCurrentMapSize().y + mMainCam.orthographicSize/2),
			mMainCam.transform.position.z);
		mMainCam.transform.position = clampedCamPos;

		mPrevPoint = mMainCam.ScreenToWorldPoint(Input.mousePosition);
	}

	public void resetCamera()
	{
		mMainCam.transform.position = new Vector3((mEditor.mapController.getCurrentMapSize().x-1.0f)/2.0f, (mEditor.mapController.getCurrentMapSize().y-1.0f)/2.0f, -10);
		mMainCam.orthographicSize = 5;
	}
}
