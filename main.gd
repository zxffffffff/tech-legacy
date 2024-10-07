extends Node


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func SwitchScene(tscn: Resource) -> void:
	print(tscn.resource_path)
	if (tscn.resource_path == "res://main.tscn"):
		$Root.get_child(0).queue_free()
		$MainControl.visible = true
	else:
		$MainControl.visible = false
		var new_scene = tscn.instantiate()
		$Root.add_child(new_scene)


func _on_keyboard_rhythm_pressed() -> void:
	print(%KeyboardRhythm.text)
	SwitchScene(preload("res://scenes/KeyboardRhythm.tscn"))


func _on_mouse_fishing_pressed() -> void:
	print(%MouseFishing.text)


func _on_mouse_building_pressed() -> void:
	print(%MouseBuilding.text)
