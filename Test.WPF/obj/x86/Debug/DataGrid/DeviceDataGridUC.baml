   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 5      1&clr-namespace:System;assembly=mscorlibSystem 5	      B >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullf@clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels;assembly=SCA.WPF!SCA.WPF.ViewModelsRoot.ViewModels 5
      (    "Test.WPF.DataGrid.DeviceDataGridUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Test.WPF.DataGrid     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      <col2clr-namespace:System.Collections;assembly=mscorlib  5      0sys&clr-namespace:System;assembly=mscorlib  5	      Q
deviceData@clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels;assembly=SCA.WPF  5
      .��5      �   � 	Resources  5   
      Test.WPF.DataGrid.People 
  people@  :SCA.WPF.ViewModelsRoot.ViewModels.DeviceInfo8036Collection 
 device(  "System.Windows.Controls.GroupStyle  
gs_Default   PanelA  ;Microsoft.Windows.Controls.Primitives.DataGridRowsPresenter   HeaderTemplate �� Padding �� Path  defaultRowStyle �� 
TargetType,  &Microsoft.Windows.Controls.DataGridRow �� Triggers P� Property P� Value	 �� Property
 �� Value  defaultCellStyle-  'Microsoft.Windows.Controls.DataGridCell �� ElementName �� Mode  defaultRowHeaderStyle=  7Microsoft.Windows.Controls.Primitives.DataGridRowHeader  ColumnHeaderStyle@  :Microsoft.Windows.Controls.Primitives.DataGridColumnHeader  tb_ElementStyle��%�  &	        &	      &	 &     &	 �     &	 �    &	 �    &	 >    &	 5      5      6     5      62     5       5      �� 5      .R�5        5       5      �� 5      .R�5      �� $
��Bold"�6?   $ 3q�6Q   ��6$   �� $
 Name�6Q   5      5      �� 5      "  6,    5      P� 5        �@ 	IsEditing . . �5!      �� 	 � ��

 �  ��68   �� 5"      	 � ��$
 2q�6<   5#      5%      �� 5'      "  6-   .�5(      �� 	 � ��
 5)      �� $ cb_Alignment��$ SelectedItem�$ OneWay=� 5*      P� 5+       �@ 	IsEditing . . �5,      �� 	 � ��

 �  ��68   �� 5-      	 � ��$
 2q�6<   5.      50      �� 52      "  62   .�53      �� 	 � ��
 6,   �� *Id+�� 54      	 � ��
 55      �� $ cb_Alignment��$ SelectedItem�$ OneWay=�56      �� 58      "  6.   .�59      �� 	 � ��
 6?   �� $ cb_Alignment��$ SelectedItem�$ OneWay=��� 5:      	 � ��
 5;      �� 5<      .R�5=      �� .+�5>   "   �� ��6,   �� 5?      5@      5B      5C      �� 5D      " ��6,   .�5E      �� 	 � ��$
 Right=�68   5F      5G      .+�5I   
   &	    Test.WPF.DataGrid.DeviceDataGrid	  -   5         	 Name$ DataGrid_Standard��5I      )
  #Microsoft.Windows.Controls.DataGrid 
  AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�5K      
������  
  RowHeaderWidth$ 25Q�5M       
  	RowHeight$ 40Q�5N       
  RowBackground
 ����� 
  AlternatingRowBackground
 ����� �� AlternationCount$ 2��5Q       
  SelectionUnit$ CellOrRowHeader=�5R       
  RowStyle# [ 5S       
  RowHeaderStyle# [ 5T       
  ColumnHeaderStyle# [ 5U       
  	CellStyle# [ 5V      $��
0,0,-297,0q�5W   M   ��6   ��  �� Source# [  
  Columns 5Y      3  -Microsoft.Windows.Controls.DataGridTextColumn  5Z      /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto 6(      Header$ ID��5[   (   4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 5]   (      Binding 5\   (   �� $ ID�5]   (     5^      $	 120 6(   $
 Code��5_   (   # [ 5a   (    5`   (   �� $
 Code�5b        5g      $	 120 6(   $ 
BuildingNo��5h   (   # [ 5j   (    5i   (   �� $ 
BuildingNo�5j   (   5l      5m      5n      