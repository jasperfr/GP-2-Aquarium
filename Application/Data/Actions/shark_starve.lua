-- shark_starve - called on update, check if shark has no hunger left
-- The shark gets hungry with each game tick
-- if no hunger left the shark will lose health
shark_starve = function(entity)
    local hunger = entity:GetLocal("Hunger")
    if hunger > 0 then
        hunger = hunger - 0.02
        entity:SetLocal("Hunger", hunger)
    else
        local health = entity:GetLocal("Health")
        health = health - 0.1
        entity:SetLocal("Health", health)
    end
end