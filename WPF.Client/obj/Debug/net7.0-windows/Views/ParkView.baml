   M S B A M L   `   `   `  ����E  AWPF.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:WPF.Client.ViewModelsWPF.Client.ViewModels  5          WPF.Client.Views.ParkView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:WPF.Client.ViewModels     5      .��5	      � $��10 10 50 10q�6     � ColumnDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6   $��Welcome to CPM��6-   $��20(�6F   $
��Bold"�6T   $��2��6f   �� 5   
   $��1��6   $��0��6   $��Number Plate��6-   $��0,10,0,0q�6D   �� 5   
   $��1��6   $��1��6   $��0,10,0,0q�6q   ��6/   �� *NumberPlate+ �� UpdateSourceTrigger$ PropertyChanged=�6q   �� 5!   
   $��2��6   $��1��6"    �� Orientation$ 
Horizontal=�62   .+�5"      �� $
��Park��6   $��0,10,0,0q�6$   ��66   �� *ParkCommand+�� 5#      $��UnPark��6   $��	10,10,0,0q�6&   ��69   �� *UnParkCommand+5$      �� 5&   
   $��3��6   $��1��6"   $ 
Horizontal=�62   .+�5'      �� $��Status:��6   $��0,10,0,0q�6&   �� 5(      $��0,10,0,0q�6/   ��6   �� *Status+6/   5)      �� 5+   
   $��4��6   $��1��6   ._�5,      �� $��0,10,0,0q�6/    �� Padding$ 10q�6A   ��6   �� *Entry+6A   ��5.      �� 5/       �� 
TargetType" ��6      BorderStyle �� BasedOn# [  6,   .�50      ��  �� Property � y� �� Value$ Visible=�63   	 �� Triggers	 51      �� 52      
 �� Binding
 6&   �� *Entry+ �� Value 6@   Z� .a�53      ��  � y�$ 	Collapsed=�6;   54      56      58      5:      5;      