using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile
{
	[SerializeField]
	private Sprite[] waterSprites;

	[SerializeField]
	private Sprite preview;

	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		return base.StartUp(position, tilemap, go);
	}

	public override void RefreshTile(Vector3Int position, ITilemap tilemap)
	{
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

				if (HasWater(tilemap, nPos))
				{
					tilemap.RefreshTile(nPos);
				}
			}
		}
	}

	public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
	{
		string composition = string.Empty;
		base.GetTileData(location, tilemap, ref tileData);

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x != 0 || y != 0)
				{
					if (HasWater(tilemap, new Vector3Int(location.x + x, location.y + y, location.z)))
					{
						composition += 'W';
					}
					else
					{
						composition += 'G';
					}
				}
				
			}
		}
		int randomVal = Random.Range(0, 100);

		if (randomVal < 15)
		{
			tileData.sprite = waterSprites[46];
		}
		else if (randomVal >= 15 && randomVal < 35)
		{

			tileData.sprite = waterSprites[48];
		}
		else
		{
			tileData.sprite = waterSprites[47];
		}



		if (composition[1] == 'G' && composition[3] == 'G' && composition[4] == 'G' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[0];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'G' && composition[5] == 'W' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[1];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'G' && composition[5] == 'G' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[2];
		}
		else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'G' && composition[5] == 'W' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[3];
		}
		else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'G' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[4];
		}
		else if (composition[0] == 'G' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'G' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[5];
		}
		else if (composition[0] == 'G' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'G' && composition[5] == 'W' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[6];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'W')
		{
			tileData.sprite = waterSprites[7];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W' && composition[5] == 'G' && composition[7] == 'W')
		{
			tileData.sprite = waterSprites[8];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'G')
		{
			tileData.sprite = waterSprites[9];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'G' && composition[6] == 'W' && composition[7] == 'G')
		{
			tileData.sprite = waterSprites[10];
		}
		else if (composition[0] == 'G' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[11];
		}
		else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[12];
		}
		else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'G' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[13];
		}
		else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'G' && composition[5] == 'G' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[14];
		}
		else if (composition[0] == 'G' && composition[1] == 'W' && composition[2] == 'G' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[15];
		}
		else if (composition[0] == 'G' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'G' && composition[5] == 'G' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[16];
		}
		else if (composition[1] == 'G' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
		{
			tileData.sprite = waterSprites[17];
		}
		else if (composition[1] == 'G' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'G')
		{
			tileData.sprite = waterSprites[18];
		}
		else if (composition[1] == 'W' && composition[2] == 'W' && composition[4] == 'W' && composition[3] == 'G' && composition[6] == 'W' && composition[7] == 'W')
		{
			tileData.sprite = waterSprites[19];
		}
		else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'G')
		{
			tileData.sprite = waterSprites[20];
		}
		else if (composition[1] == 'W' && composition[2] == 'G' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
		{
			tileData.sprite = waterSprites[21];
		}
		else if (composition[1] == 'W' && composition[2] == 'G' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'G')
		{
			tileData.sprite = waterSprites[22];
		}
		else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[23];
		}
		else if (composition[1] == 'W' && composition[2] == 'G' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[24];
		}
		else if (composition[1] == 'W' && composition[3] == 'G' && composition[4] == 'G' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[25];
		}
		else if (composition[1] == 'G' && composition[3] == 'G' && composition[4] == 'G' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[26];
		}
		else if (composition[1] == 'W' && composition[3] == 'G' && composition[4] == 'G' && composition[6] == 'W')
		{
			tileData.sprite = waterSprites[27];
		}
		else if (composition[1] == 'G' && composition[4] == 'W' && composition[3] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[28];
		}
		else if (composition[1] == 'G' && composition[3] == 'G' && composition[4] == 'W' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[29];
		}
		else if (composition[1] == 'G' && composition[3] == 'W' && composition[4] == 'G' && composition[6] == 'G')
		{
			tileData.sprite = waterSprites[30];
		}
		else if (composition == "GWWWWGWW")
		{
			tileData.sprite = waterSprites[31];
		}
		else if (composition == "GWGWWWWG")
		{
			tileData.sprite = waterSprites[32];
		}
		else if (composition == "GWGWWWWW")
		{
			tileData.sprite = waterSprites[33];
		}
		else if (composition == "WWWWWGWW")
		{
			tileData.sprite = waterSprites[34];
		}
		else if (composition == "WWGWWWWG")
		{
			tileData.sprite = waterSprites[35];
		}
		else if (composition == "WWWWWWWG")
		{
			tileData.sprite = waterSprites[36];
		}
		else if (composition == "GWWWWWWW")
		{
			tileData.sprite = waterSprites[37];
		}
		else if (composition == "WWGWWWWW")
		{
			tileData.sprite = waterSprites[38];
		}
		else if (composition == "GWWWWWWG")
		{
			tileData.sprite = waterSprites[39];
		}
		else if (composition == "GWWWWGWG")
		{
			tileData.sprite = waterSprites[40];
		}
		else if (composition == "WWWWWGWG")
		{
			tileData.sprite = waterSprites[41];
		}
		else if (composition == "WWGWWGWW")
		{
			tileData.sprite = waterSprites[42];
		}
		else if (composition == "GWGWWGWW")
		{
			tileData.sprite = waterSprites[43];
		}
		else if (composition == "WWGWWGWG")
		{
			tileData.sprite = waterSprites[44];
		}
		else if (composition == "GWGWWGWG")
		{
			tileData.sprite = waterSprites[45];
		}
	}

	private bool HasWater(ITilemap tilemap, Vector3Int position)
	{
		return tilemap.GetTile(position) == this;
	}


#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/WaterTile")]
	public static void CreateWaterTile()
	{
		string path = EditorUtility.SaveFilePanelInProject("Save WaterTile", "New WaterTile", "asset", "Save WaterTile", "Assets");
		if (path == "")
		{
			return;
		}
		AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
	}
#endif
}
