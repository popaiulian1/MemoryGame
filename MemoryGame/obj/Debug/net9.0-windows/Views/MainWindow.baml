   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MemoryGame.ViewsMemoryGame.Views  5   	   =#clr-namespace:MemoryGame.ViewModelsMemoryGame.ViewModels  5   	   !    MemoryGame.Views.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0viewsclr-namespace:MemoryGame.Views     5   	   :
viewModels#clr-namespace:MemoryGame.ViewModels     5   	     � Title$  Memory��5	   	   $	��650��6   $	��900��6%    � WindowStartupLocation$ CenterScreen=�61   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��0��6   ���#2c3e50 � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� $��0��6    �� Orientation$ 
Horizontal=�6)   $
��10,5q�6B   .+�5      �� $��New Game��6   $��5q�6,   $
��10,5q�67   ���#3498db
�������$��0q�5   A   ��5      �� *NavigateToGameCommand+y�5      �� *
IsLoggedIn+   BoolToVisibilityConverter �� 	Converter# [  5   A   �� 5      $��
Statistics��6   $��5q�6.   $
��10,5q�69   ���#3498db
�������$��0q�5    A   ��5      �� *NavigateToStatisticsCommand+y�5      �� *
IsLoggedIn+# [  5    A   �� 5"      $��Saved Games��6   $��5q�6/   $
��10,5q�6:   ���#3498db
�������$��0q�5%   A   ��5#      �� *NavigateToSavedGamesCommand+y�5$      �� *
IsLoggedIn+# [  5%   A   �� 5'      $��About��6   $��5q�6)   $
��10,5q�64   ���#3498db
�������$��0q�5*   A   ��5(      �� *OpenAboutCommand+y�5)      �� *
IsLoggedIn+# [  5*   A   5+      �� 5-      $��1��6   $ 
Horizontal=�6)   $
��10,5q�6B   .+�5.      �� $��Center=�6B   $��5q�5/      
���������5.      �� *CurrentUser.Username+y�5/   :   �� *
IsLoggedIn+# [  �� 51      $��Logout��6   $��5q�6*   $
��10,5q�65   ���#e74c3c
�������$��0q�53   A   ��52      �� *LogoutCommand+y�54      �� *
IsLoggedIn+# [  55      56      �� 59   
   $��1��6   ��6&   �� *CurrentView+ � 	Resources 5:       �� DataType*   $MemoryGame.ViewModels.LoginViewModel    MemoryGame.Views.LoginView)   #MemoryGame.ViewModels.GameViewModel   MemoryGame.Views.GameView/   )MemoryGame.ViewModels.StatisticsViewModel%   MemoryGame.Views.StatisticsView-   'MemoryGame.ViewModels.SaveGameViewModel#   MemoryGame.Views.SaveGameView��%,  (��       5;      *, +)(�� 2     5>      *, +)(�� d     5A      *, +)(�� �     5D      *, +)�� 5;      "  6   .R�5<        5=      �� 5>      "  6   .R�5?        5@      �� 5A      "  6   .R�5B        5C      �� 5D      "  6   .R�5E        5F      5G      5H      5I      5J      