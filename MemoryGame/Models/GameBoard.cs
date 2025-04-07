using System.Collections.ObjectModel;
using MemoryGame.Helpers;

namespace MemoryGame.Models;

public class GameBoard : ObservableObject
{
    private int _width;
    private int _height;
    private ObservableCollection<GameCard> _cards;
    private GameCategory _category;
    private List<GameCard> _revealedCards;
    
    public int Width { get => _width; set => SetProperty(ref _width, value); }
    public int Height { get => _height; set => SetProperty(ref _height, value); }
    public ObservableCollection<GameCard> Cards { get => _cards; set => SetProperty(ref _cards, value); }
    public GameCategory Category { get => _category; set => SetProperty(ref _category, value); }

    public GameBoard(int width, int height, GameCategory category)
    {
        if (width * height % 2 != 0)
        {
            throw new ArgumentException("Board dimensions must result in an even number of cards");
        }
        
        if (width < 2 || width > 6 || height < 2 || height > 6)
        {
            throw new ArgumentException("Board dimensions must be between 2x2 and 6x6");
        }

        Width = width;
        Height = height;
        Category = category;
        _revealedCards = new List<GameCard>();
        InitializeBoard();

        // Debug logging
        Console.WriteLine($"GameBoard created: {Width}x{Height}, {Cards.Count} cards");
    }

    private void InitializeBoard()
    {
        Cards = new ObservableCollection<GameCard>();
        var cardImages = GetCardImagesForCategory();
        var shuffledImages = ShuffleImages(cardImages);

        for (int i = 0; i < Width * Height; i++)
        {
            Cards.Add(
                new GameCard
                {
                    Id = i, 
                    ImagePath = shuffledImages[i], 
                    IsSelected = false, 
                    IsMatched = false
                });
        }
        
        Console.WriteLine($"InitializeBoard: Created {Cards.Count} cards for {Width}x{Height} board");
    }
    
    public void HideUnmatchedCards()
    {
        foreach (var card in Cards.Where(c => c.IsSelected && !c.IsMatched))
        {
            card.IsSelected = false;
        }
        
        // Clear the revealed cards list
        _revealedCards.Clear();
    }

    private List<string> GetCardImagesForCategory()
    {
        var images = Category.Images.ToList();
        
        int requiredPairs = (Width * Height) / 2;

        if (images.Count < requiredPairs)
        {
            throw new InvalidOperationException("Not enough card images in selected category");
        }
        
        var selectedImages = images.Take(requiredPairs).ToList();
        selectedImages.AddRange(selectedImages);
        return selectedImages;
    }

    private List<string> ShuffleImages(List<string> images)
    {
        var random = new Random();
        return images.OrderBy(x => random.Next()).ToList();
    }

    public bool RevealCard(GameCard card)
    {
        // Don't allow more than 2 cards to be revealed at once
        if (_revealedCards.Count >= 2)
            return false;
        
        if (card.IsMatched || card.IsSelected) 
            return false;

        card.IsSelected = true;
        _revealedCards.Add(card);

        if (_revealedCards.Count == 2)
        {
            return CheckForMatch();
        }

        return true;
    }

    private bool CheckForMatch()
    {
        if (_revealedCards.Count != 2) return false;
        
        var card1 = _revealedCards[0];
        var card2 = _revealedCards[1];
        
        bool isMatched = card1.ImagePath == card2.ImagePath;

        if (isMatched)
        {
            card1.IsMatched = true;
            card2.IsMatched = true;
            _revealedCards.Clear(); // Only clear if matched
        }
        
        return isMatched;
    }

    public void Reset()
    {
        InitializeBoard();
        _revealedCards.Clear();
    }
}