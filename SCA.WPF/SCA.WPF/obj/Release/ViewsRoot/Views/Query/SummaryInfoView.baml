   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=null1clr-namespace:SCA.WPF.UtilitySCA.WPF.Utility  3    -SCA.WPF.ViewsRoot.Views.Query.SummaryInfoView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     8eventToCommandclr-namespace:SCA.WPF.Utility     c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity    � 	Resources   	  Label �� 
TargetType �� Property �� Value��%/   &	        �� " ��.���  � ��
 �   �.���� $��
0,0,-472,0q�.+��  � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+��� $��0��$��0��$��30��$��30��
������ 
������ .����  � ToolTip$ 保存������  �� Path$ SaveIconPath� �� Mode$ OneWay=��� $��1��$��0��$��30��$��30��
������ 
������ ���� $ DownloadCommand�.���� $ 下传������ $ DownloadIconPath�$ OneWay=��� $��2��$��0��$��30��$��30��
������ 
������ ���� $ UploadCommand�.���� $ 上传������ $ UploadIconPath�$ OneWay=��  �� $��47 ��� $��30 ��� $��5 ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto �.+��� $��0��$��0��#��[  $��3��$��0,0,0,-0.326q�.��控制器名称：�� -   	 ��Name$ 	 ControllerNameInputTextBox��$��3��$��0��$��80��$	��100��$��0,0,0,-0.326q����� $ ControllerName�$ OneWay=��� $��4��$��0��#��[  $��0,0,0,-0.326q�.��控制器机号：�� -   $	 MachineNumberInputTextBox��$��5��$��0��$��80��$	��101��$��0,0,0,-0.326q����� $ ControllerMachineNumber�$ OneWay=��� $��6��$��0��#��[  $��0,0,0,-0.326q�.��串口号：�� -   
 ��Name$
 ComPortComboBox��$��7��$��0��$ �� IsSynchronizedWithCurrentItem$ FalseY�$��80��$
��Left=�$��5,0,0,-0.326q�$	��102�� �� SelectedItem �� $ SelectedComPort�$ OneWay=����� $ ComPorts��� $��8��$��0��#��[  $��0,0,0,-0.326q�.��波特率：�� -   $
 BaudsRateComboBox��$��9��$��0��$ FalseY�$��80��$
��Left=�$	��103��$��5,0,0,-0.326q� �� $ SelectedBaudRate�$ OneWay=����� $ 	BaudsRate��� -   $��btnSave��$��10��$��0��$��50��$	��104��$��0,0,0,-0.326q�.��保存�� -   $��btnScanPort��$��11��$��0��$��70��$	��105��$��0,0,0,-0.326q�.��扫描端口�� .��$"此处显示控制器Summary信息