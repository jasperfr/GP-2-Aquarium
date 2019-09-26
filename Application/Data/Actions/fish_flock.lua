fish_flock = function(entity)
    local sharks = game:GetEntitiesByTagName("shark")
    for i = 0, sharks.Count - 1 do
        if Vector2.Distance(entity.Position, sharks[i].Position) < 200 then
            entity:AddForce(Behaviours.Flee(entity, sharks[i].Position))
        end
    end

    if entity.Velocity.X > 0 then
        entity.BaseSprite = game:GetSprite("spr_FishRight")
    else
        entity.BaseSprite = game:GetSprite("spr_FishLeft")
    end

    local flock = game:GetAdjacentEntitiesByTagName(entity, "fish")
    -- local flock = game:GetEntitiesByTagName("fish")
    entity:AddForce(Behaviours.Flock(entity, flock))
end