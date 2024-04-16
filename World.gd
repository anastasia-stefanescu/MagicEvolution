extends Node

var mana_scene = preload("res://mana.tscn")
var mana_bottles = 20000
func _ready():
	for x in mana_bottles:
		var mana = mana_scene.instantiate()
		mana.position = Vector2(randf_range(-10000, 10000),randf_range(-10000, 10000))
		add_child(mana)
