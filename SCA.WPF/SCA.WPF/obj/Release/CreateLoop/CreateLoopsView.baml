   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=null7 clr-namespace:SCA.WPF.CreateLoopSCA.WPF.CreateLoop  D @SCA.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:SCA.Model;assembly=SCA.Model	SCA.Model -    'SCA.WPF.ViewsRoot.Views.CreateLoopsView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /vm clr-namespace:SCA.WPF.CreateLoop     <model*clr-namespace:SCA.Model;assembly=SCA.Model       � 	Resources   	  Label �� 
TargetType �� Property �� Value��%/   &	        �� " ��.���  � ��
 �   �.����   WindowBackgroundBrush#��[ .+��� -    ��Name$ Header��  BackgroundBrush#���    BorderBrush#���  $��1,1,1,0q� �� CornerRadius$ 5,5,0,0��._��� -    ��Name$ HeaderTextBlock��$��创建回路��$
��Wrap=�$��	15,5,10,5q�  HeaderFontColor#���  $��18(�$��Center=�$��Center=�$
��Bold"�� -    �Name$ AddControllerGrid��$��0,0,0,0q� � ColumnDefinitions �� $��4* ��� $��6* �	 � RowDefinitions	 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��_机号��#��[  
 �� Target
 ��  �� ElementName$ MachineNumberInputLabel�� �� Mode$ OneWay=��� -    ��Name$ MachineNumberInputLabel��$��0��$��1��#��[  ����  �� Path$ ControllerMachineNumber��� $��1��$��0��$��
_回路号��#��[  
 �� $ LoopCodeInputTextBox��$ OneWay=��� -    ��Name$ LoopCodeComboBox��$��1��$��1��$ �� IsSynchronizedWithCurrentItem$ FalseY�$	��120��$
��Left=�$��5,0,0,0q����� *	LoopsCode+�� $��2��$��0��$��
_节点数��#��[  
 �� $ DeviceAmountInputTextBox��$ OneWay=��� -    ��Name$ DeviceAmountInputTextBox��$��2��$��1�� �� TextWrapping$
 Wrap=�$
��Left=�$	��180��$��5,0,0,0q�$	��100���� $��3��$��0��$��
_回路数��#��[  
 �� $ LoopAmountInputTextBox��$ OneWay=��� -   $ LoopAmountInputTextBox��$��3��$��1��$
 Wrap=�$
��Left=�$	��180��$��5,0,0,0q�$	��100���� $��4��$��0��$��
_回路名��#��[  
 �� $ LoopNameInputTextBox��$ OneWay=��� -   $ LoopNameInputTextBox��$��4��$��1��$
 Wrap=�$
��Left=�$	��180��$��5,0,0,0q�$	��100���� $��5��$��2��$��15,15,15,15q� �� Orientation$ 
Horizontal=�$��Center=�$��Stretch=�.+��� -	    ��Name$ 	AddButton��$��_确定��$��Bottom=� �� 	IsDefault . $
��Left=�$	��140��$��64���� -
   $ CloseButton��$��_取消��$��Bottom=� �� IsCancel . $
��Left=�$	��150��$��10,0,0,0q�$��60���� -   $ Footer��#���  $��35��#���  $��1,0,1,1q�$ 0,0,5,5��