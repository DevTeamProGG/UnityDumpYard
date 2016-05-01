using System;

using UnityEngine;
using UnityEngine.UI;

public static class JUI
{
	public static void addNewButton(
		GameObject parent, 
		string Name,
		string Text
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
		var button_cr = button_go.AddComponent<CanvasRenderer>();

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

		// SETUP THE TEXT OBJECT
		GameObject text_go = new GameObject();
		text_go.transform.parent = button_go.transform;
		text_go.name = "Text";

		// SETUP THE TEXT TRANSFORM
		var text_tr = text_go.AddComponent<RectTransform>();
		text_tr.anchorMax = new Vector2(1.0f, 1.0f);
		text_tr.anchorMin = new Vector2(0.0f, 0.0f);
		text_tr.offsetMax = new Vector2(0.0f, 0.0f);
		text_tr.offsetMin = new Vector2(0.0f, 0.0f);

		// SETUP THE TEXT CANVAS RENDERER
		var text_cr = text_go.AddComponent<CanvasRenderer>();

		// SETUP THE TEXT SCRIPT
		var text_te = text_go.AddComponent<Text>();
		text_te.text = Text;
		text_te.font = Resources.Load<Font>("Fonts/SGK100");
		text_te.fontSize = 24;
		text_te.alignment = TextAnchor.MiddleCenter;
		text_te.color = Color.black;

	}

	private static class JUIResources
	{
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

