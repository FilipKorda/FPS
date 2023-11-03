using FPS.Guns;

namespace FPS.Guns.Modifiers
{
    public interface IModifier
    {
        void Apply(GunScriptableObject Gun);
    }
}