extends Control

var opened = false;

@onready var Start = $Menu/Buttons/Start;
@onready var ButtonMenu = $Menu/Buttons; 
@onready var Button1 = $Menu/Buttons/CheckButton;
@onready var Button2 = $Menu/Buttons/CheckButton2;
@onready var Button3 = $Menu/Buttons/CheckButton3;
@onready var Button4 = $Menu/Buttons/CheckButton4;

func _ready():
	$PressKey/Text/TextFlash.play("FadeText")
	
func _input(event):
	if event.is_action_pressed("ui_accept"):
		toggle_menu()
		
func toggle_menu():
	$PressKey.hide()
	$Menu.show()

func _on_start_pressed():
	get_tree().change_scene_to_file("res://scenes/test.tscn")

func _on_check_button_pressed():
	pass # Replace with function body.

func _on_check_button_2_pressed():
	pass # Replace with function body.

func _on_check_button_3_pressed():
	pass # Replace with function body.

func _on_check_button_4_pressed():
	pass # Replace with function body.

 
