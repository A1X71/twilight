   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Test.WPFTest.WPF  5   	       Test.WPF.TreeViewTest   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   (localclr-namespace:Test.WPF     5   	     � Title$  TreeViewTest��5   	   $	��300��6   $	��300��6+   .��5      �  � ColumnDefinitions 5   
   �� 5	      �� 5
      .+�5   
   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 6   R� 6
   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     6   $��0��5      ��5      �� *Items+.  (System.Windows.Interactivity.Interaction   	Behaviors 5      0   *Test.WPF.TreeViewSelectedItemBlendBehavior  5         SelectedItem 5      �� *SelectedItem+��5      �� 5       �� 
TargetType" Q�6   .�5      ��  �� Property �@Q�
IsSelected �� Value 63   �� *
IsSelected+5       � 	Resources 5       �� DataType   Test.WPF.Directory	 �� ItemsSource   Test.WPF.File��%�   (��       5   +   *, +)(�� d     5      *, +)�� 5      "  6+   	 6Q   �� *
SubItems+.R�5      �� ��6    �� *Name+5      �� 5      "  6   .R�5      �� ��6    �� *Name+5      5      5       �� 5"   
   $��1��5#      ��5"      �� *SelectedItem+ 5$      
 � RowDefinitions �� IsAsync��%�  (��       5%      *, +)(�� �    56      *, +)�� 5%      "  6   .R�5&      � 
 5'      �� 5(      $
��Auto �6,   �� 5)       5+      �� 5,      �� 5-      .+�5/      �� $��Folder: ��6$   �� 50      $
��Bold"�6:   $��1��6L   ��6$   �� *Name+6L   �� 51      $��Number of items: ��6$   $��1��6=   �� 52      $
��Bold"�6D   $��1��53   $   $��1��61   ��52   $   �� *SubItems.Count+53   1   54      55      �� 56      "  6   .R�57      �� .+�58      �� �� 59      $�File: ��6"   $
��Bold"�60    �� 5:      �6"   �� *Name+5;      �� 5<      ��6$   �� *	Content+ . 5=      5>      5?      5@      5A      5B      