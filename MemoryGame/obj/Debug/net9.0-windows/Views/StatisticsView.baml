   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MemoryGame.ViewsMemoryGame.Views  5      %    MemoryGame.Views.StatisticsView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:MemoryGame.Views     5      .��5
      � ���#ecf0f1  � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
����������#bdc3c7$��0,0,0,1q�5       �� Padding$ 20,10q�5      ._�5      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   .+�5      �� $��0��6   $��Game Statistics��5      $��20(�5      $
��Bold"�5       $��Center=�5!      �� 5#      $��1��6    �� Orientation$ 
Horizontal=�6-   .+�5$      �� $��Reset My Stats��6   $
��10,5q�5&      $��0,0,10,0q�5'      ���#e74c3c
�������$��0q�5*      ��5%      �� *ResetStatsCommand+5*      �� 5,      $��Refresh��6   $
��10,5q�5.      ���#3498db
�������$��0q�51      ��5-      �� *RefreshStatsCommand+51      52      53      54      � 57   
   $��1��6   $��20q�6    58      �� 59      $��* �6#   �� 5:      $��* �6#   .+�5>      �� $��0��6   
����������#bdc3c7$��1q�5A      $��0,0,10,0q�5B       �� CornerRadius$ 5��5C      ._�5D      � $��15q�6     5E      �� 5F      $
��Auto �6(   �� 5G      $��* �6(   .+�5J      �� $��0��6    $��Your Statistics��5K       $��18(�5L       $
��Bold"�5M       $��0,0,0,15q�5N       �� 5P      $��1��6!   .+�5Q      � $	��0,5q�6    5R      �� 5S   "   $	��120 �63   �� 5T   "   $��* �63   .+�5V      �� $��0��6(   $��Games Played:��5W   (   $��SemiBold"�5X   (   �� 5Y      $��1��6(   ��5Z   (   �� *$"CurrentUser.Statistics.GamesPlayed+5[      � 5]      $	��0,5q�6    5^      �� 5_   "   $	��120 �63   �� 5`   "   $��* �63   .+�5b      �� $��0��6(   $��
Games Won:��5c   (   $��SemiBold"�5d   (   �� 5e      $��1��6(   ��5f   (   �� *!CurrentUser.Statistics.GamesWon+5g      � 5i      $	��0,5q�6    5j      �� 5k   "   $	��120 �63   �� 5l   "   $��* �63   .+�5n      �� $��0��6(   $��	Win Rate:��5o   (   $��SemiBold"�5p   (   �� 5q      $��1��6(   �� 5r   "   �6&   �� *Statistics.GamesWon+ �� Mode$ OneWay=� �� 5s   "   $�/��6&    �� 5t   "   �6&   �� *Statistics.GamesPlayed+$ OneWay=� �� 5u   "   $� (��6&    �� 5v   "   �6&   �� *
Statistics+   WinRateConverter �� 	Converter# [  $ OneWay=� �� 5w   "   $�%)��6&   5x      5y      5z      5{      5|      �� 5      $��1��6   
����������#bdc3c7$��1q�5�      $��10,0,0,0q�5�      $ 5��5�      ._�5�      � $��15q�6     5�      �� 5�      $
��Auto �6(   �� 5�      $��* �6(   .+�5�      �� $��0��6    $��Top Players��5�       $��18(�5�       $
��Bold"�5�       $��0,0,0,15q�5�       �� 5�      $��1��6   $��0q�5�      
������ ��5�      �� *
TopPlayers+ �� View 5�      �� 5�      .P�5�   "   �� $
��Rank��61    �� Width$ 60��6?   	 �� CellTemplate	 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�5�   8   ��5�   8   �� 
 �� RelativeSource
 �� *FindAncestor+ �� AncestorType" ��  IndexConverter# [ 5�   8   5�   +   5�   #   �� 5�   "   $��Username��61   $	 150��5�   1    �� DisplayMemberBinding 5�   1   �� *
Username+5�   1   �� 5�   "   $��Games Played��61   $	 100��6G   	 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�5�   8   ��5�   8   �� *Statistics.GamesPlayed+5�   8   5�   +   5�   #   �� 5�   "   $��Win Rate��61   $	 100��6C   	 5�   &   �� 5�   *   .R�5�   .   �� $��Center=�68   �� 5�   2   �66   �� *Statistics.GamesWon+$ OneWay=� �� 5�   2   $�/��66    �� 5�   2   �66   �� *Statistics.GamesPlayed+$ OneWay=� �� 5�   2   $� (��66    �� 5�   2   �66   �� *Statistics.GamesPlayed+# [  $ OneWay=� �� 5�   2   $�%)��66   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      �� 5�   
   $��2��6   
����������#bdc3c7$��0,1,0,0q�5�      $ 20,10q�5�      ._�5�      �� $��Center=�5�      ��5�      �� *StatusMessage+5�      5�      5�      5�      