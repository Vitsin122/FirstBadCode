   M S B A M L   `   `   `  ????F  BPP4_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PP4_ProjectPP4_Project  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 5   	       PP4_Project.Table   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PP4_Project     5   	   Bcol2clr-namespace:System.Collections;assembly=mscorlib     5   	     ? Title$  Table??5	   	   $	??450??6   $	??800??6$   .??5
      ? .+?5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ paymentGrid??5         AutoGenerateColumns$ False??6(      HorizontalGridLinesBrush
 ?????   RowBackground
 ?????   AlternatingRowBackground
 ?????   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Месяц??6%      
IsReadOnly$
 True??64   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 90 6d   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6F   ??  ?? Path$
 Term?6d     5      $ Платеж??6%   $
 True??65   $	 50 6h   
 6G   ?? $ Payment?6h   5      5      5      