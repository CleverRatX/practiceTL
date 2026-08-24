using Fighters.Models;
using Fighters.Models.Armors;
using Fighters.Models.FighterClasses;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Xunit;

namespace Fighters.Tests.Models
{
    public class FighterCatalogTests
    {
        [Fact]
        public void Races_Always_ContainsEveryRaceImplementation()
        {
            Assert.Contains( FighterCatalog.Races, race => race is Human );
            Assert.Contains( FighterCatalog.Races, race => race is Orc );
            Assert.Contains( FighterCatalog.Races, race => race is Vampire );
        }

        [Fact]
        public void Classes_Always_ContainsEveryClassImplementation()
        {
            Assert.Contains( FighterCatalog.Classes, fighterClass => fighterClass is Knight );
            Assert.Contains( FighterCatalog.Classes, fighterClass => fighterClass is Assassin );
        }

        [Fact]
        public void Weapons_Always_ContainsEveryWeaponImplementation()
        {
            Assert.Contains( FighterCatalog.Weapons, weapon => weapon is Fists );
            Assert.Contains( FighterCatalog.Weapons, weapon => weapon is Sword );
            Assert.Contains( FighterCatalog.Weapons, weapon => weapon is Spear );
        }

        [Fact]
        public void Armors_Always_ContainsEveryArmorImplementation()
        {
            Assert.Contains( FighterCatalog.Armors, armor => armor is NoArmor );
            Assert.Contains( FighterCatalog.Armors, armor => armor is Chainmail );
            Assert.Contains( FighterCatalog.Armors, armor => armor is Plate );
        }

        [Fact]
        public void Catalogs_Always_HaveUniqueNamesInsideEachOfThem()
        {
            AssertNamesAreUnique( FighterCatalog.Races.Select( race => race.Name ) );
            AssertNamesAreUnique( FighterCatalog.Classes.Select( fighterClass => fighterClass.Name ) );
            AssertNamesAreUnique( FighterCatalog.Weapons.Select( weapon => weapon.Name ) );
            AssertNamesAreUnique( FighterCatalog.Armors.Select( armor => armor.Name ) );
        }

        private static void AssertNamesAreUnique( IEnumerable<string> names )
        {
            List<string> allNames = names.ToList();

            Assert.NotEmpty( allNames );
            Assert.All( allNames, name => Assert.False( string.IsNullOrWhiteSpace( name ) ) );
            Assert.Equal( allNames.Count, allNames.Distinct().Count() );
        }
    }
}
