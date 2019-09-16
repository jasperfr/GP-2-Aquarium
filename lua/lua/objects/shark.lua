state_sharkwander = State()

state_sharkwander.Enter = function()
	local entity = state_sharkwander.Handle.Entity

    -- Create a new starting target vector.
    local target_vector = Vector2(math.random(1280), math.random(720))
    entity:SetLocal("Target", target_vector)
end
state_sharkwander.Execute = function()
	local entity = state_sharkwander.Handle.Entity
    local position = entity.Position
    
    -- Is the shark hungry?
    local hunger = entity:GetLocal("Hunger")
    if hunger < 10 and game:Has("fish") then
        state_sharkwander.Handle:SetState(state_sharkhunt)
        return
    end

    -- Is the shark close to the target?
    local target_vector = entity:GetLocal("Target")
    if Vector2.Distance(position, target_vector) < 20 then
        state_sharkwander.Handle:SetState(state_sharkwander)
        return
    end

    entity:AddForce(Behaviours.Arrive(entity, target_vector))
end
state_sharkwander.Exit = function() 
end

state_sharkhunt = State()

state_sharkhunt.Enter = function()
    local entity = state_sharkhunt.Handle.Entity

    -- Create a new fish target.
    local target_fish = game:GetNearestByTagName("fish", entity.Position)

    entity:SetLocal("SeekTarget", target_fish)
end
state_sharkhunt.Execute = function()
    local entity = state_sharkhunt.Handle.Entity
    local target = entity:GetLocal("SeekTarget")
    
    -- If the target does not exist, go back to wander state.
    if not game:Exists(target) then
        entity:RemoveLocal("SeekTarget")
        state_sharkhunt.Handle:SetState(state_sharkwander)
        return
    end

    -- Seek the target.
    entity:AddForce(Behaviours.Seek(entity, target.Position))

    -- Target close? Eat (remove) the target.
    if Vector2.Distance(entity.Position, target.Position) < 50 then
        entity:RemoveLocal("SeekTarget")
        game:DestroyEntity(target)
        local hunger = entity:GetLocal("Hunger")
        entity:SetLocal("Hunger", hunger + 5)
        state_sharkhunt.Handle:SetState(state_sharkwander)
    end
end
state_sharkhunt.Exit = function()
end

shark = GameObject("shark")
shark:SetLocal("Health", 100)
shark:SetLocal("Hunger", 15)
shark.Mass = 50.0
shark.MaxSpeed = 4.5
shark.Size = 72.0
shark.BaseSprite = spr_shark_right
shark:AddAction(shark_move)
shark:AddAction(shark_starve)
game:AddObject("obj_shark", shark)

local sm_shark = StateMachine(shark)
sm_shark:SetState(state_sharkwander)
game:AddStateMachine(sm_shark)