st_wander = State()

st_wander.Enter = function ()
	local sm = st_wander.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " is wandering towards the target.")
end

st_wander.Execute = function ()
	local sm = st_wander.SM
	local target = sm.Target
	local position = target.Position
	local target_vector = target:GetLocal("Target")
	
	target:AddForce(Behaviours.Arrive(target, target_vector))

	local hunger = target:GetLocal("Hunger")
	hunger = hunger - 0.02
	if hunger < 10 then -- FUZZY LOGIC VARIABLE!
		sm:SetState(st_findfood)
	else
		target:SetLocal("Hunger", hunger)
	end
	
	local position = target.Position
	if Vector2.Distance(position, target_vector) < 20 then
		sm:SetState(st_findtarget)
	end
end

st_wander.Exit = function ()
	local sm = st_wander.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " stopped wandering around.")
	target:RemoveLocal("Target")
end
