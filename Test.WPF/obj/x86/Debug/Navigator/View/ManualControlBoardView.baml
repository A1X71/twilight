   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      4    .Test.WPF.Navigator.View.ManualControlBoardView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
      �   � 	Resources  5   
      tb_ElementStyle �� 
TargetType �� Property �� Value��%U   &	        �� 5      " ��6,   .�5      ��  � ��$ Right=�68   5      5      .+�5   
   &    Test.WPF.DataGrid.DeviceDataGrid  -   5          Name$ DataGrid_ManualBoard��5      )  #Microsoft.Windows.Controls.DataGrid   AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�5      
������    RowHeaderWidth$ 25Q�5         	RowHeight$ 40Q�5      	   RowBackground
	 �����
   AlternatingRowBackground

 ����� �� AlternationCount$ 2��5         SelectionUnit$ CellOrRowHeader=�5      $��
0,0,-297,0q�5   .   ��6   ��    Columns 5      3  -Microsoft.Windows.Controls.DataGridTextColumn  5       /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$	 120 6(      Header$ 编号��5!   (   4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [  5#   (      Binding 5"   (   ��  �� Path$
 Code�5$        5%      $
 Auto 6(   $ 	板卡号��5&   (   # [  5(   (    5'   (   �� $ BoardNo�5(   (     5)      $
 Auto 6(   $ 	手盘号��5*   (   # [  5,   (    5+   (   �� $ 
SubBoardNo�5,   (     5-      $
 Auto 6(   $ 	手键号��5.   (   # [  50   (    5/   (   �� $ KeyNo�50   (     51      $
 Auto 6(   $ 	地编号��52   (   # [  54   (    53   (   �� $ 
DeviceCode�54   (   57      58      59      