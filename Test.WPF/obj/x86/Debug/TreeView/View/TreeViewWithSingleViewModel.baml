   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Test.WPF.TreeView.ViewModelTest.WPF.TreeView.ViewModel  5   	   8    2Test.WPF.TreeView.View.TreeViewWithSingleViewModel   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @
ViewModels)clr-namespace:Test.WPF.TreeView.ViewModel     5   	     � Title$!  TreeViewWithSingleViewModel��5   	   $	��300��6-   $	��300��6:    � 	Resources 5      ?   9Test.WPF.TreeView.ViewModel.HierarchyDataTemplateSelector !  hierarchyDataTemplateSelector��%   &	          5   
   63   5      .��5	      � .+�5
   
   � .+�5      R� -   5        $��HierarchyTreeView��5      #��[  5       � DataContext 5   0   ��  �� Path$ HierarchyViewModel���5      �� *	Customers+��5      �� 5       �� 
TargetType" Q�6   .�5      ��  �� Property �@Q�
IsExpanded �� Value 67   �� *
IsExpanded+�� 5       �@Q�
IsSelected 67   �� *
IsSelected+�� 5       � ��$ Normal"�67    �� Triggers 5      P� 5       P� Property �@Q�
IsSelected	 P� Value	 . . �5   "   ��  � ��$
 Bold"�6?   5      5       5        CustomerTemplate
 �� DataType8   2Test.WPF.TreeView.ViewModel.HierarchyItemViewModel �� ItemsSource �� Orientation  OrderTemplate  ProductTemplate��%*  &	       &	 �     &	 �    �� 5      "
  6H    6~   �� $ Children�.R�5      �� $ 
Horizontal=�6%   .+�5      �� $��0 0 5 0q�6L   ��6(   �� $ DataItem.Name�6L   �� 5      ��6(   �� $ DataItem.ID�5      5       �� 5!      "
  6E    6{   �� $ Children�.R�5"   &   �� $ 
Horizontal=�61   .+�5#   *   �� ��64   �� $ DataItem.Code�5$   '   5%      �� 5&      "
  6G   .R�5'   &   �� $ 
Horizontal=�61   .+�5(   *   �� ��64   �� $ DataItem.Name�5)   '   5*      5+      5,      5-      5.      5/      