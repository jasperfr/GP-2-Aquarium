st_seektarget = State()

st_seektarget.Enter = function ()
	local sm = st_seektarget.SM
	local target = sm.Target
    local name = target:GetLocal("Name")
    
	print(name .. " is heading towards its target.")
end

st_seektarget.Execute = function ()
	local sm = st_seektarget.SM
    local target = sm.Target
    
	local position = target.Position
	local target_vector = target:GetLocal("SeekTarget").Position
	
	target:AddForce(Behaviours.Seek(target, target_vector))

	local position = target.Position
	if Vector2.Distance(position, target_vector) < 50 then
		sm_shark:SetState(st_eattarget)
	elseif Vector2.Distance(position, target_vector) > 200 then
		sm_shark:SetState(st_findfood)
	end
end

st_seektarget.Exit = function ()
	local sm = st_seektarget.SM
	local target = sm.Target
    local name = target:GetLocal("Name")
    
	print(name .. " has arrived to the target, or not.")
end
