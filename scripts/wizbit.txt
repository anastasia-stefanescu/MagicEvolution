extends CharacterBody2D

var rotation_dir = 0
var rotation_speed = 2
const speed = 300.0
@onready var animated_sprite_2d = $AnimatedSprite2D
var ai_x = 0
var ai_y = 0
var ai_rotation = 0
var frame_cnt = 0
var test1 = load("res://test.cs")
var test2 = load("res://test_2.cs")

func _init():
	#var test1_a = test1.new()
	var test2_a = test2.new()
	#print(test1_a.whatever())
	print(test2_a.whenever())


func get_ai_input():
	rotation_dir = 0
	"""
	if Input.is_action_pressed('rotate_right'):
		rotation_dir += 1
	if Input.is_action_pressed('rotate_left'):
		rotation_dir -= 1

	velocity = Vector2()
	if Input.is_action_pressed('ui_right'):
		velocity.x += 1
	if Input.is_action_pressed('ui_left'):
		velocity.x -= 1
	if Input.is_action_pressed('ui_down'):
		velocity.y += 1
	if Input.is_action_pressed('ui_up'):
		velocity.y -= 1
	"""
	velocity = Vector2()
	rotation_dir += ai_rotation
	velocity.x += ai_x
	velocity.y += ai_y
	if frame_cnt % 20 == 0:
		ai_rotation = randf_range(-1, 1)
		print("R: ", ai_rotation)
	if frame_cnt % 60 == 0:
		ai_x = randf_range(-1, 1)
		print("X: ", ai_x)
		
	if frame_cnt % 120 == 0:
		ai_y = randf_range(-1, 1)
		print("Y: ", ai_y)
		
		"""
	if sqrt(velocity.x*velocity.x + velocity.y*velocity.y) > 1:
		velocity=velocity.normalized()
		"""	
	velocity = velocity * speed

func _physics_process(delta):
	get_ai_input()
	frame_cnt = (frame_cnt + 1) % 120
	#if(Input.is_action_pressed('ui_right') || Input.is_action_pressed('ui_left') || Input.is_action_pressed('ui_down') || Input.is_action_pressed('ui_up')):
	if(velocity.x != 0 || velocity.y != 0):
		animated_sprite_2d.play("running")
	else:
		animated_sprite_2d.play("idle")
	rotation += rotation_dir * rotation_speed * delta
	velocity = velocity.rotated(rotation)
	move_and_slide()
