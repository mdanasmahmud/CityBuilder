using UnityEngine;

public class AutoTileMaterial : MonoBehaviour
{
    public Material material;  // Assign the material you want to tile
    public Vector2 tileSize = new Vector2(1, 1);  // Set how many times you want the texture to repeat per unit size

    void Start()
    {
        // Get the size of the object's bounds (including scale)
        Renderer renderer = GetComponent<Renderer>();
        Vector3 objectSize = renderer.bounds.size;

        // Calculate the tiling based on object size
        Vector2 tiling = new Vector2(objectSize.x / tileSize.x, objectSize.z / tileSize.y);  // Assuming X and Z plane tiling

        // Apply the tiling to the material
        material.mainTextureScale = tiling;
    }
}
