extends Node

static var instance
@onready var mana = $Mana

var all_mana = []

func _ready():
	instance = self

func _process(delta):
	replenish_mana(15)
	
func getRandomPosition(maxsize) -> Vector2:
	var x = randi_range(0, maxsize)
	var y = randi_range(0, maxsize)
	return Vector2(x, y)

func replenish_mana(treshold):
	while len(all_mana) < treshold:
		var mana_instance = mana.new(getRandomPosition(20))
		all_mana.append(mana_instance)
		
func get_all_mana() -> Array:
	return all_mana
