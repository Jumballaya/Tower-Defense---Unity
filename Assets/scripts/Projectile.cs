

using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Projectile : PoolableObject
{
  private ProjectileConfig config = null;

  public void OnEnable()
  {
    if (Config?.GetType() == typeof(ProjectileConfig))
    {
      config.SetupProjectile(this);
    }
  }

  public override ScriptableObject Config
  {
    get
    {
      return config;
    }
    set
    {
      if (value.GetType() == typeof(ProjectileConfig))
      {
        config = (ProjectileConfig)value;
        config.SetupProjectile(this);
      }
    }
  }
}