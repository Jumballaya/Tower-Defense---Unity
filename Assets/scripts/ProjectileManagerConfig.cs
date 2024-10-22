
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Manager Configuration", menuName = "Tower Defense/Projectile Manager Configuration")]
public class ProjectileManagerConfig : ScriptableObject
{
  public ProjectileConfig arrowConfig;
  public ProjectileConfig boltConfig;
  public ProjectileConfig boulderConfig;
  public ProjectileConfig cannonBallConfig;
  public ProjectileConfig bulletConfig;


  public ProjectileConfig GetConfig(ProjectileType type)
  {
    switch (type)
    {
      case ProjectileType.Arrow:
        {
          return arrowConfig;
        }
      case ProjectileType.Bolt:
        {
          return boltConfig;
        }
      case ProjectileType.Boulder:
        {
          return boulderConfig;
        }
      case ProjectileType.Bullet:
        {
          return bulletConfig;
        }
      case ProjectileType.CannonBall:
        {
          return cannonBallConfig;
        }
    }
    return arrowConfig;
  }
}