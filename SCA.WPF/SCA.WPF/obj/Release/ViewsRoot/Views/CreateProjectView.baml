   M S B A M L   `   `   `  ����   SCA.WPF/    )SCA.WPF.ViewsRoot.Views.CreateProjectView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .���� .+��� -     ��Name$  Header��$��1,1,1,0q� �� CornerRadius$ 5,5,0,0��._��� -    ��Name$ HeaderTextBlock��$��创建工程��$
��Wrap=�$��	15,5,10,5q�
���   �$��18(�$��Center=�$��Center=�$
��Bold"��� $��1,1,1,1q�._�� -    �Name$ ContentGrid��$��0,0,0,0q� � ColumnDefinitions �� $��* � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��1��$��10,0,0,0q� �� $	��29* ��� $	��83* ��� $	��14* � �� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��_工程名称��
���   � �� Target ��  �� ElementName$ ProjectNameInputTextBox�� �� Mode$ OneWay=��� -   	 ��Name$	 ProjectNameInputTextBox��$��0��$��1��
 �� TextWrapping$

 Wrap=�$
��Left=�$	��180��$��5,0,0,0q�$	��100���� $��1��$��0��$��文件存储路径��
���   � �� $ FilePathInputTextBox��$ OneWay=��� -   $	 FilePathInputTextBox��$��1��$��1��$

 Wrap=�$
��Left=�$	��180��$��5,0,0,0q�$	��120���� -    ��Name$ SelectFolderButton��$��1��$��3��.��...�� $��2��$��3��$��15,15,15,15q� �� Orientation$ 
Horizontal=�$��Center=�$��Stretch=�.+��� -   $ 	AddButton��$��_确定��$��Bottom=� �� 	IsDefault . $
��Left=�$	��140��$��64���� -   $ CloseButton��$��_取消��$��Bottom=� �� IsCancel . $
��Left=�$	��150��$��10,0,0,0q�$��60���� -	   $  Footer��$��35��$��1,0,1,1q�$ 0,0,5,5��