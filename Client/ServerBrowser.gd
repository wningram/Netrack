extends CanvasLayer


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var http_node
var serveraddress = "http://blah/Game/"

#false status means no connection/offlie, true means connected
var isOnline = false

# Called when the node enters the scene tree for the first time.
func _ready():
	http_node = HTTPRequest.new()
	add_child(http_node)
	http_node.connect("request_completed", self, "_http_request_completed")


func _on_Button_pressed():
	#check if server is up
	var error = http_node.request(serveraddress+"Status")
	if error != OK:
		push_error("Dat HTTP addy don't work real gud it seems...")
	
func _http_request_completed(result, response_code, headers, body):
	var response = JSON.parse(body.get_string_from_utf8())
	print(response)
