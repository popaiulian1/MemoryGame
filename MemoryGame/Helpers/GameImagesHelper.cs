using System.IO;
using MemoryGame.Models;

namespace MemoryGame.Helpers;

public static class GameImagesHelper
    {
        public static void InitializeGameCategories()
        {
            string rootPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            Console.WriteLine($"[DEBUG-INIT-IMGS] Local path: {rootPath}");

            // Initialize Animals category
            GameCategory.Animals.Images = new[]
            {
                rootPath+"/Assets/Images/Animals/cat.png",
                rootPath+"/Assets/Images/Animals/dog.png",
                rootPath+"/Assets/Images/Animals/dog2.png",
                rootPath+"/Assets/Images/Animals/elephant.png",
                rootPath+"/Assets/Images/Animals/fox.png",
                rootPath+"/Assets/Images/Animals/giraffe.png",
                rootPath+"/Assets/Images/Animals/horse.png",
                rootPath+"/Assets/Images/Animals/lion.png",
                rootPath+"/Assets/Images/Animals/monkey.png",
                rootPath+"/Assets/Images/Animals/panda.png",
                rootPath+"/Assets/Images/Animals/rabbit.png",
                rootPath+"/Assets/Images/Animals/tiger.png",
                rootPath+"/Assets/Images/Animals/zebra.png",
            };
            
            Console.WriteLine($"Animals category initialized with {GameCategory.Animals.Images.Count()} images");
            
            // Initialize Fruits category
            GameCategory.Fruits.Images = new[]
            {
                rootPath+"/Assets/Images/Fruits/apple.png",
                rootPath+"/Assets/Images/Fruits/banana.png",
                rootPath+"/Assets/Images/Fruits/cherry.png",
                rootPath+"/Assets/Images/Fruits/grape.png",
                rootPath+"/Assets/Images/Fruits/kiwi.png",
                rootPath+"/Assets/Images/Fruits/lemon.png",
                rootPath+"/Assets/Images/Fruits/orange.png",
                rootPath+"/Assets/Images/Fruits/peach.png",
                rootPath+"/Assets/Images/Fruits/pear.png",
                rootPath+"/Assets/Images/Fruits/pineapple.png",
                rootPath+"/Assets/Images/Fruits/strawberry.png",
                rootPath+"/Assets/Images/Fruits/watermelon.png",
                rootPath+"/Assets/Images/Fruits/mango.png",
            };
            
            Console.WriteLine($"Fruits category initialized with {GameCategory.Fruits.Images.Count()} images");
            
            // Initialize Flags category
            GameCategory.Flags.Images = new[]
            {
                rootPath+"/Assets/Images/Flags/australia.png",
                rootPath+"/Assets/Images/Flags/brazil.png",
                rootPath+"/Assets/Images/Flags/canada.png",
                rootPath+"/Assets/Images/Flags/china.png",
                rootPath+"/Assets/Images/Flags/france.png",
                rootPath+"/Assets/Images/Flags/germany.png",
                rootPath+"/Assets/Images/Flags/india.png",
                rootPath+"/Assets/Images/Flags/italy.png",
                rootPath+"/Assets/Images/Flags/japan.png",
                rootPath+"/Assets/Images/Flags/spain.png",
                rootPath+"/Assets/Images/Flags/uk.png",
                rootPath+"/Assets/Images/Flags/usa.png",
                rootPath+"/Assets/Images/Flags/romania.png",
                rootPath+"/Assets/Images/Flags/morocco.png",
                rootPath+"/Assets/Images/Flags/fiji.png",
            };
            
            Console.WriteLine($"Flags category initialized with {GameCategory.Flags.Images.Count()} images");
        }
    }