   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MemoryGame.ViewsMemoryGame.Views  5      =#clr-namespace:MemoryGame.ConvertersMemoryGame.Converters  5          MemoryGame.Views.GameView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:MemoryGame.Views     5      :
converters#clr-namespace:MemoryGame.Converters     5        � 	Resources  5      5   /MemoryGame.Converters.BoolToVisibilityConverter   BoolToVisibilityConverter<   6MemoryGame.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter��%:   &	        &	        5   
   6/     5   
   66   5      .��5      � ���#ecf0f1 � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
����������#bdc3c7$��0,0,0,1q�5       �� Padding$ 20,20q�5      ._�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5       $
��Auto �6'   .+�5#      �� $��0��6    �� Orientation$ 
Horizontal=�5$      y�5%      �� *IsGameStarted+ �� 	Converter# [ .+�5&      �� $��0,0,20,0q�6!   .+�5'      �� $��	Category:��6$   $��0,0,0,5q�65   �� 5(       �� DisplayMemberPath$
 Name��5*   #   $	��150��5+   #   ��5(   #   �� *
Categories+ �� SelectedItem 5)   #   �� *SelectedCategory+5+   #   5,      �� 5.      $��0,0,20,0q�6!   .+�5/      �� $��Width:��6$   $��0,0,0,5q�62   �� 50      -   5         ��Name$ BoardWidthComboBox��50   #   $	��100��53   #   ��51   #   �� *BoardSizeOptions+ 52   #   �� *
BoardWidth+	 �� Mode$	 TwoWay=���54      �� 55   "   .R�56   &   �� ��60   �� 57   #   59      5:      �� 5<      $��0,0,20,0q�6!   .+�5=      �� $��Height:��6$   $��0,0,0,5q�63   �� 5>      -   5        $ BoardHeightComboBox��5>   #   $	��100��5A   #   ��5?   #   �� *BoardSizeOptions+ 5@   #   �� *BoardHeight+$	 TwoWay=���5B      �� 5C   "   .R�5D   &   �� ��60   �� 5E   #   5G      5H      �� 5J      $��
Start Game��6   $��Bottom=�5L      $
��10,5q�5M      ���#2ecc71
�������$��0q�5P      ��5K      �� *StartGameCommand+5P      5Q      �� 5S      $��0��6   $ 
Horizontal=�5T      y�5U      �� *IsGameStarted+# [  .+�5V      �� $��Time: ��6    $��Center=�5W       $
��Bold"�5X       $��0,0,5,0q�5Y       �� 5Z      $��Center=�5[       $��0,0,20,0q�5\       ��5Z       �� *TimeElapsed+
 �� StringFormat$
 mm\:ss��5\       �� 5^      $��Moves: ��6    $��Center=�5_       $
��Bold"�5`       $��0,0,5,0q�5a       �� 5b      $��Center=�5c       $��0,0,20,0q�5d       ��5b       �� *	MoveCount+5d       �� 5f      $��	Matches: ��6    $��Center=�5g       $
��Bold"�5h       $��0,0,5,0q�5i       �� 5j      $��Center=�5k       $��0,0,20,0q�5l       ��5j       �� *
MatchCount+5l       �� 5n      $��Score: ��6    $��Center=�5o       $
��Bold"�5p       $��0,0,5,0q�5q       y�5r       �� *IsGameFinished+# [  �� 5s      $��Center=�5t       $��0,0,20,0q�5u       ��5s       �� *	GameScore+y�5v       �� *IsGameFinished+# [  5w      �� 5y      $��1��6   $ 
Horizontal=�5z      y�5{      �� *IsGameStarted+# [  .+�5|      �� $��	Save Game��6   $
��10,5q�5~      $��0,0,10,0q�5      ���#3498db
�������$��0q�5�      ��5}      �� *SaveGameCommand+y�5�      �� *IsGameFinished+# [ �� 5�      $��
Reset Game��6   $
��10,5q�5�      ���#e74c3c
�������$��0q�5�      ��5�      �� *ResetGameCommand+5�      5�      5�      5�      �� 5�   
   $��1��6   $��20q�6   ._�5�      � .+�5�      �� $?��9Select a category and board size, then click 'Start Game'��6   $��Center=�5�      $��Center=�5�      $��18(�5�      ���#7f8c8dy�5�      �� *IsGameStarted+# [ �� 5�      ��6   �� *Board.Cards+y�5�      �� *IsGameStarted+# [  ��5�      �� 5�      .R�5�      A�  A� Rows 6*   �� *Board.Height+ A� Columns 5�   *   �� *Board.Width+5�      ��5�      �� 5�      .R�5�      �� $��5q�6%    �� CornerRadius$ 5��5�   %   ���#bdc3c7$��1q�5�   %   ��5�   %   �� *	IsMatched+ ! BoolToCardBackgroundConverter# [ ._�5�   "   �� $��0q�5�   )   
������ $��0q�5�   )   ��5�   )   �� *DataContext.CardClickCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��5�   )   �� .��5�   &   � .+�5�   *   �� ���#3498dby�5�   4   �� *
IsSelected+# [ �� 5�   *   $��Uniform=�5�   0   $��10q�5�   0   ��5�   0   �� *	ImagePath+y�5�   0   �� *
IsSelected+# [  5�   '   5�   #   5�      5�      5�      5�      5�      �� 5�   
   $��2��6   
����������#bdc3c7$��0,1,0,0q�5�      $ 20,10q�5�      ._�5�      �� $��Center=�5�      ��5�      �� *
GameStatus+5�      5�      5�      5�      