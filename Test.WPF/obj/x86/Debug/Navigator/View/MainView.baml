   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Test.WPF.Navigator.ViewTest.WPF.Navigator.View  5   	   K*clr-namespace:Test.WPF.Navigator.ViewModelTest.WPF.Navigator.ViewModel  5   	   D @SCA.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:SCA.Model;assembly=SCA.Model	SCA.Model 5   	   (    "Test.WPF.Navigator.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6view%clr-namespace:Test.WPF.Navigator.View     5   	   @	viewModel*clr-namespace:Test.WPF.Navigator.ViewModel     5   	   <model*clr-namespace:SCA.Model;assembly=SCA.Model     5   	     � Title$  
MainWindow��5   	   $��626.224��6   $��515.035��6-    � 	Resources 5       �� DataType:   4Test.WPF.Navigator.ViewModel.DeviceInfo8036ViewModel0   *Test.WPF.Navigator.View.DeviceInfo8036View � DataContext �� Path:   4Test.WPF.Navigator.ViewModel.DeviceInfo8001ViewModel0   *Test.WPF.Navigator.View.DeviceInfo8001ViewA   ;Test.WPF.Navigator.ViewModel.LinkageConfigStandardViewModel7   1Test.WPF.Navigator.View.LinkageConfigStandardView@   :Test.WPF.Navigator.ViewModel.LinkageConfigGeneralViewModel6   0Test.WPF.Navigator.View.LinkageConfigGeneralView>	   8Test.WPF.Navigator.ViewModel.LinkageConfigMixedViewModel4
   .Test.WPF.Navigator.View.LinkageConfigMixedView>   8Test.WPF.Navigator.ViewModel.ManualControlBoardViewModel4   .Test.WPF.Navigator.View.ManualControlBoardView��%_  (��       5	      *, +)(�� n     5      *, +)(�� �     5      *, +)(�� Y    5      *, +)(�� �    5      *,	 +)(�� O    5"      *, +)�� 5	   
   "  6   .R�5
         6&   �� $$ DeviceInfoObservableCollection�5      5      �� 5   
   "  6   .R�5         6&   �� $$ DeviceInfoObservableCollection�5      5      �� 5   
   "  6   .R�5         6-   �� $3 -StandardLinkageConfigInfoObservableCollection�5      5      �� 5   
   "  6   .R�5         6,   �� $2 ,GeneralLinkageConfigInfoObservableCollection�5      5      �� 5   
   " 	 6   .R�5      
   6*   �� $0 *MixedLinkageConfigInfoObservableCollection�5       5!      �� 5"   
   "  6   .R�5#         6*   �� $0 *ManualControlBoardInfoObservableCollection�5%      5&      5(      .��5)      �  � ColumnDefinitions 5*   
   �� 5+      $	��150 �6   �� 5,      $��* �6    � RowDefinitions 5.   
   �� 5/      $��80 �6   �� 50      $	��500 �6   �� 51      $��20 �6   .+�53   
   �� $��0��6   $��2��6!   
�������6R   �� 54   
   $��1��6   $��2��6   
�������6H   �� 55   
   $��1��6   $��1��6   
�����_�6G   )   #Test.WPF.Navigator.View.UCNavigator  56   
   $��0,10,0,0q�6   $��1��6-    6;   �� �� 57   
   -   5         ��Name$ ContentArea��57   8   $��1��6   $��2��6(   ��6M   �� *CurrentView+59      5:      