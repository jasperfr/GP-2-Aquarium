st_findtarget = State()

st_findtarget.Enter = function ()
	local sm = st_findtarget.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " is finding a new target to wander to.")
end

st_findtarget.Execute = function ()
	local sm = st_findtarget.SM
	local target = sm.Target
	local name = target:GetLocal("Name")
	local position = target.Position
	local target_vector = Vector2(math.random(1280), math.random(720))

	if Vector2.Distance(position, target_vector) > 200 then
		shark:SetLocal("Target", target_vector)
		sm:SetState(st_wander)
	else
		print(name .. " hasn't found a target yet.")
	end
end

st_findtarget.Exit = function ()
	local sm = st_findtarget.SM
	local target = sm.Target
	local name = target:GetLocal("Name")

	print(name .. " has found a new target.")
end
