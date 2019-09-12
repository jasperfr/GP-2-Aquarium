-- Main application logic for the game.
-- Game : World class. Contains world data.
import ('Aquarium', 'Aquarium')
import ('System.Numerics')

local WINDOW_WIDTH = 640

math.randomseed(os.time())

shark = Entity("shark", 10, 10)
shark:SetLocal("Name", "Jack")
shark:SetLocal("Health", 100)
shark:SetLocal("Hunger",15)
shark:SetLocal("Target", Vector2(100, 100))
game:AddEntity(shark)

--[[ foreach iterations go like this:
local targets = game:GetEntitiesByTag("fish")
for i = 0, targets.Count - 1 do
	print(i, targets[i])
end
]]--

-- State machine delegate function calls.
sm_shark = StateMachine(shark)

--[[
	Fish find new target state
]]
function fish_find_target_enter()
	local sm = fish_findtarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " is finding a new target to wander to.")
end
function fish_find_target_execute()
	local sm = fish_findtarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	local position = target.Position
	local target_vector = Vector2(math.random(600), math.random(400))
	if Vector2.Distance(position, target_vector) > 50 then
		target:SetLocal("Target", target_vector)
		sm:SetState(fish_gototarget_state)
	else
		print(name .. " hasn't found a target yet.")
	end
end
function fish_find_target_exit()
	local sm = fish_findtarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " has found a new target.")
end
fish_findtarget_state = State(fish_find_target_enter, fish_find_target_execute, fish_find_target_exit)

--[[
	Fish goto target state
]]
function fish_gototarget_enter()
	local sm = fish_gototarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")
	print(name .. " is going to the target.")
end
function fish_gototarget_execute()
	local sm = fish_gototarget_state.SM
	local target = sm.Target
	local position = target.Position
	local target_vector = target:GetLocal("Target")
	
	if(position.X > target_vector.X) then
		target:SetPosition(-1, 0, true)
	end

	if(position.X < target_vector.X) then
		target:SetPosition(1, 0, true)
	end
	
	if(position.Y > target_vector.Y) then
		target:SetPosition(0, -1, true)
	end

	if(position.Y < target_vector.Y) then
		target:SetPosition(0, 1, true)
	end
	
	local position = target.Position
	if Vector2.Distance(position, target_vector) < 5 then
		sm:SetState(fish_findtarget_state)
	end
end
function fish_gototarget_exit()
	local sm = fish_gototarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " has wandered to its target position.")
end
fish_gototarget_state = State(fish_gototarget_enter, fish_gototarget_execute, fish_gototarget_exit)

local fishnames = {"Guppy", "Fishie", "Jill", "Fish2", "SuperFish"}
for i = 0, 4 do
	local fish = Entity("fish", math.random(640), math.random(400))
	fish:SetLocal("Name", fishnames[i + 1])
	fish:SetLocal("Health", 10);
	local sm_fish = StateMachine(fish)
	sm_fish:SetState(fish_findtarget_state)
	game:AddEntity(fish)
	game:AddStateMachine(sm_fish)
end

--[[
	Shark find new target state
]]
function shark_findtarget_enter()
	local sm = shark_findtarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " is finding a new target to wander to.")
end
function shark_findtarget_execute()
	local sm = shark_findtarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	local position = target.Position
	local target_vector = Vector2(math.random(200), math.random(200))
	if Vector2.Distance(position, target_vector) > 50 then
		shark:SetLocal("Target", target_vector)
		sm:SetState(shark_wander_state)
	else
		print(name .. " hasn't found a target yet.")
	end
end
function shark_findtarget_exit()
	local sm = shark_findtarget_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " has found a new target.")
end
shark_findtarget_state = State(shark_findtarget_enter, shark_findtarget_execute, shark_findtarget_exit)

--[[
	Shark wander state
]]--
function shark_wander_enter()
	local sm = shark_wander_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " is wandering towards the target.")
end
function shark_wander_execute()
	local sm = shark_wander_state.SM
	local target = sm.Target
	local position = target.Position
	local target_vector = target:GetLocal("Target")
	
	if(position.X > target_vector.X) then
		target:SetPosition(-1, 0, true)
	end

	if(position.X < target_vector.X) then
		target:SetPosition(1, 0, true)
	end
	
	if(position.Y > target_vector.Y) then
		target:SetPosition(0, -1, true)
	end

	if(position.Y < target_vector.Y) then
		target:SetPosition(0, 1, true)
	end

	local hunger = target:GetLocal("Hunger")
	hunger = hunger - 0.1
	if hunger < 10 then -- FUZZY LOGIC VARIABLE!
		sm:SetState(shark_find_food_state)
	else
		target:SetLocal("Hunger", hunger)
	end
	
	local position = target.Position
	if Vector2.Distance(position, target_vector) < 5 then
		sm:SetState(shark_findtarget_state)
	end
end
function shark_wander_exit()
	local sm = shark_wander_state.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " stopped wandering around.")
	target:RemoveLocal("Target")
end
shark_wander_state = State(shark_wander_enter, shark_wander_execute, shark_wander_exit)

--[[
	Shark find food state
	-- TODO : add either fuzzy logic or machine learning to determine at which
	--        percentage the shark should find food, (FULL, HUNGRY, STARVING)
	--		  minimum target range (FAR, NORMAL, NEAR)?
	--	      ===> Desirability of going to this target (VERY DESIRABLE, DESIRABLE, UNDESIRABLE)
]]
function shark_find_food_enter()
	local name = shark:GetLocal("Name")
	print(name .. " is getting hungry. " .. name .." is going to find something to eat.")
end
function shark_find_food_execute()
	local targets = game:GetEntitiesByTag("fish")
	shark:SetLocal("SeekTarget", targets[0])
	-- TODO fuzzy logic
	sm_shark:SetState(shark_seek_target_state)
end
function shark_find_food_exit()
	local name = shark:GetLocal("Name")
	print(name .. " has found food.")
end
shark_find_food_state = State(shark_find_food_enter, shark_find_food_execute, shark_find_food_exit)

--[[
	Shark goto food state
	-- TODO : if the target has not been caught in n amount of ticks,
	--		  find a new target.
]]
function shark_seek_target_enter()
	local name = shark:GetLocal("Name")
	print(name .. " is heading towards its target.")
end
function shark_seek_target_execute()
	local position = shark.Position
	local target = shark:GetLocal("SeekTarget").Position
	
	if(position.X > target.X) then
		shark:SetPosition(-1, 0, true)
	end

	if(position.X < target.X) then
		shark:SetPosition(1, 0, true)
	end
	
	if(position.Y > target.Y) then
		shark:SetPosition(0, -1, true)
	end

	if(position.Y < target.Y) then
		shark:SetPosition(0, 1, true)
	end
	
	local position = shark.Position
	if Vector2.Distance(position, target) < 2 then
		sm_shark:SetState(shark_eat_target_state)
	end
end
function shark_seek_target_exit()
	local name = shark:GetLocal("Name")
	print(name .. " has arrived to the target.")
end
shark_seek_target_state = State(shark_seek_target_enter, shark_seek_target_execute, shark_seek_target_exit)

--[[
	Shark eat target state
]]
function shark_eat_target_enter()
	local name = shark:GetLocal("Name")
	print(name .. " is eating the target fish!")
end
function shark_eat_target_execute()
	-- TODO might add hitpoints to the target, idk.
	local target = shark:GetLocal("SeekTarget")
	local hunger = shark:GetLocal("Hunger")
	game:Destroy(target)
	shark:RemoveLocal("SeekTarget")
	shark:SetLocal("Hunger", hunger + 50)
	sm_shark:SetState(shark_findtarget_state)
end
function shark_eat_target_exit()
	local name = shark:GetLocal("Name")
	print(name .. " has finished eating the target.")
end
shark_eat_target_state = State(shark_eat_target_enter, shark_eat_target_execute, shark_eat_target_exit)

sm_shark:SetState(shark_findtarget_state)
game:AddStateMachine(sm_shark)

print "Lua script executed without issues, starting program..."

game:SetTickSpeed(25)
game:Start()

return "Lua application ran successfully."