import ('Aquarium', 'Aquarium')
import ('System.Numerics')
require 'sprites'
require 'objects/shark'

math.randomseed(os.time())

game:SetSpatialGridSize(32);
game:SetTickSpeed(5)
game:Start()

return "LUA OK!"