   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Test.WPF.Navigator.ViewModelTest.WPF.Navigator.ViewModel  5   	   +    %Test.WPF.Navigator.View.NavigatorView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   A
ViewModels*clr-namespace:Test.WPF.Navigator.ViewModel     5   	   ^ ZCaliburn.Micro.Platform, Version=3.0.3.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5   	     � Title$  NavigatorView��5   	   $	��300��6   $	��300��6,    � 	Resources 5      @   :Test.WPF.Navigator.ViewModel.HierarchyDataTemplateSelector !  hierarchyDataTemplateSelector��%   &	          5   
   63   5	      .��5
      � 
������  � RowDefinitions 5   
   �� 5      $	��500 �6   6+   �� 5      $��* �6   6)   .+�5   
   R� -   5        $��HierarchyTreeView��5      #��[  6^    � DataContext 6.   ��  �� Path$ HierarchyViewModel���6�   �� *
Projects+��5      �� 5       �� 
TargetType" Q�6   .�5      ��  �� Property �@Q�
IsExpanded �� Value 6/   �� *
IsExpanded+�� 5       �@Q�
IsSelected 6/   �� *
IsSelected+�� 5       � ��$ Normal"�6/    �� Triggers 5      P� 5      	 P� Property	 �@Q�
IsSelected
 P� Value
 . . �5      ��  � ��$
 Bold"�67   5      5       5        ProjectTemplate �� DataType9   3Test.WPF.Navigator.ViewModel.HierarchyItemViewModel �� ItemsSource �� Orientation J� InputBindings d� MouseAction �� Command  ControllerTemplate  ControllerNodeTemplate  LoopTemplate��%�  &	       &	 �    &	 p    &	 Z    �� 5       "  6C    6y   �� $ Children�.R�5!      �� $ 
Horizontal=�6!   .+�5"      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 5#      $��0 0 5 0q�6H   ��6$   �� $ DataItem.Name� 5$      d� 5%   "   $ 	LeftClicke�6O    6/   �� *
GetDetails+6O   5'      �� 5(      ��6$   �� $ DataItem.ID�5)      5*      �� 5+      "  6F    6|   �� $ Children�.R�5,      �� $ 
Horizontal=�6!   .+�5-      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 5.      ��6$   �� $ DataItem.Name�5/      50      �� 51      "  6J    6�   �� $ Children�.R�52      �� $ 
Horizontal=�6!   .+�53      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 54      ��6$   �� $ DataItem.Name�55      56      �� 57      "  6@   .R�58      �� $ 
Horizontal=�6!   .+�59      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 5:      ��6$   �� $ DataItem.Code� 5;      d� 5<   "   $ 	LeftClicke�6O    6/   �� *
GetDetails+6O   5>      5?      5@      5A      5B      �� 5C   
   -   5        $��lstMessages��5C      $��2��6   64   5D      5E      