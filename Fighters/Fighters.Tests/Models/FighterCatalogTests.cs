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
            // Act
            IReadOnlyList<IRace> races = FighterCatalog.Races;

            // Assert
            Assert.Contains( races, race => race is Human );
            Assert.Contains( races, race => race is Orc );
            Assert.Contains( races, race => race is Vampire );
        }

        [Fact]
        public void Classes_Always_ContainsEveryClassImplementation()
        {
            // Act
            IReadOnlyList<IFighterClass> classes = FighterCatalog.Classes;

            // Assert
            Assert.Contains( classes, fighterClass => fighterClass is Knight );
            Assert.Contains( classes, fighterClass => fighterClass is Assassin );
        }

        [Fact]
        public void Weapons_Always_ContainsEveryWeaponImplementation()
        {
            // Act
            IReadOnlyList<IWeapon> weapons = FighterCatalog.Weapons;

            // Assert
            Assert.Contains( weapons, weapon => weapon is Fists );
            Assert.Contains( weapons, weapon => weapon is Sword );
            Assert.Contains( weapons, weapon => weapon is Spear );
        }

        [Fact]
        public void Armors_Always_ContainsEveryArmorImplementation()
        {
            // Act
            IReadOnlyList<IArmor> armors = FighterCatalog.Armors;

            // Assert
            Assert.Contains( armors, armor => armor is NoArmor );
            Assert.Contains( armors, armor => armor is Chainmail );
            Assert.Contains( armors, armor => armor is Plate );
        }

        [Fact]
        public void Catalogs_Always_HaveUniqueNamesInsideEachOfThem()
        {
            // Act & Assert
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
