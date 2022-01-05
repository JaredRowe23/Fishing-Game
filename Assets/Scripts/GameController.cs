// This will mainly hold references to game objects which may not have ran
// their Start function to create static instance variables on their own

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button bucketMenuButton;
    public BucketMenu bucketMenu;
    public GameObject mouseOverUI;
    public GameObject itemInfoMenu;
    public GameObject overflowItem;
    public GameObject itemViewer;
    public BucketBehaviour bucket;
    public PowerSlider powerSlider;
    public AngleArrow angleArrow;
    public Canvas rodCanvas;
    public InventoryMenu inventoryMenu;
    public GameObject inventoryMenuButton;
    public GameObject rodMenuButton;
    public GameObject baitMenuButton;
    public GameObject gearMenuButton;
    public GameObject equippedRod;
    public RodsMenu rodsMenu;
    public PauseMenu pauseMenu;

    public static GameController instance;
    
    void Start()
    {
        instance = this;
    }
}
