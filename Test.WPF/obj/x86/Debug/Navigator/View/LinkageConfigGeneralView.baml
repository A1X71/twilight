   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      6    0Test.WPF.Navigator.View.LinkageConfigGeneralView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
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
 Auto 6(   $
 A楼��5+   (   # [  5-   (    5,   (   �� $ BuildingNoA�5-   (     5.      $
 Auto 6(   $
 A区��5/   (   # [  51   (    50   (   �� $ ZoneNoA�51   (     52      $
 Auto 6(   $ A层1��53   (   # [  55   (    54   (   �� $ 	LayerNoA1�55   (     56      $
 Auto 6(   $ A层2��57   (   # [  59   (    58   (   �� $ 	LayerNoA2�59   (     5:      $
 Auto 6(   $ 类型A��5;   (   # [  5=   (    5<   (   �� $ DeviceTypeCodeA�5=   (     5>      $
 Auto 6(   $ C分类��5?   (   # [  5A   (    5@   (   �� $ TypeC�5A   (     5C      $
 Auto 6(   $ C楼号��5D   (   # [  5F   (    5E   (   �� $ BuildingNoC�5F   (     5H      $
 Auto 6(   $ C区号��5I   (   # [  5K   (    5J   (   �� $ ZoneNoC�5K   (     5L      $
 Auto 6(   $ C层号��5M   (   # [  5O   (    5N   (   �� $ LayerNoC�5O   (     5P      $
 Auto 6(   $ C机号��5Q   (   # [  5S   (    5R   (   �� $ 
MachineNoC�5S   (     5T      $
 Auto 6(   $ 
C回路号��5U   (   # [  5W   (    5V   (   �� $ LoopNoC�5W   (     5X      $
 Auto 6(   $ C编号��5Y   (   # [  5[   (    5Z   (   �� $ 	DeviceNoC�5[   (     5\      $
 Auto 6(   $ 类型C��5]   (   # [  5_   (    5^   (   �� $ DeviceTypeCodeC�5_   (   5b      5c      5d      