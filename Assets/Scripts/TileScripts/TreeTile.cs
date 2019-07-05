using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeTile : Tile
{
	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		if (go)
		{
			go.GetComponent<SpriteRenderer>().sortingOrder = -position.y * 2;

			UnityEngine.Random.seed = position.x * position.y * 100;
			float green = UnityEngine.Random.Range(0.7f, 1.0f);
			float blue = UnityEngine.Random.Range(0.8f, 1.0f);
			float scale = UnityEngine.Random.Range(0.6f, 1.0f);

			go.GetComponent<SpriteRenderer>().color = new Color(1, green, blue, 1);
			go.transform.localScale = new Vector3(scale, scale, scale);
		}		

		return base.StartUp(position, tilemap, go);
	}


#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/TreeTile")]
	public static void CreateWaterTile()
	{
		string path = EditorUtility.SaveFilePanelInProject("Save TreeTile", "New TreeTile", "asset", "Save TreeTile", "Assets");
		if (path == "")
		{
			return;
		}
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TreeTile>(), path);
	}
#endif
}

