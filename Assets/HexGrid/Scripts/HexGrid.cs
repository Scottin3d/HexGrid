using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace HexGrid
{
	public class HexGrid : MonoBehaviour
	{
		public int width = 6;
		public int height = 6;

		public HexCell cellPrefab;
		public TextMeshProUGUI cellLabelPrefab;

		public Color defaultColor = Color.white;
		public Color touchedColor = Color.magenta;

		HexCell[] cells;
		Canvas gridCanvas;
		HexMesh hexMesh;
		void Awake()
		{
			cells = new HexCell[height * width];
			gridCanvas = GetComponentInChildren<Canvas>();
			hexMesh = GetComponentInChildren<HexMesh>();

			for (int z = 0, i = 0; z < height; z++)
			{
				for (int x = 0; x < width; x++)
				{
					CreateCell(x, z, i++);
				}
			}
		}

		void Start()
		{
			hexMesh.Triangulate(cells);
		}

         private void CreateCell(int x, int z, int i)
		{
			// find position
			Vector3 position;
			position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
			position.y = 0f;
			position.z = z * (HexMetrics.outerRadius * 1.5f);

			// create cell
			HexCell cell = cells[i] = Instantiate(cellPrefab);
			cell.name = string.Format("Hex({0}, {1})", x, z);
			cell.transform.SetParent(transform, false);
			cell.transform.localPosition = position;
			cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
			cell.color = defaultColor;

			// set neighobrs
			if (x > 0)
			{
				cell.SetNeighbor(HexDirection.W, cells[i - 1]);
			}
			if (z > 0)
			{
				if ((z & 1) == 0)
				{
					cell.SetNeighbor(HexDirection.SE, cells[i - width]);
					if (x > 0)
					{
						cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
					}
				}
				else
				{
					cell.SetNeighbor(HexDirection.SW, cells[i - width]);
					if (x < width - 1)
					{
						cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
					}
				}
			}

			// create label
			TextMeshProUGUI label = Instantiate(cellLabelPrefab);
			label.rectTransform.SetParent(gridCanvas.transform, false);
			label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
			label.text = cell.coordinates.ToStringOnSeparateLines();
		}

		public void ColorCell(Vector3 position, Color color)
		{
			position = transform.InverseTransformPoint(position);
			HexCoordinates coordinates = HexCoordinates.FromPosition(position);
			int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
			HexCell cell = cells[index];
			cell.color = color;
			hexMesh.Triangulate(cells);
		}
	}
}
