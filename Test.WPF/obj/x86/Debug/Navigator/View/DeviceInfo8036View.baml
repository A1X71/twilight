   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      0    *Test.WPF.Navigator.View.DeviceInfo8036View   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
      �   � 	Resources  5   
      tb_ElementStyle �� 
TargetType �� Property �� Value��%U   &	        �� 5   
   " ��6(   .�5      ��  � ��$ Right=�64   5      5      .+�5   
   &    Test.WPF.DataGrid.DeviceDataGrid  -   5          Name$ DataGrid_Standard��5      )  #Microsoft.Windows.Controls.DataGrid   AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�5      
������    RowHeaderWidth$ 25Q�5         	RowHeight$ 40Q�5      	   RowBackground
	 �����
   AlternatingRowBackground

 ����� �� AlternationCount$ 2��5         SelectionUnit$ CellOrRowHeader=�5      $��
0,0,-297,0q�5   .   ��6   ��    Columns 5      3  -Microsoft.Windows.Controls.DataGridTextColumn  5      /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto 6(      Header$ ID��5   (   4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [  5    (      Binding 5   (   ��  �� Path$ ID�5    (     5!      $	 120 6(   $ 编号��5"   (   # [  5$   (    5#   (   �� $
 Code�5%        5&      $
 Auto 6(   $ 器件类型��5'   (   # [  5)   (    5(   (   �� $
 Type�5)   (     5*      $
 Auto 6(   $ 屏蔽��5+   (   # [  5-   (    5,   (   �� $ Disable�5-   (     5.      $
 Auto 6(   $ 
输出组1��5/   (   # [  51   (    50   (   �� $ LinkageGroup1�51   (     52      $
 Auto 6(   $ 
输出组2��53   (   # [  55   (    54   (   �� $ LinkageGroup2�55   (     56      $
 Auto 6(   $ 报警浓度��57   (   # [  59   (    58   (   �� $ 
AlertValue�59   (     5:      $
 Auto 6(   $ 预警浓度��5;   (   # [  5=   (    5<   (   �� $ ForcastValue�5=   (     5>      $
 Auto 6(   $ 延时��5?   (   # [  5A   (    5@   (   �� $ 
DelayValue�5A   (     5B      $	 120 6(   $ 楼号��5C   (   # [  5E   (    5D   (   �� $ 
BuildingNo�5E   (     5F      $	 120 6(   $ 区号��5G   (   # [  5I   (    5H   (   �� $ 
BuildingNo�5I   (     5J      $	 120 6(   $ 层号��5K   (   # [  5M   (    5L   (   �� $ ZoneNo�5M   (     5N      $	 120 6(   $ 	房间号��5O   (   # [  5Q   (    5P   (   �� $ RoomNo�5Q   (     5R      $	 120 6(   $ 安装地点��5S   (   # [  5U   (    5T   (   �� $ Location�5U   (   5W      5X      5Y      