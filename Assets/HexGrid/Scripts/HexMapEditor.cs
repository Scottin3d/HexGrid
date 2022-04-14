using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace HexGrid
{
	public class HexMapEditor : MonoBehaviour
	{
		public Color[] colors;
		public HexGrid hexGrid;
		private Color activeColor;

		void Awake()
		{
			SelectColor(0);
		}
		void Update()
		{
			if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
			{
				HandleInput();
			}
		}

		void HandleInput()
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
			if (Physics.Raycast(inputRay, out RaycastHit hit))
			{
				hexGrid.ColorCell(hit.point, activeColor);
			}
		}

		public void SelectColor(int index)
		{
			activeColor = colors[index];
		}
	}
}
