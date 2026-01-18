using UnityEngine;

public class AnimatorEventReceiver : MonoBehaviour
{
    public OgrePericoBoss boss;

    public void DoJumpDamage()
    {
        if (boss != null)
            boss.DoJumpDamage();
    }

    public void EndAttack()
    {
        if (boss != null)
            boss.EndAttack();
    }
    public void StartJumpMove()
    {
        if (boss != null)
            boss.StartJumpMove();
    }
}

