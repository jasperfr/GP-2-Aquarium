import ('Aquarium', 'Aquarium')
import ('System')
import ('System.Numerics')
import ('System.Math')
import ('System.Dynamic')
import ('System.Collections.Generic')
require 'sprites'
require 'objects/fish'
require 'objects/coral'
require 'objects/rocks'
require 'objects/shark'
require 'objects/bubbles'

math.randomseed(os.time())

for i = 0, 9 do
    local fish = game:CreateInstance("obj_fish", math.random(1280), math.random(720))
    fish.Size = 48 + math.random() * 4
end

for i = 0, 19 do
    local bubbles = game:CreateInstance("obj_bubbles", math.random(1280), math.random(720))
    bubbles:Set('sinoid', 0.2)
    bubbles:Set('vspeed', math.random() * 2)
    bubbles.Size = bubbles:Get('vspeed') * 24
end

--[[
t_baba = GameObject()
t_baba.BaseSprite = spr_text_baba
t_baba.Size = 48.0
game:AddObject("obj_t_baba", t_baba)

t_is = GameObject()
t_is.BaseSprite = spr_text_is
t_is.Size = 48.0
game:AddObject("obj_t_is", t_is)

t_you = GameObject()
t_you.BaseSprite = spr_text_you
t_you.Size = 48.0
game:AddObject("obj_t_you", t_you)

t_fish = GameObject()
t_fish.BaseSprite = spr_text_fish
t_fish.Size = 48.0
game:AddObject("obj_t_fish", t_fish)


-- Make small bubbles

bg = GameObject()
bg.BaseSprite = spr_bg
bg.Size = 1280

-- Make big bubbles
for i = 0, 19 do
    local bubble = game:CreateInstance("obj_bubble", math.random(1280), math.random(720))
    bubble:SetLocal("vspeed", math.random() * 5)
    bubble.Size = bubbles:GetLocal("vspeed") * 10
end

-- Make sharks

-- Make fish
]]--

game:CreateInstance("obj_shark", 24, 24, EStateMachine(state_sharkwander), "Foo" , ConsoleColor.Green)
game:CreateInstance("obj_shark", 76, 76, EStateMachine(state_sharkwander), "Bar" , ConsoleColor.Magenta)
game:CreateInstance("obj_shark", 76, 76, EStateMachine(state_sharkwander), "Baz" , ConsoleColor.Yellow)

game:GenerateMap("lua/map.txt", 48)

game:SetSpatialGridSize(48);
game:SetTickSpeed(5)
game:Start()

return "LUA OK!"