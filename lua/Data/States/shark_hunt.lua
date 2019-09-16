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