using System.Windows;
using System.Windows.Controls;

namespace MemoryGame.Views;

public partial class AboutView : Window
{
    public AboutView()
    {
        InitializeComponent();
    }
    
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}