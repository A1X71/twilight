   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nulli9clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.Navigator+SCA.WPF.ViewModelsRoot.ViewModels.Navigator  ?$clr-namespace:SCA.WPF.InfrastructureSCA.WPF.Infrastructure  5    /SCA.WPF.ViewsRoot.Views.Navigator.NavigatorView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Klocal9clr-namespace:SCA.WPF.ViewModelsRoot.ViewModels.Navigator     c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  9Infrastructure$clr-namespace:SCA.WPF.Infrastructure  .���� .+�� -     �Name$  EmptyContentTool�����	#FF3892C5$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�Visible=� � ColumnDefinitions �� $
��Auto ��� $��32 ��� $��32 ��� $��32 � � RowDefinitions �� $
��Auto �.+��� $��0��$��0��.��NEAT�� -    ��Name$ btnAddProject��$��1��$��0��$��32��$��32��
������ 
������ .���� ����  �� Path$ ProjectAddIconPath� �� Mode$ OneWay=��� -   $ btnOpenProject��$��2��$��0��$��32��$��32��
������ 
������ .���� ���� $ ProjectOpenIconPath�$ OneWay=��� -   $ btnCloseProject��$��3��$��0��$��32��$��32��
������ 
������ .���� ���� $ ProjectDeleteIconPath�$ OneWay=�� -   $  ProjectTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��工程�� -   $' !btnCloseProjectInProjectToolStrip��$��1��$��0��$��32��$��32��
������ 
������ .���� ���� $ ProjectDeleteIconPath�$ OneWay=��� -   $ btnExportToExcel��$��2��$��0��$��32��$��32��
������ 
������ .���� ���� $ ExportExcelIconPath�$ OneWay=��� -   $ btnMerge��$��3��$��0��$��32��$��32��
������ 
������ .���� ���� $ ProjectMergeIconPath�$ OneWay=��� -	   $ btnAddController��$��4��$��0��$��32��$��32��
������ 
������ .���� ���� $ ControllerAddIconPath�$ OneWay=��� -
   $ btnDeviceType��$��5��$��0��$��32��$��32��
������ 
������ .���� ���� $ DeviceTypeIconPath�$ OneWay=�� -   $  ControllerTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto �.+��� -    ��Name$ NodeText��$��0��$��0��.��	控制器�� -   $ btnDeleteController��$��1��$��0��$��32��$��32��
������ 
������ .���� ���� $ ControllerDeleteIconPath�$ OneWay=��� -   $ btnStartController��$��2��$��0��$��32��$��32��
������ 
������ .���� ���� $ ControllerStartIconPath�$ OneWay=��� -   $ btnStopController��$��3��$��0��$��32��$��32��
������ 
������ .���� ���� $ ControllerStopIconPath�$ OneWay=��� -   $ btnSetMasterController��$��4��$��0��$��32��$��32��
������ 
������ .���� ���� $! ControllerSetMasterIconPath�$ OneWay=��� -   $ btnControllerMerge��$��5��$��0��$��32��$��32��
������ 
������ .���� ���� $ ProjectMergeIconPath�$ OneWay=�� -   $  	LoopsTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto ��� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��回路�� -   $ 
btnAddLoop��$��1��$��0��$��32��$��32��
������ 
������ .���� ���� $ LoopAddIconPath�$ OneWay=�� -   $  SingleLoopTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto ��� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��回路�� -   $ 
btnDelLoop��$��1��$��0��$��32��$��32��
������ 
������ .���� ���� $ LoopDeleteIconPath�$ OneWay=�� -   $  StandardLinkageTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��标准组态� -   $  MixedLinkageTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��混合组态� -   $  GeneralLinkageTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��通用组态� -   $  ManualControlBoardTool��$��0,5,0,0q�$
��Left=�$��Stretch=�$	��300��$y�	Collapsed=� �� $
��Auto � �� $
��Auto �.+��� $��0��$��0��.��网络手动盘�  � 	Resources O   ISCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorDataTemplateSelector !  hierarchyDataTemplateSelectorP   JSCA.WPF.ViewModelsRoot.ViewModels.Navigator.PrimaryControllerColorSelector  primaryColorSelector��%   &	        &	           �� $��* �.+�R� -   $��HierarchyTreeView��#��[  ���� *
Projects+����  �� 
TargetType" Q�.��� 	 �� Property	 �@Q�
IsExpanded
 �� Value
 �� *
IsExpanded+�� 	 �@Q�
IsSelected
 �� *
IsSelected+�� 	 � ��$
 Normal"� �� Triggers P�  P� Property �@Q�
IsSelected P� Value . . ��� 	 � ��$

 Bold"�   ProjectTemplate �� DataTypeH   BSCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel �� ItemsSource �� Orientation J� InputBindings d� MouseAction �� Command �� RelativeSource �� AncestorType  ControllerTemplate �� 	Converter  ControllerNodeTemplate ��Name  LoopTemplate��%  &	       &	     7  &	 �    &	 ]    �� "   �� $ Children�.R��� $ 
Horizontal=�.+��� $��15������ $ DataItem.IconInTree��� $��0 0 5 0q����� $ DataItem.Name� d� $ 	LeftClicke� ��  �� *FindAncestor+" �$! NavigateToDeviceInfoCommand��� "   �� $ Children�.R��� $ 
Horizontal=�.+��� $��15������ $ DataItem.IconInTree��� ���� $ DataItem.Name����� $ DataItem.PrimaryFlag�8   �� "   �� $ Children�.R��� $ 	NodeStack��$ 
Horizontal=�.+��� $��15������ $ DataItem.IconInTree��� ���� $ DataItem.Name��� "  .R��� $ 
Horizontal=�.+��� $��15������ $ DataItem.IconInTree��� ���� $ DataItem.Code�