using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] public bool isPlaceable;

    private Color hoverColor;

    private GameObject tower;
    private Color startColor;
    private Color placeable = new Color(62 / 255f, 255 / 255f, 42 / 255f, 1f);
    private Color notPlaceable = new Color(255 / 255f, 32 / 255f, 48 / 255f, 1f);
    private bool isMouseOn = false;
    private bool isTowerSelected = false;

    private void Start()
    {
        startColor = sr.color;
        hoverColor = isPlaceable ? placeable : notPlaceable;
    }


    private void OnMouseEnter()
    {
        if (IsPointerOverUIElement())
        {
            return;
        }

        if(BuildManager.main.GetSelectedTower().cost > LevelManager.main.GetMana() || tower != null || !isPlaceable)
        {
            sr.color = notPlaceable;
        }
        else
        {
            sr.color = placeable;
        }
        
        isMouseOn = true;

        BaseTower towerToBuild = BuildManager.main.GetSelectedTower();
        GhostTowerManager.Instance.ShowGhostTower(towerToBuild.prefab, transform.position, sr.color, 0.4f);

        if (tower != null)
        {
            GhostTowerManager.Instance.HideGhostTower();
        }
        else // only show ghost tower if there's no tower on the plot
        {
            GhostTowerManager.Instance.ShowGhostTower(towerToBuild.prefab, transform.position, sr.color, 0.4f);
        }
    }

    private void OnMouseExit()
    {
        if (IsPointerOverUIElement())
        {
            GhostTowerManager.Instance.HideGhostTower();
            sr.color = startColor;
            return;
        }

        sr.color = startColor;
        isTowerSelected = false;

        if (isMouseOn) return;
        isMouseOn = false;

        GhostTowerManager.Instance.HideGhostTower();
    }

    private void OnMouseDown()
    {
        if (IsPointerOverUIElement())
        {
            return;
        }
        // show the range of the tower if it exists
        if (tower != null && !isTowerSelected)
        {
            GhostTowerManager.Instance.ShowGhostTower(tower, transform.position, hoverColor, 0.4f);
            isTowerSelected = true;
        }
        else if (tower == null && isPlaceable)
        {
            isTowerSelected = false;
            GhostTowerManager.Instance.HideGhostTower();

             BaseTower towerToBuild = BuildManager.main.GetSelectedTower();


            if (towerToBuild.cost > LevelManager.main.GetMana())
            {
                print("not enough moneys");
                return;
            }

            LevelManager.main.DecreaseMana(towerToBuild.cost);
            tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        
            GhostTowerManager.Instance.HideGhostTower();
        }
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}   
