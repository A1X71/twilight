   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Test.WPF.AnotherNavigatingViews.Views%Test.WPF.AnotherNavigatingViews.Views  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   6    0Test.WPF.AnotherNavigatingViews.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Elocal3clr-namespace:Test.WPF.AnotherNavigatingViews.Views     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$  
MainWindow��5   	   $	��420��6   $	��787��6)    � 	Resources 5       
  client  product��%U   &	        &	       ��5	   
   clientUC��6.   ��5
   
   	productUC��60   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��80 �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� $��0��6   $��2��6!   
�������6R   �� 5   
   $��1��6   $��2��6   
�������6H   �� 5   
   $��1��6   $��1��6   
�����_�6G   �� 5   
   -   5         ��Name$ ContentArea��5   8   $��1��6   $��1��6(   ��6M   �� *CurrentView+�� 5   
   $��5q�6   $��0��6    $��1��60   .+�5      �� ��6   �� *SwitchViewsCommand+��6=   ��  �� Source# [  .��6z   	Clients6�   �� 5      ��6   �� *SwitchViewsCommand+��6=   �� # [ .��6|   
Products6�   5"      5#      5$      