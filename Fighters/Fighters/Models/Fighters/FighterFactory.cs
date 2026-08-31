using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public interface IFighterFactory
    {
        IFighter Create( string name, IRace race, IFighterClass fighterClass, IWeapon weapon, IArmor armor );
    }

    public class FighterFactory : IFighterFactory
    {
        public IFighter Create( string name, IRace race, IFighterClass fighterClass, IWeapon weapon, IArmor armor )
        {
            return new Fighter( name, race, fighterClass, weapon, armor );
        }
    }
}
