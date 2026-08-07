using Fighters.Models.Fighters;

namespace Fighters.Extensions
{
    public static class IFighterExtension
    {
        public static bool IsAlive( this IFighter fighter ) => fighter.GetCurrentHealth() > 0;
    }
}
