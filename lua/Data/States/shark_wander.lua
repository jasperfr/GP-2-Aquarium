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