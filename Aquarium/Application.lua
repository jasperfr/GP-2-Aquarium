-- Main application logic for the game.
-- Game : World class. Contains world data.
import ('Aquarium', 'Aquarium')
import ('System.Numerics')

shark = Entity(10, 10)
shark:SetLocal("Name", "Jack")
shark:SetLocal("Health", 100)
shark:SetLocal("Hunger", 20)
shark:SetLocal("Target", Vector2(100, 100))

-- State machine delegate function calls.
sm_shark = StateMachine()

--[[
	Shark find new target state
]]
function shark_findtarget_enter()
	local name = shark:GetLocal("Name")
	print(name .. " is finding a new target to wander to.")
end
function shark_findtarget_execute()
	local name = shark:GetLocal("Name")
	local position = shark.Position
	local target = Vector2(math.random(200), math.random(200))
	if Vector2.Distance(position, target) > 50 then
		shark:SetLocal("Target", target)
		sm_shark:SetState(shark_wander_state)
	else
		print(name .. " hasn't found a target yet.")
	end
end
function shark_findtarget_exit()
	local name = shark:GetLocal("Name")
	print(name .. " has found a new target.")
end
shark_findtarget_state = State(shark_findtarget_enter, shark_findtarget_execute, shark_findtarget_exit)

--[[
	Shark wander state
]]--
function shark_wander_enter()
	local name = shark:GetLocal("Name")
	print(name .. " is wandering towards the target.")
end
function shark_wander_execute()
	local position = shark.Position
	local target = shark:GetLocal("Target")
	
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
	if Vector2.Distance(position, target) < 5 then
		sm_shark:SetState(shark_findtarget_state)
	end
end
function shark_wander_exit()
	local name = shark:GetLocal("Name")
	print(name .. " stopped wandering around.")
end
shark_wander_state = State(shark_wander_enter, shark_wander_execute, shark_wander_exit)

function xyz_enter()
	print("Entered XYZ state.")
end
function xyz_execute()

	local hunger = shark:GetLocal("Hunger")
	local health = shark:GetLocal("Health")

	if(hunger > 0) then
		shark:SetLocal("Hunger", hunger - 1)
	else
		shark:SetLocal("Health", health - 1)
	end
	
	-- print("Running XYZ state.  (Moving the shark around.)")
	-- print("Shark is at " .. shark.Position.X .. "," .. shark.Position.Y .. "\n")
end
function xyz_exit()
	print("Exited XYZ state.")
end
xyz_state = State(xyz_enter, xyz_execute, xyz_exit)

sm_shark:SetState(shark_findtarget_state)
game:AddStateMachine(sm_shark)

game:AddEntity(shark)
game:SetTickSpeed(25)

print "Lua script executed without issues, starting program..."

game:Start()

return "Lua application ran successfully."