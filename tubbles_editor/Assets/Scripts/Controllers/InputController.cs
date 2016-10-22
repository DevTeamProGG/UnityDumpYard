					using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class InputController 
{
	private EditorController mEditor;

	private Camera mMainCam;
	private GameObject mCursor;

	private Vector3 mPrevPoint;
	private SpriteRenderer mMouseCursor;
	private Sprite mMouseEditingCursorSprite;

	public InputController(Camera mainCamera)
	{
		mEditor = EditorController.Instance;

		mMainCam = mainCamera;

		mCursor = new GameObject();
		mCursor.name = "Mouse Cursor";
		mMouseCursor = mCursor.AddComponent<SpriteRenderer>();
		mMouseEditingCursorSprite = Resources.Load<Sprite>("Sprites/UI/cursor");
		mMouseCursor.sprite = mMouseEditingCursorSprite;

		resetCameraZoomAndSize();
	}

	public void Update () 
	{
		Vector3 currPoint;

		if(EventSystem.current.IsPointerOverGameObject())
		{
			// DO GUI THINGS

			// HIDE THE EDITING CURSOR
			if(mMouseCursor.sprite != null)
			{
				mMouseCursor.sprite = null;
			}
		}
		else
		{
			// DO GUI FORBIDDEN THINGS

			// ENTERING ROUTINE FOR CAMERA
			currPoint = mMainCam.ScreenToWorldPoint(Input.mousePosition);
			if(Input.GetMouseButton(1))
			{
				Vector3 diff = mPrevPoint - currPoint;
				mMainCam.transform.transform.Translate(diff);
			}

			// MAKE THE USER ABLE TO SEE THE CURSOR, AND LET IT FOLLOW THE MOUSE
			if(mMouseCursor.sprite != mMouseEditingCursorSprite)
			{
				mMouseCursor.sprite = mMouseEditingCursorSprite;
			}
			mCursor.transform.position = new Vector3(Mathf.Round(currPoint.x), Mathf.Round(currPoint.y), -1);

			// MAKE THE USER ABLE TO SAVE THE MAP WITH S-BUTTON
			if(Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				mEditor.mapController.saveMap();
			}

			// MAKE THE USER ABLE TO LOAD THE MAP WITH L-BUTTON
			if(Input.GetKeyDown(KeyCode.L) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
			{
				mEditor.mUIController.newSelectFileDialog(mEditor.mapController.loadMapAs, "Select a file to load", new string[]{".map"});
			}

			// MAKE THE USER ABLE TO CLEAR THE MAP WITH C-BUTTON
			if(Input.GetKeyDown(KeyCode.C)) 
			{
				mEditor.mapController.clearMap();
			}

			// MAKE THE USER ABLE TO FILL THE MAP WITH F-BUTTON
			if(Input.GetKeyDown(KeyCode.F)) 
			{
				mEditor.mapController.clearMapGrass();
			}

			// MAKE THE USER ABLE TO LEFT-CLICK TO TOGGLE SPRITES
			if(Input.GetMouseButton(0))
			{
				mEditor.mapController.paintSpriteAtLocation(mEditor.mUIController.getCurrentTileBrushName(), currPoint);
			}

			// MAKE THE USER ABLE TO SCROLL-ZOOM
			if(Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.Minus))
			{
				mMainCam.orthographicSize = Mathf.Min(mMainCam.orthographicSize + 1, 50);
			}
			if(Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.Equals))
			{
				mMainCam.orthographicSize = Mathf.Max(mMainCam.orthographicSize - 1, 2);
			}

			// MAKE THE USER ABLE TO RESET THE CAMERA WITH THE MIDDLE MOUSE BUTTON
			if(Input.GetMouseButtonDown(2))
			{
				resetCameraZoom();
			}

			// MAKE THE USER ABLE TO EXIT THE GAME WITH ESC-BUTTON
			if(Input.GetKeyDown(KeyCode.Escape)) 
			{
				mEditor.mUIController.showMainMenu();
			}
		}
		// DO THE REST OF THINGS

		// TEST THE FADE OVERLAY OPTION
		if(Input.GetKeyDown(KeyCode.T)) 
		{
			mEditor.mUIController.cycleOverlayVisibility();
		}

		// EXITING ROUTINE FOR CAMERA
		Vector3 clampedCamPos = new Vector3(
			Mathf.Clamp(mMainCam.transform.position.x, -mMainCam.orthographicSize/2, mEditor.mapController.getCurrentMapSize().x + mMainCam.orthographicSize/2),
			Mathf.Clamp(mMainCam.transform.position.y, -mMainCam.orthographicSize/2, mEditor.mapController.getCurrentMapSize().y + mMainCam.orthographicSize/2),
			mMainCam.transform.position.z);
		mMainCam.transform.position = clampedCamPos;

		mPrevPoint = mMainCam.ScreenToWorldPoint(Input.mousePosition);
	}

	public void resetCameraZoomAndSize()
	{
		mMainCam.transform.position = new Vector3((mEditor.mapController.getCurrentMapSize().x-1.0f)/2.0f, (mEditor.mapController.getCurrentMapSize().y-1.0f)/2.0f, -10);
		mMainCam.orthographicSize = 15;
	}

	public void resetCameraZoom()
	{
		mMainCam.orthographicSize = 15;
	}
}
