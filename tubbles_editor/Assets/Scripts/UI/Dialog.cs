using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
	public enum Type
	{
		EFileDoesNotExist,
		EFileIsNotCorrectFormat,
		QOverwriteFile,
		WOldMapVersion,
	}

	public Button YesButton, NoButton, OkButton;
	public Text mText;

	private Type mType;
	private Action mWhenYes;
	private Action mWhenNo;
	private Action mWhenOk;

	public void InitializeOk(GameObject parent, Type reason, string message, Action cbWhenOk)
	{
		this.gameObject.transform.SetParent(parent.transform, false);
		mType = reason;
		mText.text = message;
		mWhenOk = cbWhenOk;

		YesButton.gameObject.SetActive(false);
		NoButton.gameObject.SetActive(false);
	}

	public void InitializeYesNo(GameObject parent, Type reason, string message, Action cbWhenYes, Action cbWhenNo)
	{
		this.gameObject.transform.SetParent(parent.transform, false);
		mType = reason;
		mText.text = message;
		mWhenYes = cbWhenYes;
		mWhenNo = cbWhenNo;

		OkButton.gameObject.SetActive(false);
	}

	public void Finish()
	{
		Destroy(this.gameObject);
	}

	public void cbYesButtonClicked()
	{
		if(mWhenYes != null)
			mWhenYes();
		Finish();
	}

	public void cbNoButtonClicked()
	{
		if(mWhenNo != null)
			mWhenNo();
		Finish();
	}

	public void cbOkButtonClicked()
	{
		if(mWhenOk != null)
			mWhenOk();
		Finish();
	}

}
