   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=null?$clr-namespace:SCA.WPF.ProjectSettingSCA.WPF.ProjectSetting  0    *SCA.WPF.ViewsRoot.Views.ProjectSettingView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3vm$clr-namespace:SCA.WPF.ProjectSetting       � 	Resources     ControllerNameEnumDS S� 
ObjectType4   .SCA.WPF.ProjectSetting.ProjectSettingViewModel S� 
MethodName 	 Label �� 
TargetType �� Property �� Value  projectSettingVM��%�   &	        &	      &	 ]     S� "  $ GetControllers���� " ��.���  � ��
 �   ���  � ��$ Right=�   � DataContext ��  �� Source# [ .����   WindowBackgroundBrush#��[ .+��� -    ��Name$ Header��  BackgroundBrush#���    BorderBrush#���  $��1,1,1,0q�	 �� CornerRadius$	 5,5,0,0��._��� -   
 ��Name$
 HeaderTextBlock��$��项目设置��$
��Wrap=�$��	15,5,10,5q�  HeaderFontColor#���  $��18(�$��Center=�$��Center=�$
��Bold"�� -    �Name$ AddControllerGrid��$��0,0,0,0q� � ColumnDefinitions �� $��4* ��� $��6* � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$!��_自动保存时间(分钟)��#��[  �� Target ��  �� ElementName$&  AutoSaveTimeIntervalInputTextBox�� �� Mode$ OneWay=��� -    ��Name$&  AutoSaveTimeIntervalInputTextBox��$��0��$��1�� �� TextWrapping$
 Wrap=�$
��Left=�$	��180��$��5,0,0,0q�$	��100������  �� Path$ SaveInterval��� $��1��$��0��$��_主控制器名称��#��[  �� $ ControllerNameComboBox��$ OneWay=��� -    ��Name$ ControllerNameComboBox��$��1��$��1��$ �� IsSynchronizedWithCurrentItem$
 TrueY� �� SelectedValuePath$	 Key�� �� DisplayMemberPath$ Value��$	��120��$
��Left=�$��5,0,0,0q� �� SelectedValue �� $ 
SelectedID�# [ $ OneWay=����� # [  �� $��2��$��2��$��15,15,15,15q� �� Orientation$ 
Horizontal=�$��Center=�$��Stretch=�.+��� -    ��Name$ 	AddButton��$��_确定��$��Bottom=� �� 	IsDefault . $
��Left=�$	��140��$��64���� -   $ CloseButton��$��_取消��$��Bottom=� �� IsCancel . $
��Left=�$	��150��$��10,0,0,0q�$��60���� -   $ Footer��#���  $��35��#���  $��1,0,1,1q�$	 0,0,5,5��