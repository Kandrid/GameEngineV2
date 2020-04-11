# GameEngineV2
Redesign of the original GameEngine

Raster Fonts 8x8 is required for proper display of the Console Engine.
12x16 can be used for a larger view but also a slightly less square ratio.

-Documentation, by a friend-

Notes:

Press Tab to clear any visual artifacts left on the screen should they occur when changing the window size.


x and y are centered at the top left of the map, incrementing down and to the right.
[Square bracket] parameters are optional.
[string name]/[int x, int y] indicates access to entities by their name or by their coordinate. 
Map file must be in the same folder as the exe file.
The first map must be called : default.cmap.
Create a text file with the extension .cmap for the map file. 
When creating the text file, map must be rectangular and to indicate the next line use new lines. A new line must be used at the end of the final line as well.
The map height and width are stored in MAP_HEIGHT and MAP_WIDTH variables respectively


Entity Constructor:

Entity {variable name} = new Entity([string name], char symbol, int x, int y, [float Vx], [float Vy]) 
Creates an entity. The symbol is the displayed character. Vx and Vy are the velocities.

All the entities that get constructed get added to a HashSet named Entities.
You may add an arbitrary entity to the HashSet by doing : new Entity(...)


Entity Functions:

Note: When calling static versions of functions, call them through the Entity class (ex. Entity.Delete(...)), otherwise call them through the entity object in question.

Delete(int x, int y, [char symbol])
Deletes all entities on a specific tile.
When using a character it will only delete all entities on the tile that match the character.

MoveBy([string name]/[int x, int y], float xFloat, float yFloat)
Moves entities by a specified amount.

MoveTo([string name]/[int x, int y], float xFloat, float yFloat)
Move entity to a specific location.

MoveAll(char symbol, float xFloat, float yFloat)
Moves all entities of a specific character by a specific amount.

SetVelocity([string name]/[int x, int y], float Vx, float Vy)
Sets the velocity of an entity.

SetType([string name]/[int x, int y], char symbol)
Change the symbol of an entity.

ContainsEntity(int x, int y)
Checks whether an entity is at specified location.

Contains(char symbol, int x, int y)
Checks whether an entity of a specified character is at a specified location.

EntityMap(int x, int y, [char symbol])
Returns the first entity found at specified location.


Camera Functions:

ZoomIn()
Changes the CAMERA range towards the center of the screen.

ZoomOut()
Changes the CAMERA range away from the center of the screen.

MoveUp()
Moves the location of the CAMERA 1 tile up.

MoveDown()
Moves the location of the CAMERA 1 tile down.

MoveLeft()
Moves the location of the CAMERA 1 tile left.

MoveRight()
Moves the location of the CAMERA 1 tile right.


Map Function:

LoadMap(string mapFileName)
Loads a designated map file.


Engine Tools:

int FRAMERATE - Used to set a desired framerate for the engine to approach.

int AVERAGE_FRAMES - Used to set the number of frames for the engine to use to average the displayed framerate value.

bool FRAME_BY_FRAME - Used to enable advancing 1 frame at a time by pressing any button.

Inputs can be taken by accessing the "INPUT" variable which contains the current ConsoleKey input value. ex: User press 'A', INPUT == ConsoleKey.A

Debug DEBUG_MODE - Debug enum controlling debug mode {Full, Partial, None}
    Full: Displayed framerate + entity count + full properties of up to 20 recent entities.
    Partial: Displayed framerate + entity count.

Custom debug variables can be added by adding a name and variable value to the customDebugVariables List at the end of each frame to log its current value.
customDebugVariables.Add("[name]", [variable])
