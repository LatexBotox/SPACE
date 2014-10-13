using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangarUI : MonoBehaviour {
	public GUISkin skin;
	public int columnWidth = 150;
	public int rowHeight = 30;
	public int rowSpacing = 20;

	GUIStyle iconActive, iconDisabled, iconNormal, buttonActive, colorIndicator, emptyStyle;

	ShipBuilder shipbuilder;

	int row = 0;
	int column = 0;

	Color setAllColor = Color.white;

	delegate void UI(int active);
	UI topUI;
	UI currentUI;
	int curActive;
	int parentUI;

	int selectedPartIndex = -1;
	int selectedPartID = -1;
	UI 	selectedPartType;

	void Start() {
		topUI = Default;
		shipbuilder = ShipBuilder.instance;
		iconActive = skin.GetStyle("Icon Active");
		iconDisabled = skin.GetStyle ("Icon Disabled");
		iconNormal = skin.GetStyle ("Icon Button");
		buttonActive = skin.GetStyle("ActiveButton");
		colorIndicator = skin.GetStyle ("ColorIndicator");
		emptyStyle = skin.GetStyle ("Empty");
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
		GUI.depth = 0;
		CreatePanel ();
		CreateNavButton ("Research",Research);
		CreateNavButton ("Components",Components);

        //Debug stuff

		/*if (CreateButton ("Spawn Ship")) {
			shipbuilder.SpawnShip();
		}

		if (CreateButton ("Destroy Ship")) {
			shipbuilder.DespawnShip();
		}
        */
		if (CreateButton ("Save")) 
			shipbuilder.Save ();
        /*

		if (CreateButton ("Load"))
			shipbuilder.Load (0);*/
	}

	void Research(int active) {
		Default (0);
		curActive = active;
		currentUI = Research;
		CreatePanel ();
		Rect r = new Rect(column*columnWidth+(columnWidth-32)/2,rowSpacing,32,32);
		foreach (Upgrade u in shipbuilder.upgrades) {
			Upgrade (u, r);
			r.y += rowHeight+rowSpacing;
		}

		r = new Rect(column*columnWidth+(columnWidth-32)/2,rowSpacing,32,32);
		foreach (Upgrade u in shipbuilder.upgrades) {
			UpgradeTooltip (u, r);
			r.y += rowHeight+rowSpacing;
		}
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

		Color c = setAllColor;
		Color cold = GUI.color;
		GUI.color = c;
		CreateColorIndicator();
		GUI.color = cold;
		
		c.r = CreateColorSlider (c.r);
		c.g = CreateColorSlider (c.g);
		c.b = CreateColorSlider (c.b);
		if (c!=setAllColor) {
			shipbuilder.SetAllColors(c);
			setAllColor = c;
		}
	}

	void Cockpit(int active) {
		Components (0);
		curActive = active;
		currentUI = Cockpit;

		CreatePanel ();
        
        int i = 0;
        foreach (Cockpit c in shipbuilder.cockpits)
        {
            if (CreateNavButton(c.componentName, SpecificPart))
            {
                selectedPartID = c.uniqueID;
                selectedPartIndex = i;
                selectedPartType = Cockpit;
            }

            i++;
        }

		shipbuilder.cockpitColor = Component(shipbuilder.cockpitColor);
	}

	void Hull(int active) {
		Components (1);
		curActive = active;
		currentUI = Hull;

		CreatePanel ();

        int i = 0;
        foreach (Hull h in shipbuilder.hulls)
        {
            if (CreateNavButton(h.componentName, SpecificPart))
            {
                selectedPartID = h.uniqueID;
                selectedPartIndex = i;
                selectedPartType = Hull;
            }

            i++;
        }

		shipbuilder.hullColor = Component(shipbuilder.hullColor);
	}

	void Wings(int active) {
		Components (2);
		curActive = active;
		currentUI = Wings;

		CreatePanel ();

        int i = 0;
        foreach (Wings w in shipbuilder.wings)
        {
            if (CreateNavButton(w.componentName, SpecificPart))
            {
                selectedPartID = w.uniqueID;
                selectedPartIndex = i;
                selectedPartType = Wings;
            }

            i++;
        }

		shipbuilder.wingColor = Component(shipbuilder.wingColor);
	}

	void Engine(int active) {
		Components (3);
		curActive = active;
		currentUI = Engine;

		CreatePanel ();

        int i = 0;
        foreach (Engine e in shipbuilder.engines)
        {
            if (CreateNavButton(e.componentName, SpecificPart))
            {
                selectedPartID = e.uniqueID;
                selectedPartIndex = i;
                selectedPartType = Engine;
            }

            i++;
        }

		shipbuilder.engineColor = Component(shipbuilder.engineColor);
	}

	void Weapons(int active) {
		Components (4);
		curActive = active;
		currentUI = Weapons;

		CreatePanel ();

        int i = 0;
        foreach (Weapon w in shipbuilder.weapons)
        {
            if (CreateNavButton(w.componentName, SpecificPart))
            {
                selectedPartID = w.uniqueID;
                selectedPartIndex = i;
                selectedPartType = Weapons;
            }

            i++;
        }

		shipbuilder.weaponColor = Component(shipbuilder.weaponColor);
	}

	Color Component(Color componentColor) {
		Color c = componentColor;
		Color cold = GUI.color;
		GUI.color = c;
		CreateColorIndicator();
		GUI.color = cold;
		
		c.r = CreateColorSlider(c.r);
		c.g = CreateColorSlider (c.g);
		c.b = CreateColorSlider (c.b);
		return c;
	}

	void Upgrade(Upgrade u, Rect r) {
		GUI.DrawTexture (r, u.icon);
		if (u.bought) {
			GUI.Button (r,"",iconActive);
		} else if (u.preReq==null||u.preReq.bought) {
			if (GUI.Button (r,"",iconNormal))
				u.Buy();
		} else {
			GUI.Button (r,"",iconDisabled);
		}
	}

	void SpecificPart(int active) {
		if (selectedPartID < 0)
			return;

		selectedPartType(selectedPartIndex);

        column++;
        //float width = Camera.main.pixelWidth - columnWidth * column;

        if (shipbuilder.IsBought(selectedPartID))
        {
            if (GUI.Button(new Rect(Camera.main.pixelWidth - columnWidth - 5, Camera.main.pixelHeight - rowHeight - 5, columnWidth, rowHeight), "Equip"))
            {
                shipbuilder.Equip(selectedPartID);
            }
        }
        else if (shipbuilder.IsUnlocked(selectedPartID))
        {
            if (GUI.Button(new Rect(Camera.main.pixelWidth - columnWidth - 5, Camera.main.pixelHeight - rowHeight - 5, columnWidth, rowHeight), "Buy - " + 999))
            {
                shipbuilder.Buy(selectedPartID);
            }
        }
	}

	void UpgradeTooltip(Upgrade u, Rect r) {
		GUI.Button (r,new GUIContent("",u.Tooltip),emptyStyle);

		float w = skin.label.fixedWidth;
		float h = skin.label.CalcHeight(new GUIContent(GUI.tooltip),w);
		
		if (!string.IsNullOrEmpty(GUI.tooltip))
			GUI.Label (new Rect(Mathf.Clamp (Event.current.mousePosition.x-w,5,Camera.main.pixelWidth-w-5),
			                    Mathf.Clamp (Event.current.mousePosition.y-h,5,Camera.main.pixelHeight-h-5),w,h),GUI.tooltip);
		GUI.tooltip = null;
	}

	bool CreateNavButton(string text, UI f) {
		if (++row == curActive) {
			if (GUI.Button (new Rect(columnWidth*column,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth,rowHeight),text,buttonActive)) {
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
			if (GUI.Button (new Rect(columnWidth*column,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth,rowHeight),text,buttonActive)) {
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
		GUI.Box (new Rect(columnWidth*column+5,rowSpacing+(rowHeight+rowSpacing)*row,columnWidth-10,rowHeight),"",colorIndicator);
	}
}
