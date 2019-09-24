st_eattarget = State()

st_eattarget.Enter = function ()
	local sm = st_eattarget.SM
	local target = sm.Target
    local name = target:GetLocal("Name")

	print(name .. " is eating the target fish!")
end

st_eattarget.Execute = function ()
	local sm = st_eattarget.SM
    local target = sm.Target
    
	-- TODO might add hitpoints to the target, idk.
	local seektarget = target:GetLocal("SeekTarget")
	local hunger = target:GetLocal("Hunger")
	game:Destroy(seektarget)
	target:RemoveLocal("SeekTarget")
	target:SetLocal("Hunger", hunger + 5)
	sm_shark:SetState(st_findtarget)
end

st_eattarget.Exit = function ()
	local sm = st_eattarget.SM
	local target = sm.Target
    local name = target:GetLocal("Name")

	print(name .. " has finished eating the target.")
end
