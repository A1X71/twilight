   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      0    *Test.WPF.Navigator.View.DeviceInfo8001View   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
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
 Auto 6(   $ 器件类型��5'   (   # [  5)   (    5(   (   �� $
 Type�5)   (     5*      $
 Auto 6(   $ 特性��5+   (   # [  5-   (    5,   (   �� $ Feature�5-   (     5.      $
 Auto 6(   $ 屏蔽��5/   (   # [  51   (    50   (   �� $ Disable�51   (     52      $
 Auto 6(   $ 	灵敏度��53   (   # [  55   (    54   (   �� $ SensitiveLevel�55   (     56      $
 Auto 6(   $ 
输出组1��57   (   # [  59   (    58   (   �� $ LinkageGroup1�59   (     5:      $
 Auto 6(   $ 
输出组2��5;   (   # [  5=   (    5<   (   �� $ LinkageGroup2�5=   (     5>      $
 Auto 6(   $ 
输出组3��5?   (   # [  5A   (    5@   (   �� $ LinkageGroup3�5A   (     5C      $
 Auto 6(   $ 延时��5D   (   # [  5F   (    5E   (   �� $ 
DelayValue�5F   (     5H      $
 Auto 6(   $ 	板卡号��5I   (   # [  5K   (    5J   (   �� $ BoardNo�5K   (     5L      $
 Auto 6(   $ 	手盘号��5M   (   # [  5O   (    5N   (   �� $ 
SubBoardNo�5O   (     5P      $
 Auto 6(   $ 	手键号��5Q   (   # [  5S   (    5R   (   �� $ KeyNo�5S   (     5T      $
 Auto 6(   $ 广播分区��5U   (   # [  5W   (    5V   (   �� $ BroadcastZone�5W   (     5Y      $	 120 6(   $ 楼号��5Z   (   # [  5\   (    5[   (   �� $ 
BuildingNo�5\   (     5]      $	 120 6(   $ 区号��5^   (   # [  5`   (    5_   (   �� $ 
BuildingNo�5`   (     5a      $	 120 6(   $ 层号��5b   (   # [  5d   (    5c   (   �� $ ZoneNo�5d   (     5e      $	 120 6(   $ 	房间号��5f   (   # [  5h   (    5g   (   �� $ RoomNo�5h   (     5i      $	 120 6(   $ 安装地点��5j   (   # [  5l   (    5k   (   �� $ Location�5l   (   5n      5o      5p      