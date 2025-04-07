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
        Width = width;
        Height = height;
        Category = category;
        _revealedCards = new List<GameCard>();
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        if (Width * Height % 2 != 0)
        {
            throw new ArgumentException("Board dimensions must result in an even number of cards");
        }
        
        Cards = new ObservableCollection<GameCard>();
        var cardImages = GetCardImagesForCategory();
        var shuffledImages = ShuffleImages(cardImages);

        for (int i = 0; i < Width * Height; i++)
        {
            Cards.Add(
                new GameCard
                {
                    Id = i, ImagePath = shuffledImages[i], 
                    IsSelected = false, IsMatched = false
                });
        }
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
        if (card.IsMatched || card.IsSelected) return false;

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
        }
        else
        {
            card1.IsMatched = false;
            card2.IsMatched = false;
        }
        
        _revealedCards.Clear();
        return isMatched;
    }

    public void Reset()
    {
        InitializeBoard();
        _revealedCards.Clear();
    }
}