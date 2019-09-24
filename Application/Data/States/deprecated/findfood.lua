--[[
	Shark find food state
	-- TODO : add either fuzzy logic or machine learning to determine at which
	--        percentage the shark should find food, (FULL, HUNGRY, STARVING)
	--		  minimum target range (FAR, NORMAL, NEAR)?
	--	      ===> Desirability of going to this target (VERY DESIRABLE, DESIRABLE, UNDESIRABLE)
]]
st_findfood = State()

st_findfood.Enter = function ()
	local sm = st_findfood.SM
	local target = sm.Target
    local name = target:GetLocal("Name")
    
	print(name .. " is getting hungry. " .. name .." is going to find something to eat.")
end

st_findfood.Execute = function ()
	local sm = st_findfood.SM
	local target = sm.Target
    local targetFish = game:GetNearestByTag("fish", target.Position)
    
	target:SetLocal("SeekTarget", targetFish)
	-- TODO fuzzy logic
	sm_shark:SetState(st_seektarget)
end

st_findfood.Exit = function ()
	local sm = st_findfood.SM
	local target = sm.Target
    local name = target:GetLocal("Name")
    
	print(name .. " has found food.")
end