
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Configuration", menuName = "Tower Defense/Projectile Configuration")]
public class ProjectileConfig : ScriptableObject
{
    public ProjectileType type;
    public Mesh mesh;
    public Material material;
    public Vector3 scale = new Vector3(1f, 1f, 1f);

    public void SetupProjectile(Projectile projectile)
    {
        var meshFilter = projectile.GetComponent<MeshFilter>();
        var meshRenderer = projectile.GetComponent<MeshRenderer>();
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        projectile.transform.localScale = scale;
    }
}
