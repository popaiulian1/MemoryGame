using MemoryGame.Models;

namespace MemoryGame.Helpers;

public static class GameImagesHelper
    {
        public static void InitializeGameCategories()
        {
            // Initialize Animals category
            GameCategory.Animals.Images = new[]
            {
                "/MemoryGame;component/Assets/Images/Animals/cat.png",
                "/MemoryGame;component/Assets/Images/Animals/dog.png",
                "/MemoryGame;component/Assets/Images/Animals/elephant.png",
                "/MemoryGame;component/Assets/Images/Animals/fox.png",
                "/MemoryGame;component/Assets/Images/Animals/giraffe.png",
                "/MemoryGame;component/Assets/Images/Animals/horse.png",
                "/MemoryGame;component/Assets/Images/Animals/lion.png",
                "/MemoryGame;component/Assets/Images/Animals/monkey.png",
                "/MemoryGame;component/Assets/Images/Animals/panda.png",
                "/MemoryGame;component/Assets/Images/Animals/rabbit.png",
                "/MemoryGame;component/Assets/Images/Animals/tiger.png",
                "/MemoryGame;component/Assets/Images/Animals/zebra.png",
            };
            
            // Initialize Fruits category
            GameCategory.Fruits.Images = new[]
            {
                "/MemoryGame;component/Assets/Images/Fruits/apple.png",
                "/MemoryGame;component/Assets/Images/Fruits/banana.png",
                "/MemoryGame;component/Assets/Images/Fruits/cherry.png",
                "/MemoryGame;component/Assets/Images/Fruits/grape.png",
                "/MemoryGame;component/Assets/Images/Fruits/kiwi.png",
                "/MemoryGame;component/Assets/Images/Fruits/lemon.png",
                "/MemoryGame;component/Assets/Images/Fruits/orange.png",
                "/MemoryGame;component/Assets/Images/Fruits/peach.png",
                "/MemoryGame;component/Assets/Images/Fruits/pear.png",
                "/MemoryGame;component/Assets/Images/Fruits/pineapple.png",
                "/MemoryGame;component/Assets/Images/Fruits/strawberry.png",
                "/MemoryGame;component/Assets/Images/Fruits/watermelon.png",
            };
            
            // Initialize Flags category
            GameCategory.Flags.Images = new[]
            {
                "/MemoryGame;component/Assets/Images/Flags/australia.png",
                "/MemoryGame;component/Assets/Images/Flags/brazil.png",
                "/MemoryGame;component/Assets/Images/Flags/canada.png",
                "/MemoryGame;component/Assets/Images/Flags/china.png",
                "/MemoryGame;component/Assets/Images/Flags/france.png",
                "/MemoryGame;component/Assets/Images/Flags/germany.png",
                "/MemoryGame;component/Assets/Images/Flags/india.png",
                "/MemoryGame;component/Assets/Images/Flags/italy.png",
                "/MemoryGame;component/Assets/Images/Flags/japan.png",
                "/MemoryGame;component/Assets/Images/Flags/spain.png",
                "/MemoryGame;component/Assets/Images/Flags/uk.png",
                "/MemoryGame;component/Assets/Images/Flags/usa.png",
            };
        }
    }