   M S B A M L   `   `   `  ����   SCA.WPF3    -SCA.WPF.ViewsRoot.Views.Query.LoopSummaryView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources   	  Label �� 
TargetType �� Property �� Value��%/   &	        �� " ��.���  � ��
 �   �.���� .+��  � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+��� $��0��$��0��$��30��$��30��
������ 
������ ���� *SaveProjectInfoCommand+.����  � ToolTip$ 保存������  �� Path$ SaveIconPath� �� Mode$ OneWay=��� $��1��$��0��$��30��$��30��
������ 
������ ���� $ DownloadCommand�.���� $ 下传������ $ DownloadIconPath�$ OneWay=��� $��2��$��0��$��30��$��30��
������ 
������ ���� $ UploadCommand�.���� $ 上传������ $ UploadIconPath�$ OneWay=��  �� $��80 ��� $	��100 ��� $��80 ��� $	��100 ��� $��80 ��� $	��100 � �� $
��Auto �.+��� $��0��$��0��#��[  .��回路号：�� -   	 ��Name$	 LoopCodeComboBox��$��0��$��1��$
 �� IsSynchronizedWithCurrentItem$
 FalseY�$
��Left=����� *	LoopsCode+�� $��0��$��2��#��[  .��设备类型：�� -   $	 DeviceTypeComboBox��$��0��$��3��$
 FalseY�$
��Left=����� *DeviceTypes+�� $��0��$��4��#��[  .��设备编号：�� $��0��$��5���� #��[  .��53此处显示此项目回路下的所有器件信息