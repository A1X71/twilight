   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SCA.WPF.CreateControllerSCA.WPF.CreateController  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem C&clr-namespace:SCA.WPF.CreateControllerSCA.WPF.CreateController  2    ,SCA.WPF.ViewsRoot.Views.CreateControllerView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5VM&clr-namespace:SCA.WPF.CreateController     6sys&clr-namespace:System;assembly=mscorlib     8local&clr-namespace:SCA.WPF.CreateController       � 	Resources     ControllerTypeEnumDS S� 
ObjectType8   2SCA.WPF.CreateController.CreateControllerViewModel S� 
MethodName  SerialPortNumberEnumDS  createControllerVM 	 Label �� 
TargetType �� Property �� Value��%�   &	        &	 "     &	 F     &	 K     S� "  $ GetControllerType��S� "  $ GetSerialPortNumber��  �� " ��.���  � ��
 �   � � DataContext ��  �� Source# [ .����   WindowBackgroundBrush#��[ .+��� -    ��Name$ Header��  BackgroundBrush#���    BorderBrush#���  $��1,1,1,0q�	 �� CornerRadius$	 5,5,0,0��._��� -   
 ��Name$
 HeaderTextBlock��$��创建控制器��$
��Wrap=�$��	15,5,10,5q�  HeaderFontColor#���  $��18(�$��Center=�$��Center=�$
��Bold"�� -    �Name$ AddControllerGrid��$��-1,0,1,0q� � ColumnDefinitions �� $��4* ��� $��6* � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��_控制器名称��#��[ �� -    ��Name$  ControllerNameInputTextBox��$��0��$��1�� �� TextWrapping$
 Wrap=�$
��Left=�$	��180��$	��100��  InputFontColor#���  �� $��1��$��0��$��_控制器类型��#��[ �� $��0,1,0,0q�$��1��$��1��._�� $��
10,5,10,10q�$��Stretch=�$��Stretch=� �� $��20 ��� $	��100 � �� $
��Auto ��� $
��Auto �.+��� -    ��Name$ ControllerTypeComboBox��$ �� IsSynchronizedWithCurrentItem$ FalseY�$	��120��$��1��$
��Left=�$��2��$��0,0,0,-0.163q����� # [  �� $��2��$��0��$��_控制器机号��#��[  �� Target ��  �� ElementName$&  ControllerMachineNumInputTextBox�� �� Mode$ OneWay=��� -   $&  ControllerMachineNumInputTextBox��$��2��$��1��$
 Wrap=�$
��Left=�$	��180��#���  $	��100���� $��3��$��0��$��_器件长度��#��[ �� -   $ DeviceCodeLengthComboBox��$	��120��$��3��$��1��$
��Left=�$��5,0,0,0q�$ FalseY�����  �� Path$ DeviceCodeLength��� $��4��$��0��$��
_串口号��#��[ �� $��0,1,0,0q�$��4��$��1��._�� $��
10,5,10,10q�$��Stretch=�$��Stretch=� �� $
��Auto � �� $
��Auto ��� $
��Auto �.+��� -   $ SerialPortNumberComboBox��$ FalseY�$	��120��$��1��$
��Left=�$��-4,0,0,0q����� # [ �� $��0,1,0,0q�._�� $��
10,5,10,10q�$��Stretch=�$��Stretch=� �� $
��Auto � �� $
��Auto ��� $
��Auto ��� $��5��$��2��$��15,15,15,15q� �� Orientation$ 
Horizontal=�$��Center=�$��Stretch=�.+��� -	    ��Name$ 	AddButton��$��_确定��$��Bottom=� �� 	IsDefault . $
��Left=�$	��140��$��64���� -
   $ CloseButton��$��_取消��$��Bottom=� �� IsCancel . $
��Left=�$	��150��$��10,0,0,0q�$��60���� -   $ Footer��#���  $��35��#���  $��1,0,1,1q�$	 0,0,5,5��