namespace MemoryGame.Models;

public class GameCategory
{
    public string Name { get; set; }
    public IEnumerable<string> Images { get; set; }

    public static readonly GameCategory Animals = new GameCategory
    {
        Name = "Animals",
        Images =
        [
            // TO DO -> Add images
        ]
    };

    public static readonly GameCategory Fruits = new GameCategory
    {
        Name = "Fruits",
        Images =
        [
            // TO DO -> Add images
        ]
    };

    public static readonly GameCategory Flags = new GameCategory
    {
        Name = "Flags",
        Images =
        [
            // TO DO -> Add images
        ]
    };
}