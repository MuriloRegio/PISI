using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class hud : MonoBehaviour
{
	public int row_x_start = 350;
	public int row_y_start = 110;
	public int row_step    = 40;
	public int row_diff    = 5;

	public int col_x_start = 415;
	public int col_y_start = 50;
	public int col_step    = 30;
	public int col_diff    = 15;

	public int size = 20;

	private GUIStyle guiStyle = new GUIStyle();

	void Start() {
		guiStyle.normal.textColor = Color.white;
	}

	void OnGUI() {
		guiStyle.fontSize = size;
		for (int i = 0; i < 8; i++){
			char index = (char) ('A' + i);
			GUI.Label(new Rect(row_x_start, row_y_start+i*(row_step+row_diff), 100, 20), ""+index, guiStyle);
		}

		for (int i = 0; i < 8; i++){
			int index = i + 1;
			GUI.Label(new Rect(col_x_start+i*(col_step+col_diff), col_y_start, 100, 20), ""+index, guiStyle);
		}
	}
}