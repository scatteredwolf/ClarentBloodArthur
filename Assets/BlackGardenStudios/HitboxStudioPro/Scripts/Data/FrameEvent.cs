using System;

namespace BlackGardenStudios.HitboxStudioPro
{
    [Serializable]
    public enum FrameEvent
    {
        PROJECTILE_1,
        PROJECTILE_2,
        PROJECTILE_3,
        PROJECTILE_4,
        JUMP,
        ENABLE_COLLISIONS,
        DISABLE_COLLISIONS,
        CLEAR_INPUTS,
        RESET_TARGETS,
        ENABLE_MOVE,
        DISABLE_MOVE,
        ENABLE_DIRECTION,
        DISABLE_DIRECTION,
        SET_BOUNCE,
        SET_HEIGHT,
        PLAY_SOUND,
        SET_POISE_DAMAGE,
        SET_ATTACK_DAMAGE
    }
}