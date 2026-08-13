using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models
{
    public static class FighterCatalog
    {
        public static IReadOnlyList<IRace> Races { get; } =
        [
            new Human(),
            new Orc(),
            new Vampire()
        ];

        public static IReadOnlyList<IFighterClass> Classes { get; } =
        [
            new Knight(),
            new Assassin()
        ];

        public static IReadOnlyList<IWeapon> Weapons { get; } =
        [
            new Fists(),
            new Sword(),
            new Spear()
        ];

        public static IReadOnlyList<IArmor> Armors { get; } =
        [
            new NoArmor(),
            new Chainmail(),
            new Plate()
        ];
    }
}
