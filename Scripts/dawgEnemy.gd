extends CharacterBody2D

@onready var target = get_parent().get_node("../player")
@onready var animated_sprite = $AnimatedSprite2Ddawg

var speed = 300
var stop_distance = 300  
var direction



func _physics_process(_delta):
	var distance = position.distance_to(target.position)
	
	if distance > stop_distance:
		direction = (target.position - position).normalized()
		velocity = direction * speed
		
		
		if is_on_floor():
			animated_sprite.play("run")

	else:
		velocity = Vector2.ZERO
	
	look_at(target.position)
	rotation = 0
	
		

	move_and_slide()
