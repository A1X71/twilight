   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullk:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo,SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo  A%clr-namespace:SCA.WPF.ViewsRoot.ViewsSCA.WPF.ViewsRoot.Views  ?$clr-namespace:SCA.WPF.InfrastructureSCA.WPF.Infrastructure  ?    9SCA.WPF.ViewsRoot.Views.DetailInfo.ManualControlBoardView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Llocal:clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo     6View%clr-namespace:SCA.WPF.ViewsRoot.Views     ?infrastructure$clr-namespace:SCA.WPF.Infrastructure     6dg-http://schemas.microsoft.com/wpf/2008/toolkit  .���   � 	Resources     tb_ElementStyle �� 
TargetType �� Property �� Value)   #SCA.WPF.Infrastructure.BindingProxy 	 proxy   Data��%H   &	        &	 &     �� " ��.���  � ��$ Right=�   �� .+��� .+�� $��0,5,0,0q�$
��Left=�$��Stretch=�$	��300�� � ColumnDefinitions �� $
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
��Left=�.@��� .���� -   $��btnAddMoreLine��$
��Left=�$��0��$��0��$��30��$��30��
������ 
������ .��添加更多项�� -   $��btnAddNewLine��$
��Left=�$��0��$��0��$��30��$��30��
������ 
������ ����  �� Path$ AddNewRecordCommand����� $ AddedAmount�.���� ���� $ AddIconPath� �� Mode$ OneWay=��� -   $��	btnDelete��$��1��$��0��$��30��$��30��
������ 
������ .���� ���� $ DelIconPath�$ OneWay=��� -   $��btnCopy��$��2��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Copy������ 	 �� ElementName$	 DataGrid_ManualBoard��.���� ���� $ CopyIconPath�$ OneWay=��� -   $��btnPaste��$��3��$��0��$��30��$��30��
������ 
������ $��ApplicationCommands.Paste������ $	 DataGrid_ManualBoard��.���� ���� $ PasteIconPath�$ OneWay=��� $��4��$��0��$��30��$��30��
������ 
������ .���� ���� $ SaveIconPath�$ OneWay=��� $��5��$��0��$��30��$��30��
������ 
������ .���� ���� $ DownloadIconPath�$ OneWay=��� $��6��$��0��$��30��$��30��
������ 
������ .���� ���� $ UploadIconPath�$ OneWay=�H   BSCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DetailInfoGridControl  -   
  Name$
 DataGrid_ManualBoard��)  #Microsoft.Windows.Controls.DataGrid   AutoGenerateColumns .  $ �� IsSynchronizedWithCurrentItem$
 TrueY�
������    RowHeaderWidth$ 25Q�   	RowHeight$ 40Q�   RowBackground
 �����   AlternatingRowBackground
 ����� �� AlternationCount$ 2��   SelectionUnit$ CellOrRowHeader=����� $0 *ManualControlBoardInfoObservableCollection�    
errorStyle �� Triggers P� Property P� Value �� RelativeSource��%�   &	       �� " �� P�  �@=�HasError . . ���  � ��
 �  ����  �@�ToolTip ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�   Columns 3  -Microsoft.Windows.Controls.DataGridTextColumn  /  )Microsoft.Windows.Controls.DataGridColumn   Width8  2Microsoft.Windows.Controls.DataGridLengthConverter$
 Auto    Header$ ID��4  .Microsoft.Windows.Controls.DataGridBoundColumn   ElementStyle# [     Binding �� $ ID�  $ * $ 编号��# [     EditingElementStyle# [  �� $
 Code� �� ValidatesOnDataErrors .   $ * $ 	板卡号��# [  # [  �� $ BoardNo� .   $ * $ 	手盘号��# [  # [  �� $ 
SubBoardNo� .   $ * $ 	手键号��# [  # [  �� $ KeyNo� .   $ * $ 	地编号��# [  # [  �� $ 
DeviceCode� . :   4SCA.WPF.ViewsRoot.Views.CreateManualControlBoardView  -     Name$ CreateManualControlBoard��$��Center=�$��Center=� � DataContext �� $ CreateVM�y��� *)'Data.CreateManualControlBoardVisibility+  �� Source#  [ $ TwoWay=�