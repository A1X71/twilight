   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      4    .Test.WPF.Navigator.View.LinkageConfigMixedView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
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
 Auto 6(      Header$ ID��5   (   4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [  5    (      Binding 5   (   ��  �� Path$ ID�5    (     5!      $	 120 6(   $ 编号��5"   (   # [  5$   (    5#   (   �� $
 Code�5%        5&      $
 Auto 6(   $ 动作常数��5'   (   # [  5)   (    5(   (   �� $ ActionCoefficient�5)   (     5*      $
 Auto 6(   $ 动作类型��5+   (   # [  5-   (    5,   (   �� $ 
ActionType�5-   (     5.      $
 Auto 6(   $ A分类��5/   (   # [  51   (    50   (   �� $ TypeA�51   (     52      $
 Auto 6(   $
 A楼��53   (   # [  55   (    54   (   �� $ BuildingNoA�55   (     56      $
 Auto 6(   $
 A区��57   (   # [  59   (    58   (   �� $ ZoneNoA�59   (     5:      $
 Auto 6(   $
 A层��5;   (   # [  5=   (    5<   (   �� $ LayerNoA�5=   (     5>      $
 Auto 6(   $ A路号��5?   (   # [  5A   (    5@   (   �� $ LoopNoA�5A   (     5B      $
 Auto 6(   $ A编号��5C   (   # [  5E   (    5D   (   �� $ DeviceCodeA�5E   (     5F      $
 Auto 6(   $ A类型��5G   (   # [  5I   (    5H   (   �� $ DeviceTypeCodeA�5I   (     5K      $
 Auto 6(   $ B分类��5L   (   # [  5N   (    5M   (   �� $ TypeB�5N   (     5O      $
 Auto 6(   $
 B楼��5P   (   # [  5R   (    5Q   (   �� $ BuildingNoB�5R   (     5S      $
 Auto 6(   $
 B区��5T   (   # [  5V   (    5U   (   �� $ ZoneNoB�5V   (     5W      $
 Auto 6(   $
 B层��5X   (   # [  5Z   (    5Y   (   �� $ LayerNoB�5Z   (     5[      $
 Auto 6(   $ B路号��5\   (   # [  5^   (    5]   (   �� $ LoopNoB�5^   (     5_      $
 Auto 6(   $ B编号��5`   (   # [  5b   (    5a   (   �� $ DeviceCodeB�5b   (     5c      $
 Auto 6(   $ B类型��5d   (   # [  5f   (    5e   (   �� $ DeviceTypeCodeB�5f   (     5i      $
 Auto 6(   $ C分类��5j   (   # [  5l   (    5k   (   �� $ TypeC�5l   (     5n      $
 Auto 6(   $ C楼号��5o   (   # [  5q   (    5p   (   �� $ BuildingNoC�5q   (     5s      $
 Auto 6(   $ C区号��5t   (   # [  5v   (    5u   (   �� $ ZoneNoC�5v   (     5w      $
 Auto 6(   $ C层号��5x   (   # [  5z   (    5y   (   �� $ LayerNoC�5z   (     5{      $
 Auto 6(   $ C机号��5|   (   # [  5~   (    5}   (   �� $ 
MachineNoC�5~   (     5      $
 Auto 6(   $ 
C回路号��5�   (   # [  5�   (    5�   (   �� $ LoopNoC�5�   (     5�      $
 Auto 6(   $ C编号��5�   (   # [  5�   (    5�   (   �� $ 	DeviceNoC�5�   (     5�      $
 Auto 6(   $ C类型��5�   (   # [  5�   (    5�   (   �� $ DeviceTypeCodeC�5�   (   5�      5�      5�      