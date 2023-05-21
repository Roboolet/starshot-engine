extends Control

var tabArray

func _ready():
	tabArray = [$Tab_Project, $Tab_Scene, $Tab_Prefabs, $Tab_Sprites]

func _on_tab_changed(tab):
	for n in tabArray.size():
		if n == tab:
			tabArray[n].show()
		else:
			tabArray[n].hide()
