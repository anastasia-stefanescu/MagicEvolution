extends Control

# Called when the node enters the scene tree for the first time.
func _ready():
	$Text/Valori/Id.text = "-"
	$Text/Valori/Hp.text = "-"
	$Text/Valori/Mana.text = "-"
	$Text/Valori/Gen.text = "-"
	$Text/Valori/Neuron.text = "-"
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func update_menu(id, hp, maxHp, mana, maxMana, gen, neurons):
	$Text/Valori/Id.text = "[Center]" + id;
	$Text/Valori/Hp.text = "[Center]" + hp + " / " + maxHp;
	$Text/Valori/Mana.text = "[Center]" + mana + " / " + maxMana;
	$Text/Valori/Gen.text = "[Center]" + gen
	$Text/Valori/Neuron.text = "[Center]" + neurons;


func _on_close_pressed():
	print("CLOSED FUCKER!")


func _on_close_button_down():
	print("CLOSED FUCKER!")
