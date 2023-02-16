using System.Text;
using BL.Lib;

namespace Clientside.Tests;

public class HappywayTests
{
    [Theory]
    [MemberData(nameof(GetTestData))]
    public Task Can_ParseWebsite(string url, string pattern, string group, string delimiter, string output)
    {
        // Given is MemberData

        // When
        string input = new Reader().GetTextinputData(url);
        StringBuilder sb = new();
        IEnumerable<string> candidates = new Parser().GetEachMatch(input, pattern, group, delimiter);

        foreach (var item in candidates)
            sb.AppendLine(item);

        // Then
        Assert.Equal(output, sb.ToString());

        return Task.CompletedTask;
    }

    public static IEnumerable<object[]> GetTestData()
    {
        string output = """
            title="Starship">,Starship,ship
            title="Ship class">,Ship class,class
            title="Class III neutronic fuel carrier">,Class III neutronic fuel carrier,Class
            title="Class 4 stardrive vessel">,Class 4 stardrive vessel,vessel
            title="Survey vessel">,Survey vessel,vessel
            title="Akira class">,Akira class,class
            title="Ambassador class">,Ambassador class,class
            title="Angelou class">,Angelou class,class
            title="Antares class (Federation)">,Antares class (Federation),class
            title="Survey ship">,Survey ship,ship
            title="Science vessel">,Science vessel,vessel
            title="Apollo class">,Apollo class,class
            title="Bradbury class">,Bradbury class,class
            title="Cardenas class">,Cardenas class,class
            title="California class">,California class,class
            title="Support ship">,Support ship,ship
            title="Challenger class">,Challenger class,class
            title="Cheyenne class">,Cheyenne class,class
            title="Constellation class">,Constellation class,class
            title="Constitution class">,Constitution class,class
            title="Starship class">,Starship class,class
            title="Constitution class (alternate reality)">,Constitution class (alternate reality),class
            title="Starship class">,Starship class,class
            title="Constitution class (31st century)">,Constitution class (31st century),class
            title="Courage class">,Courage class,class
            title="Crossfield class">,Crossfield class,class
            title="Science vessel">,Science vessel,vessel
            title="Warship">,Warship,ship
            title="Curiosity class">,Curiosity class,class
            title="Daedalus class">,Daedalus class,class
            title="Danube class">,Danube class,class
            title="Dauntless class">,Dauntless class,class
            title="Defiant class">,Defiant class,class
            title="Warship">,Warship,ship
            title="Dreadnought class">,Dreadnought class,class
            title="Eisenberg class">,Eisenberg class,class
            title="Engle class">,Engle class,class
            title="Erewon class">,Erewon class,class
            title="Excelsior class">,Excelsior class,class
            title="Excelsior II class">,Excelsior II class,class
            title="Federation class">,Federation class,class
            title="Freedom class (22nd century)">,Freedom class (22nd century),class
            title="Starship class">,Starship class,class
            title="Freedom class (24th century)">,Freedom class (24th century),class
            title="Friendship class">,Friendship class,class
            title="Galaxy class (23rd century)">,Galaxy class (23rd century),class
            title="Gagarin class">,Gagarin class,class
            title="Hermes class">,Hermes class,class
            title="Scout ship">,Scout ship,ship
            title="Hoover class">,Hoover class,class
            title="Inquiry class">,Inquiry class,class
            title="Intrepid class">,Intrepid class,class
            title="Intrepid class (32nd century)">,Intrepid class (32nd century),class
            title="Class J starship">,Class J starship,ship
            title="Cargo ship">,Cargo ship,ship
            title="Cadet vessel">,Cadet vessel,vessel
            title="Survey vessel">,Survey vessel,vessel
            title="Korolev class">,Korolev class,class
            title="Lancelot class">,Lancelot class,class
            title="Luna class">,Luna class,class
            title="Magee class">,Magee class,class
            title="Malachowski class">,Malachowski class,class
            title="Mars class">,Mars class,class
            title="Merced class">,Merced class,class
            title="Merian class">,Merian class,class
            title="Miranda class">,Miranda class,class
            title="Science vessel">,Science vessel,vessel
            title="Supply ship">,Supply ship,ship
            title="Federation mission scoutship">,Federation mission scoutship,ship
            title="Scout ship">,Scout ship,ship
            title="Timeship">,Timeship,ship
            title="Nebula class">,Nebula class,class
            title="New Orleans class">,New Orleans class,class
            title="Niagara class">,Niagara class,class
            title="Nimitz class">,Nimitz class,class
            title="Norway class">,Norway class,class
            title="Nova class">,Nova class,class
            title="Science vessel">,Science vessel,vessel
            title="Scout ship">,Scout ship,ship
            title="Obena class">,Obena class,class
            title="Oberth class">,Oberth class,class
            title="Science vessel">,Science vessel,vessel
            title="Olympic class">,Olympic class,class
            title="Medical ship">,Medical ship,ship
            title="Parliament class">,Parliament class,class
            title="Peregrine class">,Peregrine class,class
            title="Courier ship">,Courier ship,ship
            title="Prometheus class">,Prometheus class,class
            title="Protostar class">,Protostar class,class
            title="Ptolemy class">,Ptolemy class,class
            title="Radiant class">,Radiant class,class
            title="Science vessel">,Science vessel,vessel
            title="Renaissance class">,Renaissance class,class
            title="Reliant class">,Reliant class,class
            title="Ross class">,Ross class,class
            title="Saber class">,Saber class,class
            title="Sagan class">,Sagan class,class
            title="Saladin class">,Saladin class,class
            title="Shepard class">,Shepard class,class
            title="Sombra class">,Sombra class,class
            title="Sovereign class">,Sovereign class,class
            title="Federation scout ship">,Federation scout ship,ship
            title="Scout ship">,Scout ship,ship
            title="Soyuz class">,Soyuz class,class
            title="Springfield class">,Springfield class,class
            title="Steamrunner class">,Steamrunner class,class
            title="Sutherland class">,Sutherland class,class
            title="Sydney class">,Sydney class,class
            title="Texas class">,Texas class,class
            title="Automated starship">,Automated starship,ship
            title="Winston&#39;s trading vessel">,Winston&#39;s trading vessel,vessel
            title="Universe class">,Universe class,class
            title="Walker class">,Walker class,class
            title="Wallenberg class">,Wallenberg class,class
            title="Wells class">,Wells class,class
            title="Timeship">,Timeship,ship
            title="Yellowstone class">,Yellowstone class,class
            title="wikipedia:Ship class">,wikipedia:Ship class,class
            title="Constitution class">,Constitution class,class
            title="The Case of Jonathan Doe Starship">,The Case of Jonathan Doe Starship,ship
            title="Earth starship classes">,Earth starship classes,class
            title="Federation starships">,Federation starships,ship
            title="Unnamed Federation starships">,Unnamed Federation starships,ship
            title="wikipedia:Ship class">,wikipedia:Ship class,class
            title="Category:Federation starship classes">,Category:Federation starship classes,class
            
            """;

        string url = "https://memory-alpha.fandom.com/wiki/Federation_starship_classes";
        string pattern = @"title=""([^\""]*(class|ship|vessel)[^\""]*)"">";
        string group = string.Empty;
        string delimiter = ",";

        yield return new object[] { url, pattern, group, delimiter, output };

        url = @"C:\test.html"; // This would be the website saved as html-file locally.

        yield return new object[] { url, pattern, group, delimiter, output };
    }
}
