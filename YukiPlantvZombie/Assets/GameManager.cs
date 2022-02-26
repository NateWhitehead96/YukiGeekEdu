using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // our currency
    public int suns;
    // what building/plant we want to place
    public PlantBuilding plantToPlace;
    // cursor thats going to hold the plant we want to place
    public CustomCursor customCursor;
    // list of all the tiles
    public Tile[] tiles;
    // the collision layer we use to pick up suns
    public LayerMask SunMask; 

    // Start is called before the first frame update
    void Start()
    {
        customCursor.gameObject.SetActive(false); // hide the custom cursor on start
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && plantToPlace != null) // we're clicking with left click, and we have a plant on our cursor
        {
            Tile nearestTile = null; // creating a tile that will be the nearest tile to our mouse click
            float nearestDistance = float.MaxValue; // the nearest distance to the tile
            foreach (Tile tile in tiles) // look through our grid of tiles
            {
                float distance = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)); // find a distance between the tile and our mouse
                if(distance < nearestDistance) // if our mouse is over the tile
                {
                    nearestDistance = distance;
                    nearestTile = tile;
                }
            }
            if(nearestTile.isOccupied == false)
            {
                Instantiate(plantToPlace, nearestTile.transform.position, transform.rotation); // create/build the plant
                plantToPlace = null; // reset the plant to place back to nothing
                nearestTile.isOccupied = true; // the tile we placed the plant is now occupied
                Cursor.visible = true; // bring back the defualt cursor
                customCursor.gameObject.SetActive(false); // hide that custom cursor
            }
        }
        if (Input.GetMouseButtonDown(0) && plantToPlace == null) // we're just clicking around, with no plant on our mouse
        {
            // we're firing an invisible laser (raycast) onto the screen, and if it hits an object with the sunlayer it will do something
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, SunMask);
            if (hit.collider.GetComponent<Sun>()) // if the thing we're hitting is a sun
            {
                suns += hit.collider.GetComponent<Sun>().inceaseSuns; // increase suns
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void BuyPlant(PlantBuilding plant) // button function to create a plant
    {
        if(suns >= plant.cost)
        {
            customCursor.gameObject.SetActive(true); // show the cursor
            customCursor.GetComponent<SpriteRenderer>().sprite = plant.GetComponent<SpriteRenderer>().sprite; // whatever the plant sprite is, we make the cursor look the same
            Cursor.visible = false; // hide the white defualt cursor to only show the custom cursor

            suns -= plant.cost; // subtract the cost from our total suns
            plantToPlace = plant; // set the plant we want to place to be the plant we're selecting
        }
    }
}
