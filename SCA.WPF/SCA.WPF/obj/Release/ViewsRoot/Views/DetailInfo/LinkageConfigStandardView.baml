   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullk:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo,SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo  ?$clr-namespace:SCA.WPF.InfrastructureSCA.WPF.Infrastructure  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 1&clr-namespace:System;assembly=mscorlibSystem B    <SCA.WPF.ViewsRoot.Views.DetailInfo.LinkageConfigStandardView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Llocal:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo     ?infrastructure$clr-namespace:SCA.WPF.Infrastructure     6dg-http://schemas.microsoft.com/wpf/2008/toolkit  <col2clr-namespace:System.Collections;assembly=mscorlib  0sys&clr-namespace:System;assembly=mscorlib  .���   � 	Resources     tb_ElementStyle �� 
TargetType �� Property �� Value��%0   &	        �� " ��.���  � ��$ Right=�.+��� .+�� $��0,5,0,0q�$
��Left=�$��Stretch=�$	��300�� � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+�� $��Stretch=�$��Stretch=� �� $
��Auto � �� $
��Auto �.+��� $��45��$��0��$��0��$
��Left=�.@��� .��添加更多项�� -   $��btnAddNewLine��
������ 
������ $
��Left=�$��0��$��0��$��30��$��30������  �� Path$ AddNewRecordCommand����� $ AddedAmount�.���� ���� $ AddIconPath� �� Mode$ OneWay=��� -   $��	btnDelete��$��1��$��0��$��30��$��30��
������ 
������ .���� ���� $ DelIconPath�$ OneWay=��� -   $��btnCopy��$��2��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Copy������  �� ElementName$ DataGrid_Standard��.���� ���� $ CopyIconPath�$ OneWay=��� -   $��btnPaste��$��3��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Paste������ $ DataGrid_Standard��.���� ���� $ PasteIconPath�$ OneWay=��� $��4��$��0��$��30��$��30��
������ 
������ .���� ���� $ SaveIconPath�$ OneWay=��� $��5��$��0��$��30��$��30��
������ 
������ ���� $ DownloadCommand�.���� ���� $ DownloadIconPath�$ OneWay=��� $��6��$��0��$��30��$��30��
������ 
������ .���� ���� $ UploadIconPath�$ OneWay=�H   BSCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DetailInfoGridControl  -   	  Name$	 DataGrid_Standard��)  #Microsoft.Windows.Controls.DataGrid
   AutoGenerateColumns
 .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�
������    RowHeaderWidth$ 25Q�   	RowHeight$ 40Q�   RowBackground
 �����   AlternatingRowBackground
 ����� �� AlternationCount$ 2��   SelectionUnit$ CellOrRowHeader=����� $3 -StandardLinkageConfigInfoObservableCollection�  )   #SCA.WPF.Infrastructure.BindingProxy 	 proxy   Data  
errorStyle �� Triggers P� Property P� Value �� RelativeSource��%�   &	       &	         �� �� " �� P�  �@=�HasError . . ���  � ��
 �  ����  �@�ToolTip ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�   Columns 3  -Microsoft.Windows.Controls.DataGridTextColumn  /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto    Header$ ID��4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [     Binding �� $ ID�  $	 120 $ 输出组号��# [     EditingElementStyle# [  �� $
 Code� �� ValidatesOnDataErrors .   $
 Auto $ 联动模块1��# [   �� $ 	DeviceNo1�  $
 Auto $ 联动模块2��# [   �� $ 	DeviceNo2�  $
 Auto $ 联动模块3��# [   �� $ 	DeviceNo3�  $
 Auto $ 联动模块4��# [   �� $ 	DeviceNo4�  $
 Auto $ 联动模块5��# [   �� $ 	DeviceNo5�   
Visibility �� *Data.IsVisualColumn+ �� Source# [   $
 Auto $ 联动模块6��# [   �� $ 	DeviceNo6� �� *Data.IsVisualColumn+# [   $
 Auto $ 联动模块7��# [   �� $ 	DeviceNo7� �� *Data.IsVisualColumn+# [   $
 Auto $ 联动模块8��# [   �� *Data.IsVisualColumn+# [  �� $ 	DeviceNo8�7  1Microsoft.Windows.Controls.DataGridComboBoxColumn  $ 动作常数��    SelectedItemBinding  �� $ ActionCoefficient�!   ItemsSource! "	  System.Collections.ArrayList	  ��0����1����2����3����4����5��  $
 Auto $ 
联动组1��# [  # [  �� $ 
LinkageNo1� .   $
 Auto $ 
联动组2��# [  # [  �� $ 
LinkageNo2� .   $ * $ 
联动组3��# [  # [  �� $ 
LinkageNo3� . 