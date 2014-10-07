using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangarUI : MonoBehaviour {
	public GUISkin skin;
	public int columnWidth = 150;
	public int rowHeight = 30;
	public int rowSpacing = 20;

	ShipBuilder shipbuilder;

	int row = 0;
	int column = 0;

	delegate void UI(int active);
	UI topUI;
	UI currentUI;
	int curActive;
	int parentUI;

	void Start() {
		topUI = Default;
		shipbuilder = ShipBuilder.instance;
	}

	void Update() {
		if(!shipbuilder)
			shipbuilder = ShipBuilder.instance;
	}

	void OnGUI() {
		GUI.skin = skin;
		column = row = -1;

		topUI(-1);
	}

	void Default(int active) {
		currentUI = Default;
		curActive = active;
		CreatePanel ();
		CreateNavButton ("Research",Research);
		CreateNavButton ("Components",Components);

		if (CreateButton ("Spawn Ship")) {
			shipbuilder.SpawnShip();
		}

		if (CreateButton ("Destroy Ship")) {
			shipbuilder.DespawnShip();
		}
	}

	void Research(int active) {
		Default (0);
		curActive = active;
		currentUI = Research;
		CreatePanel ();
	}

	void Components(int active) {
		Default (1);
		curActive = active;
		currentUI = Components;
		CreatePanel ();
		CreateNavButton ("Cockpit", Cockpit);
		CreateNavButton ("Hull", Hull);
		CreateNavButton ("Wings", Wings);
		CreateNavButton ("Engine", Engine);
		CreateNavButton ("Weapons", Weapons);
	}

	void Cockpit(int active) {
		Components (0);
		curActive = active;
		currentUI = Cockpit;

		CreatePanel ();
		for (int i = 0; i < shipbuilder.cockpits.Length;i++) {
			if (CreateButton ("Cockpit " + (i+1), i==shipbuilder.cockpitIndex))
				shipbuilder.cockpitIndex = i;
		}

		Component(ref shipbuilder.cockpitColor);
	}

	void Hull(int active) {
		Components (1);
		curActive = active;
		currentUI = Hull;

		CreatePanel ();
		for (int i = 0; i < shipbuilder.hulls.Length;i++) {
			if (CreateButton ("Hull " + (i+1),i==shipbuilder.hullIndex))
				shipbuilder.hullIndex = i;
		}

		Component(ref shipbuilder.hullColor);
	}

	void Wings(int active) {
		Components (2);
		curActive = active;
		currentUI = Wings;

		CreatePanel ();
		for (int i = 0; i < shipbuilder.wings.Length;i++) {
			if (CreateButton ("Wings " + (i+1),i==shipbuilder.wingIndex))
				shipbuilder.wingIndex = i;
		}

		Component(ref shipbuilder.wingColor);
	}

	void Engine(int active) {
		Components (3);
		curActive = active;
		currentUI = Engine;

		CreatePanel ();
		for (int i = 0; i < shipbuilder.engines.Length;i++) {
			if (CreateButton ("Engine " + (i+1),i==shipbuilder.engineIndex))
				shipbuilder.engineIndex = i;
		}

		Component(ref shipbuilder.engineColor);
	}

	void Weapons(int active) {
		Components (4);
		curActive = active;
		currentUI = Weapons;

		CreatePanel ();
		for (int i = 0; i < shipbuilder.weapons.Length;i++) {
			if (CreateButton ("Weapon " + (i+1),i==shipbuilder.weaponIndex))
				shipbuilder.weaponIndex = i;
		}

		Component(ref shipbuilder.weaponColor);
	}

	void Component(ref Color componentColor) {
		Color c = componentColor;
		Color cold = GUI.color;
		GUI.color = c;
		CreateColorIndicator();
		GUI.color = cold;
		
		c.r = CreateColorSlider(c.r);
		c.g = CreateColorSlider (c.g);
		c.b = CreateColorSlider (c.b);
		componentColor = c;
	}

	bool CreateNavButton(string text, UI f) {
		if (++row == curActive) {
			if (GUI.Button (new Rect(columnWidth*column,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth,rowHeight),text,skin.FindStyle ("ActiveButton"))) {
				topUI = currentUI;
				return true;
			}
		} else if (GUI.Button (new Rect(columnWidth*column,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth,rowHeight),text)) {
			topUI = f;
			return true;
		}
		return false;
	}

	bool CreateButton(string text) { 
		return CreateButton (text, false);
	}

	bool CreateButton(string text, bool isActive) {
		row++;
		if (isActive) {
			if (GUI.Button (new Rect(columnWidth*column,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth,rowHeight),text,skin.FindStyle ("ActiveButton"))) {
				return true;
			}
		} else if (GUI.Button (new Rect(columnWidth*column,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth,rowHeight),text)) {
			return true;
		}
		return false;
	}

	float CreateColorSlider(float value) {
		row++;
		return GUI.HorizontalSlider(new Rect(columnWidth*column+5,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth-10,rowHeight),
		                            value, 0, 1);
	}

	void CreatePanel() {
		column++;
		row = -1;
		GUI.Box (new Rect(columnWidth*column,0,columnWidth,Camera.main.pixelHeight),"");
	}

	void CreateColorIndicator() {
		row++;
		GUI.Box (new Rect(columnWidth*column+5,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth-10,rowHeight),"",skin.FindStyle("ColorIndicator"));
	}
}
