#region Script Synopsis
//Một MonoBehaviour được gắn vào bất kỳ đối tượng nào nhận va chạm từ các viên đạn/laser và khởi tạo vụ nổ nếu đã được thiết lập và áp dụng sát thương cho đối tượng.
//Ví dụ: Bất kỳ đối tượng nào nhận sát thương (người chơi, kẻ thù, v.v.).
//Tìm hiểu thêm về hệ thống va chạm tại: https://neondagger.com/variabullet2d-system-guide/#collision-system
#endregion

using UnityEngine;
using System.Collections;

namespace ND_VariaBULLET
{
    public abstract class ShotCollisionDamage : ShotCollision, IShotCollidable
    {
       
        public float Dame;
        [Tooltip("Đặt tên của prefab vụ nổ sẽ được khởi tạo khi HP = 0.")]
        public string DeathExplosion;

        [Tooltip("Điểm Sức Khỏe. Giảm theo giá trị sát thương của IDamager khi va chạm.")]
        public float HP;

        [Range(0.1f, 8f)]
        [Tooltip("Thay đổi kích thước của vụ nổ cuối cùng (khi HP = 0).")]
        public float FinalExplodeFactor = 2;

        [Tooltip("Kích hoạt hiển thị sát thương bằng cách nhấp nháy màu sắc (thông qua cài đặt DamageColor) khi HP bị giảm.")]
        public bool DamageFlicker;

        [Range(5, 40)]
        [Tooltip("Đặt thời gian hiệu ứng nhấp nháy khi va chạm.")]
        public int FlickerDuration = 6;

        [Tooltip("Đặt màu sắc mà đối tượng sẽ nhấp nháy khi HP giảm và DamageFlicker được kích hoạt.")]
        public Color DamageColor;
        public Color NormalColor;
        public SpriteRenderer rend;
        public virtual void  Start()
        {
            rend = GetComponent<SpriteRenderer>();
            NormalColor = rend.color;
        }

        //check va chạm với vật thể khác và trừ máu của vật thể đó 
        public virtual new IEnumerator OnLaserCollision(CollisionArgs sender)
        {
            if (CollisionFilter.collisionAccepted(sender.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(sender.point))
            {
                setDamage(sender.damage);
                CollisionFilter.setExplosion(LaserExplosion, ParentExplosion, this.transform, new Vector2(sender.point.x, sender.point.y), 0, this);
                yield return setFlicker();
            }
        }


        //Check xem vật thể có va chạm với đạn hay gì đó của đối phương không nếu có thì trừ mauus theo dame của đói phương 

        public virtual new IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionFilter.collisionAccepted(collision.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(collision.contacts[0].point))
            {
                setDamage(collision.gameObject.GetComponent<IDamager>().DMG);
                CollisionFilter.setExplosion(BulletExplosion, ParentExplosion, this.transform, collision.contacts[0].point, 0, this);
                yield return setFlicker();
            }
        }

        public virtual void setDamage(float damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                if (DeathExplosion != "")
                {
                    string explosion = DeathExplosion;
                    GameObject finalExplode = GlobalShotManager.Instance.ExplosionRequest(explosion, this);

                    finalExplode.transform.position = this.transform.position;
                    finalExplode.transform.parent = null;
                    finalExplode.transform.localScale = new Vector2(finalExplode.transform.localScale.x * FinalExplodeFactor, finalExplode.transform.localScale.y * FinalExplodeFactor);
                }

                Destroy(this.gameObject);
            }
        }

        protected IEnumerator setFlicker()
        {
            if (rend == null)
            {
                Utilities.Warn("Không có SpriteRenderer đính kèm. Không thể nhấp nháy khi bị sát thương.", this);
                yield return null;
            }

            if (DamageFlicker)
            {
                bool flicker = false;
                for (int i = 0; i < FlickerDuration * 2; i++)
                {
                    flicker = !flicker;

                    if (flicker)
                        rend.color = DamageColor;
                    else
                        rend.color = NormalColor;

                    yield return null;
                };

                rend.color = NormalColor;
            }
        }
    }
}
