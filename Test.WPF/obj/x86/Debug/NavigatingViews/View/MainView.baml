   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Test.WPF.NavigatingViews.ViewModel"Test.WPF.NavigatingViews.ViewModel  5   	   M+clr-namespace:Test.WPF.NavigatingViews.ViewTest.WPF.NavigatingViews.View  5   	   ,    &Test.WPF.NavigatingViews.View.MainView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Blocal0clr-namespace:Test.WPF.NavigatingViews.ViewModel     5   	   A	localView+clr-namespace:Test.WPF.NavigatingViews.View     5   	     � Title$  MainView��5   	   $	��300��6   $	��300��6'    � DataContext 5      6   0Test.WPF.NavigatingViews.ViewModel.MainViewModel  5   
   .��5
      � -   5         �Name$ 
LayoutRoot��5
       � ColumnDefinitions 5   
   �� 5      $
��0.2* �6   �� 5      $
��Auto �6   �� 5      $
��0.8* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� $
��Left=�6   $��5��62   $��1��5      $��1��5      �� 5   
   $��0q�6   $	��Top=�5      $��23��5      $��3��5      $��2��5      5   /Test.WPF.NavigatingViews.View.NavigationControl  5%   
   $��0,10,0,0q�5&      $��1��5'       6   �� �� 5)   
   $��0q�6   $��1��5+      $��2��5,      ��5)   %   ��  �� Path$ CurrentView�5,      �� 5-   
   $��3��6   
���   ���6   �� *Name+67   5.      5/      