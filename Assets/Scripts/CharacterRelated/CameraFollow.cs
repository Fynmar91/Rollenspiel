using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Tilemap tilemap;

	private Transform target;
	private float xMin, xMax, yMin, yMax;
	private Camera cam;
	private Player player;

	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;
		target = GameObject.FindGameObjectWithTag("Player").transform;
		player = target.GetComponent<Player>(); 

		Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
		Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

		SetLimits(minTile, maxTile);
		player.SetLimits(minTile, maxTile);
	}

	private void LateUpdate()
	{
		transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -cam.orthographicSize);
	}

	private void SetLimits(Vector3 minTile, Vector3 maxTile)
	{
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		xMin = minTile.x + width / 2;
		xMax = maxTile.x - width / 2;

		yMin = minTile.y + height / 2;
		yMax = maxTile.y - height / 2;
	}
}
