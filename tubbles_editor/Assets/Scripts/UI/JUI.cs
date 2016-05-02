using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;

public static class JUI
{
	//----------------------------------------------
	// SETUP FULLSCREEN PANEL
	//----------------------------------------------
	public static void setupFullscreenPanel(
		GameObject panel,
		GameObject parent,
		string Name
	)
	{
		panel.gameObject.name = Name;
		panel.gameObject.transform.parent = parent.transform;

		var panel_rt = panel.AddComponent<RectTransform>();
		panel_rt.anchorMax = new Vector2(1.0f, 1.0f);
		panel_rt.anchorMin = new Vector2(0.0f, 0.0f);
		panel_rt.offsetMax = new Vector2(0.0f, 0.0f);
		panel_rt.offsetMin = new Vector2(0.0f, 0.0f);

		panel.AddComponent<CanvasRenderer>();

		var panel_im = panel.AddComponent<Image>();
		panel_im.sprite = JUIResources.backgroundSprite();
		panel_im.type = Image.Type.Sliced;
		panel_im.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}

	//----------------------------------------------
	// ADD NEW PANEL
	//----------------------------------------------
	public static GameObject addUIPanel(
		GameObject parent,
		string Name
	)
	{
		GameObject panel_go = new GameObject();

		panel_go.gameObject.name = Name;
		panel_go.gameObject.transform.parent = parent.transform;

		var panel_rt = panel_go.AddComponent<RectTransform>();
		panel_rt.anchorMax = new Vector2(0.5f, 0.5f);
		panel_rt.anchorMin = new Vector2(0.5f, 0.5f);
		panel_rt.offsetMax = new Vector2(0.0f, 0.0f);
		panel_rt.offsetMin = new Vector2(0.0f, 0.0f);
		panel_rt.sizeDelta = new Vector2(800.0f, 600.0f); // WIDTH HEIGHT

		panel_go.AddComponent<CanvasRenderer>();

		var panel_im = panel_go.AddComponent<Image>();
		panel_im.sprite = JUIResources.backgroundPanelSprite();
		panel_im.type = Image.Type.Sliced;
		panel_im.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		return panel_go;
	}

	//----------------------------------------------
	// ADD NEW BUTTON
	//----------------------------------------------
	public static GameObject addNewButton(
		GameObject parent, 
		string Name,
		string Text,
		UnityAction callback
	)
	{
		// SETUP THE BUTTON OBJECT
		GameObject button_go = new GameObject();
		button_go.transform.parent = parent.transform;
		button_go.name = Name;

		// SETUP THE BUTTON TRANSFORM
		var button_tr = button_go.AddComponent<RectTransform>();
		button_tr.offsetMax = new Vector2(160.0f, 30.0f);
		button_tr.offsetMin = new Vector2(0.0f, 0.0f);
		button_tr.anchoredPosition = new Vector2(0.0f, 0.0f);

		// SETUP THE BUTTON CANVAS RENDERER
		button_go.AddComponent<CanvasRenderer>();

		// SETUP THE BUTTON IMAGE
		var button_im = button_go.AddComponent<Image>();
		button_im.sprite = JUIResources.buttonTargetGraphic();
		button_im.type = Image.Type.Sliced;

		// SETUP THE BUTTON SCRIPT
		var button_bu = button_go.AddComponent<Button>();
		button_bu.transition = Selectable.Transition.SpriteSwap;
		SpriteState ss = new SpriteState();
		ss.highlightedSprite = JUIResources.buttonHighlightedSprite();
		ss.pressedSprite = JUIResources.buttonPressedSprite();
		ss.disabledSprite = JUIResources.buttonDisabledSprite();
		button_bu.spriteState = ss;
		Navigation nav = new Navigation();
		nav.mode = Navigation.Mode.None;
		button_bu.navigation = nav;
		button_bu.onClick.AddListener(callback);

		// SETUP THE TEXT OBJECT
		GameObject text_go = new GameObject();
		text_go.transform.parent = button_go.transform;
		text_go.name = "Text";

		// SETUP THE TEXT TRANSFORM
		var text_rt = text_go.AddComponent<RectTransform>();
		text_rt.anchorMax = new Vector2(1.0f, 1.0f);
		text_rt.anchorMin = new Vector2(0.0f, 0.0f);
		text_rt.offsetMax = new Vector2(0.0f, 0.0f);
		text_rt.offsetMin = new Vector2(0.0f, 0.0f);

		// SETUP THE TEXT CANVAS RENDERER
		text_go.AddComponent<CanvasRenderer>();

		// SETUP THE TEXT SCRIPT
		var text_te = text_go.AddComponent<Text>();
		text_te.text = Text;
		text_te.font = Resources.Load<Font>("Fonts/SGK100");
		text_te.fontSize = 24;
		text_te.alignment = TextAnchor.MiddleCenter;
		text_te.color = Color.black;

		return button_go;
	}

	private static class JUIResources
	{
		public static Sprite backgroundSprite()
		{
			return Resources.Load<Sprite>("Sprites/UI/background");
		}

		public static Sprite backgroundPanelSprite()
		{
			return Resources.Load<Sprite>("Sprites/UI/panel_background");
		}

		public static Sprite buttonTargetGraphic()
		{
			return Resources.Load<Sprite>("Sprites/UI/button/menuitem_base");
		}

		public static Sprite buttonHighlightedSprite()
		{
			return Resources.Load<Sprite>("Sprites/UI/button/menuitem_highlight");
		}

		public static Sprite buttonPressedSprite()
		{
			return Resources.Load<Sprite>("Sprites/UI/button/menuitem_pressed");
		}

		public static Sprite buttonDisabledSprite()
		{
			return Resources.Load<Sprite>("Sprites/UI/button/menuitem_disabled");
		}
	}
}

