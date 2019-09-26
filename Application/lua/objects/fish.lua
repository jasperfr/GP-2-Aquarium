fish = GameObject("fish")
fish.Mass = 20.0
fish.MinSpeed = 2.0
fish.MaxSpeed = 3.5
fish.Size = 10.0
fish.BaseSprite = spr_fish_left
fish.IsSpatial = true
game:AddObject("obj_fish", fish)

fish.StepEvent = function(entity)
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
    entity:AddForce(Behaviours.Flock(entity, flock))

    if entity.Position.X > 1280 then
        entity.Position = Vector2(0, entity.Position.Y)
    end
    if entity.Position.X < 0 then
        entity.Position = Vector2(1280, entity.Position.Y)
    end
    if entity.Position.Y < 48 then
        entity.Position = Vector2(entity.Position.X, entity.Position.Y + 1)
        entity.Velocity = Vector2(entity.Velocity.X, math.abs(entity.Velocity.Y) * 0.5)
    end
    if entity.Position.Y > 600 then
        entity.Position = Vector2(entity.Position.X, entity.Position.Y - 1)
        entity.Velocity = Vector2(entity.Velocity.X, -math.abs(entity.Velocity.Y) * 0.5)
    end
end

--[[
    // Might have to change this. Loops the room around and stops moving at certain y-level.
    if(Position.X > 1280) Position.X = 0;
    if(Position.X < 0) Position.X = 1280;
    if(Position.Y < 48) {
        Position.Y++;
        Velocity.Y = Math.Abs(Velocity.Y) * 0.5f;
    }
    if(Position.Y > 600) {
        Position.Y--;
        Velocity.Y = -Math.Abs(Velocity.Y) * 0.5f;
    }
]]--