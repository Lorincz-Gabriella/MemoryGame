   M S B A M L   `   `   `  ����D  @JocMemory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:JocMemory.ViewModelJocMemory.ViewModel  5   	   ;"clr-namespace:JocMemory.ConvertersJocMemory.Converters  5   	       JocMemory.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:JocMemory.ViewModel     5   	   3conv"clr-namespace:JocMemory.Converters     5   	     � Title$  
MainWindow��5	   	   $	��600��6   $	��950��6)   
������� � DataContext 5      &    JocMemory.ViewModel.MeniuCommand  5   
    � 	Resources 5      -   'JocMemory.Converters.ImagePathConverter   ImagePathConverter  RoundedButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Padding	 �� Triggers
 P� Property P� Value��%-  &	        &	        5   
   6"   �� 5   
   " ��6+   .�5      ��  � ��$	 140��6&   �� 5       � ��$ 45��6'   �� 5       � ��
 �����6+   �� 5       � �� �#5047466+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 16(�6)   �� 5       � ��$ Segoe Script)�6+   �� 5       � �� �#B896856,   �� 5       � ��$ 1q�60   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5       �� #��z��6!   #��z��5!   !   #��z��5"   !   $ 15��5#   !   $ 5q�5$   !   ._�5%      �� $��Center=�6/   $��Center=�6L   5&      5'      5)      	 5*      P� 5+      
 �@J�IsMouseOver . . �5,      ��  � �� �#E6D5C463   5-      5/      51      .��53      � ��54   
   �� 55       j� Stretch$
 Fill=�57       �� Opacity$	 0.4Q�6     �� ImageSource 56      �� *BackgroundImagePath+ �� 	Converter# [  57       .+�5:   
   �� $��Memory Game��6   $��Center=�5;      $	��Top=�5<      $��0,20,0,0q�5=      $��60(�5>      $
��Bold"�5?      $��Segoe Script)�5@       �� TextAlignment$ Center=�5A      ��5B      �� 5C       �� 
StartPoint$	 0,0&�6&    �� EndPoint$	 1,1&�67   .��5D      �  � Color$ White��6#    � Offset$	 0.0Q�61   � 5E      $ #B89685��6#   $	 1.0Q�63   5F       J� Effect 5H      3  -System.Windows.Media.Effects.DropShadowEffect  5I         Color$ Black��6#      
BlurRadius$ 8Q�61      ShadowDepth$ 4Q�6@      Opacity$	 0.5Q�6P   5K      �� 5N   
   $��Users��6   $��20(�5O      $
��Bold"�5P      
�������$��140,100,555,160q�5R      
������ 
�������$��1q�5U      .��5V      ��  �� DisplayMemberPath$
 Name��5X      $��18(�5Y      
������ $��0q�5[      
�������$��Center=�5]      ��5V      �� *Users+ �� SelectedItem 5W      �� *SelectedUser+5]      5^      �� 5`   
   $	��250��6   $	��250��6   $��570,130,50,50q�5a      $
��Left=�5b      $	��Top=�5c      ��5d      �� *SelectedUser.ImagePath+# [  �� 5f   
   $��New User��6   $
��Left=�5g      $��100,480,0,0q�6,   $	��Top=�5h      #��[ 6)   ��5f   $   �� *AddUserCommand+5h   )   �� 5j   
   $
��Exit��6   $
��Left=�5k      $��295,480,0,0q�6,   $	��Top=�5l      #��[ 6)   ��5j       �� *ExitCommand+5l   )   �� 5n   
   $
��Play��6   $
��Left=�5o      $��495,480,0,0q�6,   $	��Top=�5p      #��[ 6)   ��5n       �� *PlayCommand+5p   )   �� 5r   
   $��Delete User��6   $
��Left=�5s      $��695,480,0,0q�6,   $	��Top=�5t      #��[ 6)   ��5r   '   �� *DeleteUserCommand+5t   )   5u      5v      