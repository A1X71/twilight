   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullk:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo,SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 1&clr-namespace:System;assembly=mscorlibSystem U/clr-namespace:SCA.WPF.ViewModelsRoot.Validation!SCA.WPF.ViewModelsRoot.Validation  ;    5SCA.WPF.ViewsRoot.Views.DetailInfo.DeviceInfo8007View   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Llocal:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo     6dg-http://schemas.microsoft.com/wpf/2008/toolkit  <col2clr-namespace:System.Collections;assembly=mscorlib  0sys&clr-namespace:System;assembly=mscorlib  @
validation/clr-namespace:SCA.WPF.ViewModelsRoot.Validation    � 	Resources     TypesCollection �� Source��%,   &	        ��  �� *ValidDeviceType+.���     tb_ElementStyle �� 
TargetType �� Property �� Value��%0   &	       �� " ��.���  � ��$ Right=�.+��� .+�� $��0,5,0,0q�$
��Left=�$��Stretch=�$	��300�� � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+�� $��Stretch=�$��Stretch=� �� $
��Auto � �� $
��Auto �.+��� $��45��$��0��$��0��$
��Left=�.@��� .��添加更多项�� -   $��btnAddNewLine��
������ 
������ $
��Left=�$��0��$��0��$��30��$��30������ *AddNewRecordCommand+����  �� Path$ AddedAmount�.���� ���� $ AddIconPath� �� Mode$ OneWay=��� -   $��	btnDelete��$��1��$��0��$��30��$��30��
������ 
������ .���� ���� $ DelIconPath�$ OneWay=��� -   $��btnCopy��$��2��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Copy������ 	 �� ElementName$	 DataGrid_Device��.���� ���� $ CopyIconPath�$ OneWay=��� -   $��btnPaste��$��3��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Paste������ $	 DataGrid_Device��.���� ���� $ PasteIconPath�$ OneWay=��� $��4��$��0��$��30��$��30��
������ 
������ .���� ���� $ SaveIconPath�$ OneWay=��� $��5��$��0��$��30��$��30��
������ 
������ ���� $ DownloadCommand�.���� ���� $ DownloadIconPath�$ OneWay=��� $��6��$��0��$��30��$��30��
������ 
������ .���� ���� $ UploadIconPath�$ OneWay=�H   BSCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DetailInfoGridControl  -   
  Name$
 DataGrid_Device��)  #Microsoft.Windows.Controls.DataGrid   AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�
������    RowHeaderWidth$ 25Q�   	RowHeight$ 40Q�   RowBackground
 �����   AlternatingRowBackground
 ����� �� AlternationCount$ 2��   SelectionUnit$ CellOrRowHeader=����� $$ DeviceInfoObservableCollection�    
errorStyle �� Triggers P� Property P� Value �� RelativeSource��%�   &	       �� " �� P�  �@=�HasError . . ���  � ��
 �  ����  �@�ToolTip ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�!   RowValidationErrorTemplate �� .R�� $��	0,-2,0,-2q� � ToolTip$ 此行有错误��.+�A� $��0��
���  ��#��z��#��z���� $��!��#��z��$
��Bold"�
�������$��Center=�   Columns 3  -Microsoft.Windows.Controls.DataGridTextColumn  /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto    Header$ ID��4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [    Binding �� $ ID�  $	 120 $ 编码��# [  �� $
 Code�7  1Microsoft.Windows.Controls.DataGridComboBoxColumn  $ 器件类型��   DisplayMemberPath$
 Name��   SelectedValuePath$
 Code��    SelectedValueBinding  �� *
TypeCode+!   ItemsSource! �� " �� Source#" [    $
 Auto $ 特性��# [  �� $ Feature�7  1Microsoft.Windows.Controls.DataGridCheckBoxColumn  $ 屏蔽��$
 Auto  �� $ Disable�# �� ValidatesOnDataErrors# .   $ 	灵敏度��$   SelectedItemBinding$ �� $ SensitiveLevel�! "	  System.Collections.ArrayList	  ��1����2����3��  $ 
输出组1��%   EditingElementStyle#% [ # [  �� $ LinkageGroup1�# .   $ 
输出组2��#% [ # [  �� $ LinkageGroup2�# .   $ 楼号��#% [ # [  �� $ 
BuildingNo�# .   $ 区号��#% [ # [  �� $ ZoneNo�# .   $ 层号��#% [ # [  �� $ FloorNo�# .   $ 	房间号��#% [ # [  �� $ RoomNo�# .   $ * $ 安装地点��#% [ # [  �� $ Location�# . 