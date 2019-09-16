-- Main application logic for the game.
-- Game : World class. Contains world data.
import ('Aquarium', 'Aquarium')
import ('System.Numerics')

require 'Data/States/shark_wander'

local WINDOW_WIDTH = 1280
local WINDOW_HEIGHT = 720

function flock(entity)
	print(entity)
end

flocking = State()
flocking.Enter = function() end
flocking.Execute = function()
	local sm = flocking.SM
	local target = sm.Target
	local flocks = game:GetMovingEntitiesByTag("fish")
	target:AddForce(Behaviours.Flock(target, flocks))
	
	if Vector2.Distance(target.Position, shark.Position) < 100 then
		target:AddForce(Behaviours.Flee(target, shark.Position))
	end

	target:AddForce(Vector2(0.1, 0))
end
flocking.Exit = function() end

math.randomseed(os.time())

-- Create the shark object.
shark = MovingEntity("shark", 10, 10)
shark:SetLocal("Name", "Jack")
shark:SetLocal("Health", 100)
shark:SetLocal("Hunger", 50)
shark.Mass = 50.0
shark.MaxSpeed = 4.5
shark.DrawSize = 20.0
game:AddEntity(shark)

for i = 0, 1 do
	local fish = MovingEntity("fish", math.random(WINDOW_WIDTH), math.random(WINDOW_HEIGHT))
	-- local sm_fish = StateMachine(fish)
	
	fish.DrawSize = 5.0 + math.random(7)
	fish.ShowDebug = false
	fish.Mass = 30.0
	fish.MinSpeed = 1.5
	fish.MaxSpeed = 5.0
	-- sm_fish:SetState(flocking)
	fish:AddLocalAction(flock(fish))

	game:AddEntity(fish)
	-- game:AddStateMachine(sm_fish)
end

-- Create the state machine for the shark.
sm_shark = StateMachine(shark)
sm_shark:SetState(state_sharkwander)
game:AddStateMachine(sm_shark)

print "Lua script executed without issues, starting program..."

game:SetTickSpeed(5)
game:Start()

return "Lua application ran successfully."