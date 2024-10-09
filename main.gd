extends Node


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	Common.connect("CommonSignal", _on_common_signal)


# Called every frame. 'delta' is the elapsed time since the previous frame.
# func _process(delta: float) -> void:
# 	pass


func SwitchScene(tscn: Resource) -> void:
	if (tscn == null):
		$Root.get_child(0).queue_free()
		$MainControl.visible = true
	else:
		print(tscn.resource_path)
		$MainControl.visible = false
		var new_scene = tscn.instantiate()
		$Root.add_child(new_scene)


func _on_common_signal(key : String, value: String) -> void:
	print("CommonSignal %s %s" % [key, value])
	match (key):
		"Home":
			SwitchScene(null)


func _on_keyboard_rhythm_pressed() -> void:
	print(%KeyboardRhythm.text)
	SwitchScene(preload("res://scenes/KeyboardRhythm.tscn"))


func _on_mouse_fishing_pressed() -> void:
	print(%MouseFishing.text)


func _on_mouse_building_pressed() -> void:
	print(%MouseBuilding.text)
