   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullk:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo,SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 1&clr-namespace:System;assembly=mscorlibSystem ?    9SCA.WPF.ViewsRoot.Views.DetailInfo.LinkageConfigMixedView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Llocal:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo     Bcol2clr-namespace:System.Collections;assembly=mscorlib     6sys&clr-namespace:System;assembly=mscorlib     6dg-http://schemas.microsoft.com/wpf/2008/toolkit  .���   � 	Resources     tb_ElementStyle �� 
TargetType �� Property �� Value  LinkageActionTypeEnumDS S� 
ObjectTypeN   HSCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigMixedViewModel S� 
MethodName  LinkageTypeEnumDS  TypesCollection �� Source��%�   &	        &	 &     &	 K     &	 j     �� " ��.���  � ��$ Right=�S� "  $ GetLinkageActionType��S� "  $ GetLinkageType����  �� *ValidDeviceType+.+��� .+�� $��0,5,0,0q�$
��Left=�$��Stretch=�$	��300�� � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+�� $��Stretch=�$��Stretch=� �� $
��Auto � �� $
��Auto �.+��� $��45��$��0��$��0��$
��Left=�.@��� .��添加更多项�� -   $��btnAddNewLine��
������ 
������ $
��Left=�$��0��$��0��$��30��$��30������ 	 �� Path$	 AddNewRecordCommand����� $	 AddedAmount�.���� ���� $	 AddIconPath�
 �� Mode$
 OneWay=��� -   $��	btnDelete��$��1��$��0��$��30��$��30��
������ 
������ .���� ���� $	 DelIconPath�$
 OneWay=��� -   $��btnCopy��$��2��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Copy������  �� ElementName$ DataGrid_Mixed��.���� ���� $	 CopyIconPath�$
 OneWay=��� -   $��btnPaste��$��3��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Paste������ $ DataGrid_Mixed��.���� ���� $	 PasteIconPath�$
 OneWay=��� $��4��$��0��$��30��$��30��
������ 
������ .���� ���� $	 SaveIconPath�$
 OneWay=��� $��5��$��0��$��30��$��30��
������ 
������ ���� $	 DownloadCommand�.���� ���� $	 DownloadIconPath�$
 OneWay=��� $��6��$��0��$��30��$��30��
������ 
������ .���� ���� $	 UploadIconPath�$
 OneWay=�H   BSCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DetailInfoGridControl  -     Name$ DataGrid_Mixed��)  #Microsoft.Windows.Controls.DataGrid   AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�
������    RowHeaderWidth$ 25Q�   	RowHeight$ 40Q�   RowBackground
 �����   AlternatingRowBackground
 ����� �� AlternationCount$ 2��   SelectionUnit$ CellOrRowHeader=����� $0	 *MixedLinkageConfigInfoObservableCollection�    
errorStyle �� Triggers P� Property P� Value �� RelativeSource��%�   &	       �� " �� P�  �@=�HasError . . ���  � ��
 �  ����  �@�ToolTip ��  �� *Self+$)	 #(Validation.Errors)[0].ErrorContent�   Columns 3  -Microsoft.Windows.Controls.DataGridTextColumn  /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto    Header$ ID��4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [     Binding �� $	 ID�  $	 120 $ 编号��# [     EditingElementStyle# [  �� $
	 Code� �� ValidatesOnDataErrors . 7  1Microsoft.Windows.Controls.DataGridComboBoxColumn  $ 动作常数��    SelectedItemBinding  �� $	 ActionCoefficient�!   ItemsSource! "	  System.Collections.ArrayList	  ��0����1����2����3����4����5��  $ 动作类型��! �� " �� Source#" [ #   SelectedValueBinding# �� $	 
ActionType�$
 TwoWay=�  $ A分类��! �� #" [ # �� $	 TypeA�$
 TwoWay=�  $
 Auto $
 A楼��# [  # [  �� $	 BuildingNoA� .   $
 Auto $
 A区��# [  # [  �� $	 ZoneNoA� .   $
 Auto $
 A层��# [  # [  �� $	 LayerNoA� .   $
 Auto $ A路号��# [  # [  �� $	 LoopNoA� .   $
 Auto $ A编号��# [  # [  �� $	 DeviceCodeA� .   $ A类型��$   DisplayMemberPath$
$ Name��%   SelectedValuePath$
% Code��# �� *DeviceTypeCodeA+! �� #" [   $ B分类��! �� #" [ # �� $	 TypeB�$
 TwoWay=�  $
 Auto $
 B楼��# [  # [  �� $	 BuildingNoB� .   $
 Auto $
 B区��# [  # [  �� $	 ZoneNoB� .   $
 Auto $
 B层��# [  # [  �� $	 LayerNoB� .   $
 Auto $ B路号��# [  # [  �� $	 LoopNoB� .   $
 Auto $ B编号��# [  # [  �� $	 DeviceCodeB� .   $ B类型��$
$ Name��$
% Code��# �� $	 DeviceTypeCodeB�! �� #" [   $ C分类��! �� #" [ # �� $	 TypeC�$
 TwoWay=�  $
 Auto $ C楼号��# [  # [  �� $	 BuildingNoC� .   $
 Auto $ C区号��# [  # [  �� $	 ZoneNoC� .   $
 Auto $ C层号��# [  # [  �� $	 LayerNoC� .   $
 Auto $ C机号��# [   �� $	 
MachineNoC�  $
 Auto $ 
C回路号��# [  # [  �� $	 LoopNoC� .   $
 Auto $ C编号��# [  # [  �� $	 DeviceCodeC� .   $ C类型��$
$ Name��$
% Code��# �� $	 DeviceTypeCodeC�! �� #" [ 