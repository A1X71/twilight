   M S B A M L   `   `   `  ����C  ?Test.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:Test.WPF.NavigatingViews.ViewTest.WPF.NavigatingViews.View  5      W0clr-namespace:Test.WPF.NavigatingViews.ViewModel"Test.WPF.NavigatingViews.ViewModel  5      5    /Test.WPF.NavigatingViews.View.NavigationControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:Test.WPF.NavigatingViews.View     5      KImplementation0clr-namespace:Test.WPF.NavigatingViews.ViewModel     5        � 	Resources  5
       �� DataType=   7Test.WPF.NavigatingViews.ViewModel.StockQuotesViewModel2   ,Test.WPF.NavigatingViews.View.StockQuoteView<   6Test.WPF.NavigatingViews.ViewModel.PortofolioViewModel1   +Test.WPF.NavigatingViews.View.PortfolioView��%�   (��       5      *, +)(�� 2     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      5      .��5      � -   5         �Name$ 
LayoutRoot��5       � RowDefinitions 5   
   �� 5      $
��Auto �6   $��45��6*   �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� 
���  ��$
��Wrap=�6@   ��6   ��  �� Path$
 Name�6@   �� 5   
   $��OK��6   
���  ��$
��Wrap=�6/   �� 5   
   $��1��5   (   ��6   �� $ CurrentView�6(   �� 5   
   $��Stock Quotes��6   $��2��6(   ��66   �� $ NavigateToStocksCommand��� 5   
   $��	Portfolio��6   $��3��6'   ��64   �� $  NavigateToPortfolioCommand�5       5!      