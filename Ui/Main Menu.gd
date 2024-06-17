extends Control

var opened = false;

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

func _on_biome_menu_item_selected(index):
	Global.selected_index = index
	print(Global.selected_index)

func _on_wiz_number_value_changed(value):
	Global.wiz = value
	print(Global.wiz)
	
func _on_mana_value_changed(value):
	Global.mana = value
	print(Global.mana)
