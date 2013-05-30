using UnityEngine;

public static class GUIAssist 
{
	public static GUIStyle BiggerText(int bigness)
	{
		GUIStyle style = new GUIStyle();
		style.fontSize = bigness;
		style.normal.textColor = Color.white;
		return style;
	}
}