   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      7    1Test.WPF.Navigator.View.LinkageConfigStandardView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
      �   � 	Resources  5   
      tb_ElementStyle �� 
TargetType �� Property �� Value��%U   &	        �� 5      " ��6,   .�5      ��  � ��$ Right=�68   5      5      .+�5   
   &    Test.WPF.DataGrid.DeviceDataGrid  -   5          Name$ DataGrid_Standard��5      )  #Microsoft.Windows.Controls.DataGrid   AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�5      
������    RowHeaderWidth$ 25Q�5         	RowHeight$ 40Q�5      	   RowBackground
	 �����
   AlternatingRowBackground

 ����� �� AlternationCount$ 2��5         SelectionUnit$ CellOrRowHeader=�5      $��
0,0,-297,0q�5   .   ��6   ��    Columns 5      3  -Microsoft.Windows.Controls.DataGridTextColumn  5      /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto 6(      Header$ ID��5   (   4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [  5    (      Binding 5   (   ��  �� Path$ ID�5    (     5!      $	 120 6(   $ 输出组号��5"   (   # [  5$   (    5#   (   �� $
 Code�5%        5&      $
 Auto 6(   $ 联动模块1��5'   (   # [  5)   (    5(   (   �� $ 	DeviceNo1�5)   (     5*      $
 Auto 6(   $ 联动模块2��5+   (   # [  5-   (    5,   (   �� $ 	DeviceNo2�5-   (     5.      $
 Auto 6(   $ 联动模块3��5/   (   # [  51   (    50   (   �� $ 	DeviceNo3�51   (     52      $
 Auto 6(   $ 联动模块4��53   (   # [  55   (    54   (   �� $ 	DeviceNo4�55   (     56      $
 Auto 6(   $ 联动模块5��57   (   # [  59   (    58   (   �� $ 	DeviceNo5�59   (     5:      $
 Auto 6(   $ 联动模块6��5;   (   # [  5=   (    5<   (   �� $ 	DeviceNo6�5=   (     5>      $
 Auto 6(   $ 联动模块7��5?   (   # [  5A   (    5@   (   �� $ 	DeviceNo7�5A   (     5C      $
 Auto 6(   $ 联动模块8��5D   (   # [  5F   (    5E   (   �� $ 	DeviceNo8�5F   (     5H      $
 Auto 6(   $ 动作常数��5I   (   # [  5K   (    5J   (   �� $ ActionCoefficient�5K   (     5L      $
 Auto 6(   $ 
联动组1��5M   (   # [  5O   (    5N   (   �� $ 
LinkageNo1�5O   (     5P      $
 Auto 6(   $ 
联动组2��5Q   (   # [  5S   (    5R   (   �� $ 
LinkageNo2�5S   (     5T      $
 Auto 6(   $ 
联动组3��5U   (   # [  5W   (    5V   (   �� $ 
LinkageNo3�5W   (   5Z      5[      5\      