   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Test.WPF.Navigator.ViewModelTest.WPF.Navigator.ViewModel  5      )    #Test.WPF.Navigator.View.UCNavigator   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A
ViewModels*clr-namespace:Test.WPF.Navigator.ViewModel     5      .��5	      �   � 	Resources  5   
   @   :Test.WPF.Navigator.ViewModel.HierarchyDataTemplateSelector !  hierarchyDataTemplateSelector��%   &	          5      67   5       � RowDefinitions 5   
   �� 5      $	��300 �6   6+   �� 5      $��* �6   6)   .+�5   
   R� -   5        $��HierarchyTreeView��5      #��[  6/   ��6u   �� *
Projects+ � DataContext 5      5   /Test.WPF.Navigator.ViewModel.HierarchyViewModel  5      ��5      �� 5       �� 
TargetType" Q�6   .�5      ��  �� Property �@Q�
IsExpanded �� Value 63   �� *
IsExpanded+�� 5       �@Q�
IsSelected 63   �� *
IsSelected+�� 5       � ��$ Normal"�63    �� Triggers 5      P� 5        P� Property �@Q�
IsSelected P� Value . . �5!      ��  � ��$
 Bold"�6;   5"      5$        5'        ProjectTemplate	 �� DataType9   3Test.WPF.Navigator.ViewModel.HierarchyItemViewModel
 �� ItemsSource �� Path �� Orientation J� InputBindings d� MouseAction �� Command �� RelativeSource �� AncestorType  ControllerTemplate  ControllerNodeTemplate �� CommandParameter  LoopTemplate��%�  &	       &	 �    &	 �    &	 0    �� 5(      "	  6C   
 6y   �� $ Children�.R�5)      �� $ 
Horizontal=�6!   .+�5*      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 5+      $��0 0 5 0q�6H   ��6$   �� $ DataItem.Name� 5,      d� 5-   "   $ 	LeftClicke�6�    6/   ��  �� *FindAncestor+" �$! NavigateToDeviceInfoCommand�6�   5/      �� 50      ��6$   �� $ DataItem.ID�51      52      �� 53      "	  6F   
 6|   �� $ Children�.R�54      �� $ 
Horizontal=�6!   .+�55      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 56      ��6$   �� $ DataItem.Name�57      58      �� 59      "	  6J   
 6�   �� $ Children�.R�5:      �� $ 
Horizontal=�6!   .+�5;      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 5=      ��6$   �� $ DataItem.Name� 5>      d� 5@   "   $ 	LeftClicke�5B   /    5@   /   �� *)'DataContext.NavigateToDetailInfoCommand+ �� " � 5A   /   �� 5B   /   5E      5F      5G      �� 5H      "	  6@   .R�5I      �� $ 
Horizontal=�6!   .+�5J      �� $��15��6L   ��6    �� $ DataItem.IconInTree�6Z   �� 5K      ��6$   �� $ DataItem.Code� 5L      d� 5N   "   $ 	LeftClicke�6�    6/   �� *)'DataContext.NavigateToDeviceInfoCommand+ �� " � 6�   �� $ DataItem�6�   5P      5Q      5R      5S      5T      5U      5V      