#region Script Synopsis
    //A monobehavior that is attached to any object that receives collisions from bullet/laser shots and instantiates explosions if set.
    //Examples: Any non-damaging solid object such as terrain, walls, platforms and so forth.
    //Learn more about the collision system at: https://neondagger.com/variabullet2d-system-guide/#collision-system
#endregion

using UnityEngine;
using System.Collections;

namespace ND_VariaBULLET
{
    public class ShotCollision : MonoBehaviour, IShotCollidable
    {
        [Tooltip("Chỉ định các lớp va chạm có thể tạo ra vụ nổ khi va chạm với đối tượng này.")]
        public string[] CollisionList;

        [Tooltip("Chỉ định tên của prefab vụ nổ sẽ được tạo ra khi va chạm với laser. [Lưu ý: prefab cũng cần được tải trước trong GlobalShotManager.ExplosionPrefabs].")]
        public string LaserExplosion;

        [Tooltip("Chỉ định tên của prefab vụ nổ sẽ được tạo ra khi va chạm với viên đạn. [Lưu ý: prefab cũng cần được tải trước trong GlobalShotManager.ExplosionPrefabs].")]
        public string BulletExplosion;

        [Tooltip("Chỉ định liệu vụ nổ có di chuyển cùng đối tượng này hay không, hoặc giữ nguyên tại điểm va chạm.")]
        public bool ParentExplosion = true;
        public IEnumerator OnLaserCollision(CollisionArgs sender)
        {
            if (CollisionFilter.collisionAccepted(sender.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(sender.point))
            {
                CollisionFilter.setExplosion(LaserExplosion, ParentExplosion, this.transform, new Vector2(sender.point.x, sender.point.y), 0, this);
                yield return null;
            }
        }

        public IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionFilter.collisionAccepted(collision.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(collision.contacts[0].point))
            {
                CollisionFilter.setExplosion(BulletExplosion, ParentExplosion, this.transform, collision.contacts[0].point, 0, this);
                yield return null;
            }
        }
    }
}