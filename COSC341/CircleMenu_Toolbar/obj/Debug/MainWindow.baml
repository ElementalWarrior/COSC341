   M S B A M L   `   `   `  ����M  ICircleMenu_Toolbar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ToolbarToolbar  5   	   #    CircleMenu_Toolbar.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Toolbar     5   	     � Title$"  COSC341 - CircleMenu_Toolbar��5   	   $	��720��6.   $
��1280��6;    � 	Resources 5
       
  eraser � Cursor�� 5   
   %.   &	        �� 5      $ 
eraser.ani��6'   5      .��5      � -   5        $��canvas��5      .+�5   
   �� -   5         ��Name$ 	inkCanvas��5       �� 
Background
 �����$ Arrow��6:    �� Left$ 0��6I   
 �� Top$ 0��6Y   $
��1280��6h   $	��720��6u    �� UseCustomCursor .  �� EditingMode$
 None=�6�   �� 5   
   $	��340��6   $	��Top=�6    $��Center=�68   
�������$
��Wrap=�6h   $`��ZA right click will open the context menu. Press submit when you are finished each drawing.��5      5      �� 5   
   -   5        $��instructions��5      $	��Top=�6(   $��Center=�6@   $��Draw a red cricle��6]   
�������$��0,40,0,0q�6�   � 5   
   -   5        $��toolbar��5      $	��260��6   $��52��6*   	 K� ZIndex$	 1��66   $
��Left=�6G   $	��Top=�6b   $y�Hidden=�6z   .+�5      �� -   5        $��
background��5      $	��260��6*   $��52��66   $��Center=�6B   $��Center=�6_   
�������6z   �� 5      -   5        $��draw_button��5      $��52��6(   $��52��63   $
��Left=�6?   $	��Top=�6Z   $��0,0,0,0q�6r   
���   �$��1q�6�   ��6�   Z� .��5      �� $��draw.png��6   5      �� 5      -   5        $��erase_button��5      $��52��6)   $��52��64   $
��Left=�6@   $	��Top=�6[   $��52,0,0,0q�6s   
���   �$��0,1,1,1q�6�   ��6�   Z� .��5      �� $��	erase.png��6   5       �� 5!      -   5        $��black_button��5!      $��52��6)   $��52��64   $
��Left=�6@   $	��Top=�6[   $��	104,0,0,0q�6s   
���   �$��0,1,1,1q�6�   ��6�   Z� .��5"      �� $��	black.png��6   5#      �� 5$      -	   5        $��
red_button��5$      $��52��6'   $��52��62   $
��Left=�6>   $	��Top=�6Y   $��	156,0,0,0q�6q   
���   �$��0,1,1,1q�6�   ��6�   Z� .��5%      �� $��red.png��6   5&      �� 5'      -
   5        $��blue_button��5'      $��52��6(   $��52��63   $
��Left=�6?   $	��Top=�6Z   $��	208,0,0,0q�6r   
���   �$��0,1,1,1q�6�   ��6�   Z� .��5(      �� $��blue.png��6   5)      5+      �� 5-   
   -   5        $��submit_button��5-      $	��100��6&   $��50��62   $��Center=�6>   $��Bottom=�6[   $��Submit��6v   $��0,0,0,0q�6�   $��18(�6�   5.      5/      