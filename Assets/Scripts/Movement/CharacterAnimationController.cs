using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteAnimator))]
public class CharacterAnimationController : MonoBehaviour
{
    public SpriteAnimation Idle;
    public SpriteAnimation Walk;
    public SpriteAnimation Jump;

    private Rigidbody2D _rigid;
    private SpriteAnimator _animator;

    private float _lastTimeOnGround;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<SpriteAnimator>();
    }

    private void Update()
    {
        const float WALK_ANIMATION_SPEED_THRESHOLD = 1f;
        const float WALK_TO_FALL_ANIMATION_TIME = 0.25f;
        const float JUMP_FORCE = 10f;

        if (!this.OnGround2D())
        {
            if ((Time.unscaledTime - _lastTimeOnGround) > WALK_TO_FALL_ANIMATION_TIME || _rigid.velocity.y >= JUMP_FORCE)
            {
                _animator.Animation = Jump;
                return;
            }
        }
        else
        {
            _lastTimeOnGround = Time.unscaledTime;
        }


        if (Math.Abs(_rigid.velocity.x) >= WALK_ANIMATION_SPEED_THRESHOLD)
        {
            _animator.Animation = Walk;
        }
        else
        {
            _animator.Animation = Idle;
        }
            
    }
}