   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Test.WPF.DataGridTest.WPF.DataGrid  5   	   '    !Test.WPF.DataGrid.OperateWithDrop   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1localclr-namespace:Test.WPF.DataGrid     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	     � Title$  OperateWithDrop��5   	   $	��300��6!   $	��300��6.    � 	Resources 5      )   #Test.WPF.DataGrid.ProductCollection   ProductList��%   &	          5   
   6"   5	      .��5
      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��productsDataGrid��5   %   $��0,28,0,0q�6      AutoGenerateColumns .     SelectionMode$ Extended=�5         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6,    J� 	AllowDrop .  � DataContext 5   =   ��  �� Source# [  ��5   /   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 	ProductId��6C   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6%   �� *	ProductId+6X     5      $	 ProductName��6E   
 6%   �� *ProductName+6\     5      $	 ProductPrice��6F   
 6%   �� *ProductPrice+6^   5      �� 5   
   $
��Wrap=�6   $'��!DataGrid Row Drag And Drop Sample��6(   $	��Top=�6Q   $��3,1,0,0q�6i   $��24��6z   $
��Left=�6�   $	��268��6�   $��14.667(�6�   $
��Bold"�6�   $��Italic$�6�   5      5      