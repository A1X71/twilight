   M S B A M L   `   `   `  ���� B  >SCA.WPF, Version=0.0.0.4, Culture=neutral, PublicKeyToken=nullU/clr-namespace:SCA.WPF.ViewModelsRoot.Converters!SCA.WPF.ViewModelsRoot.Converters  1clr-namespace:SCA.WPF.UtilitySCA.WPF.Utility  6    0SCA.WPF.ViewsRoot.Views.Query.ProjectSummaryView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  ?	converter/clr-namespace:SCA.WPF.ViewModelsRoot.Converters  2eventToCommandclr-namespace:SCA.WPF.Utility    � 	Resources   	  Label �� 
TargetType �� Property �� ValueA   ;SCA.WPF.ViewModelsRoot.Converters.ComboxEmptyItemConverters  comboxEmptyItemConverterP   JSCA.WPF.ViewModelsRoot.Converters.ComboxEmptyItemForSelectedValueConverter , (comboxEmptyItemForSelectedValueConverter;   5SCA.WPF.ViewModelsRoot.Converters.DeviceTypeConverter  deviceTypeConverter��%u   &	        &	 >     &	 C     &	 H     �� " ��.���  � ��
 �   ���  � ��$ Right=�      .���� .+��  � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+��� $��0��$��0��$��30��$��30��
������ 
������ ���� *SaveProjectInfoCommand+.����  � ToolTip$ 保存������  �� Path$ SaveIconPath� �� Mode$ OneWay=��� $��1��$��0��$��30��$��30��
������ 
������ ���� *ToggleSimulatorInfoCommand+.���� $ 模拟联动������ $ SimulatorIconPath�$ OneWay=��  �� $��80 ��� $	��100 � �� $
��Auto �.+��� $��0��$��0��#��[  .��工程名称：�� -   	 ��Name$	 ProjectNameInputTextBox��$��1��$��0������ $ ProjectName�$ OneWay=�� -   
 �Name$
 SummaryInfo��y��� $ SummaryVisibility�.+��� #��[  .��'%此处显示此项目的Summary信息�� -    ��Name$ 	Simulator��y��� $ SimulatorVisibility�.+�� -   $
 	QueryZone�� �� �� �� �� �� ��  �� �� �� .+��� $��0��$��0��$��	控制器��#��[  �� $��0��$��1�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ ID�� �� SelectedValue �� *ControllerID+$ TwoWay=����� $ ValidController��� $��1��$��0��$��回路��#��[  �� $��1��$��1��$
 Name��$ ID��$ �� IsSynchronizedWithCurrentItem$
 trueY� �� *LoopID+$ TwoWay=� �� 	Converter# [ ���� $ 
ValidLoops�# [ �� $��1��$��2��$��楼号��#��[  �� $��1��$��3��$ Value��$	 Key�� �� SelectedIndex$ 0��$
 trueY� �� *
BuildingNo+$ TwoWay=�# [ ���� $ BuildingNoCollection�# [ �� $��1��$��4��$��类型��#��[  �� $��1��$��5��$ 0��$
 trueY�$
 Name��$
 Code�� �� *
TypeCode+$ TwoWay=�# [ ���� $ 
DeviceType�# [ �� $��0��$��2�� �� Orientation$ 
Horizontal=�.+��� $��标准组态�� d� 	IsChecked �� $" StandardLinkageSimulatorFlag��� $��混合组态�� �� $ MixedLinkageSimulatorFlag��� $��通用组态�� �� $! GeneralLinkageSimulatorFlag��� -    ��Name$ btnLinkageSimulator��$��0��$��5��$��模拟联动������ *LinkageSimulatorCommand+�� -    ��Name$ 
QueryGroup��$��选择器件��$��2��$��0��$��6��$��0,10,0,0q�.���� .��&   System.Windows.Controls.DataGrid  -     Name$ QueryDataGrid��   AutoGenerateColumns .  $	��200��   SelectionUnit$ FullRow=����� *%#QueryDeviceInfoObservableCollection+���� )  #System.Windows.Controls.DataGridRow"  .�-      Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ *    
IsReadOnly .    Header$ 序号��1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� $ 
SequenceNo�  $ *  . $ 	地编号�� �� $
 Code�  $ *  . $ 器件类型�� �� $ TypeCode�# [   $ *  . $ 
输出组1�� �� $ LinkageGroup1�  $ *  . $ 
输出组2�� �� $ LinkageGroup2�  $ *  . $ 
输出组3�� �� $ LinkageGroup3�  $ *  . $ 楼号�� �� $ 
BuildingNo�  $ *  . $ 区号�� �� $ ZoneNo�  $ *  . $ 层号�� �� $ FloorNo�� -	   $
 	InputZone�� ��  �� .+��� -
   $ 
InputGroup��$��输入器件��$��0��$��0��$��0,10,0,0q�.���� .��  -   $ InputDataGrid�� .  $	��200��$ FullRow=����� *%#InputDeviceInfoObservableCollection+���� "  .�-      $ *  . $ 序号�� �� $ 
SequenceNo�  $ *  . $ 	地编号�� �� $
 Code�  $ *  . $ 器件类型�� �� $ TypeCode�# [   $ *  . $ 
输出组1�� �� $ LinkageGroup1�  $ *  . $ 
输出组2�� �� $ LinkageGroup2�  $ *  . $ 
输出组3�� �� $ LinkageGroup3�  $ *  . $ 楼号�� �� $ 
BuildingNo�  $ *  . $ 区号�� �� $ ZoneNo�  $ *  . $ 层号�� �� $ FloorNo�� -   $
 
ResultZone�� ��  �� .+��� -   $ LinkageGroup��$��联动器件��$��0��$��0��$��0,10,0,0q�.���� .��  -   $ LinkageDataGrid�� .  $	��200��$ FullRow=����� *'%LinkageDeviceInfoObservableCollection+   $ *  . $ 序号�� �� $ 
SequenceNo�  $ *  . $ 范围说明�� �� $ Scope�  $ *  . $ 控制器名称�� �� $ ControllerName�  $ *  . $ 	地编号�� �� $
 Code�  $ *  . $ 器件类型�� �� $ TypeCode�# [   $ *  . $ 
输出组1�� �� $ LinkageGroup1�  $ *  . $ 
输出组2�� �� $ LinkageGroup2�  $ *  . $ 
输出组3�� �� $ LinkageGroup3�  $ *  . $ 楼号�� �� $ 
BuildingNo�  $ *  . $ 区号�� �� $ ZoneNo�  $ *  . $ 层号�� �� $ FloorNo�