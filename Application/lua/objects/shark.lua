state_sharkwander = EntityState()

state_sharkwander.Enter = function(entity)
    -- Create a new starting target vector.
    local target_vector = Vector2(math.random(1280), math.random(400))
    entity:Set("Target", target_vector)
    entity:Say(entity.Alias .. " is wandering to " .. target_vector.X .. "," .. target_vector.Y)
end
state_sharkwander.Execute = function(entity)
    local sm = state_sharkwander.StateMachine
    local position = entity.Position

    -- Is the shark hungry?
    local hunger = entity:Get("Hunger")
    if hunger < 10 and game:Has("fish") then
        sm:SetState(state_sharkhunt)
        return
    end

    -- Is the shark close to the target?
    local target_vector = entity:Get("Target")
    if Vector2.Distance(position, target_vector) < 20 then
        sm:SetState(state_sharkwander)
        return
    end

    entity:AddForce(Behaviours.Arrive(entity, target_vector))
end
state_sharkwander.Exit = function(entity) 
end


state_sharkhunt = EntityState()

state_sharkhunt.Enter = function(entity)
    entity:Say(entity.Alias .. " is getting hungry.")
    local target_fish = game:GetNearestByTagName("fish", entity.Position)
    entity:Set("SeekTarget", target_fish)
end
state_sharkhunt.Execute = function(entity)
    local sm = state_sharkhunt.StateMachine
    local target = entity:Get("SeekTarget")
    
    -- If the target does not exist, find another target.
    if not game:Exists(target) then
        entity:Say(entity.Alias .. "'s target is gone!")
        entity:Set("SeekTarget", nil)
        -- Are there still fishes left?
        if game:Has("fish") then 
            entity:Say(entity.Alias .. " is going to find another fish.")
            local target_fish = game:GetNearestByTagName("fish", entity.Position)
            entity:Set("SeekTarget", target_fish)
        -- No fishes left.
        else 
            entity:Say(entity.Alias .. " is hungry but there are no more fish. It will starve soon.")
            sm:SetState(state_sharkwander)
        end
        return
    end

    -- Seek the target.
    entity:AddForce(Behaviours.Seek(entity, target.Position))

    -- Target close? Eat (remove) the target.
    if Vector2.Distance(entity.Position, target.Position) < 70 then
        entity:Set("SeekTarget", nil)
        game:Destroy(target)
        entity:Say(entity.Alias .. " ate a fish!")

        local hunger = entity:Get("Hunger")
        entity:Set("Hunger", hunger + 5)

        -- Still hungry?
        if hunger < 10 then 
            entity:Say(entity.Alias .. " is still hungry.")
            -- Are there still fishes left?
            if game:Has("fish") then 
                entity:Say(entity.Alias .. " is going to find another fish.")
                local target_fish = game:GetNearestByTagName("fish", entity.Position)
                entity:Set("SeekTarget", target_fish)
            -- No fishes left.
            else 
                entity:Say(entity.Alias .. " is hungry but there are no more fish. It will starve soon.")
                sm:SetState(state_sharkwander)
            end
        -- Not hungry anymore.
        else
            entity:Say(entity.Alias .. " is well fed.")
            sm:SetState(state_sharkwander)
        end
        
        return
    end

    -- Target too far? Seek another.
    if Vector2.Distance(entity.Position, target.Position) > 500 then
        entity:Say(entity.Alias .. " couldn't find their target.")
        local target_fish = game:GetNearestByTagName("fish", entity.Position)
        entity:Set("SeekTarget", target_fish)
    end
end
state_sharkhunt.Exit = function(entity)
    entity:Say(entity.Alias .. " stopped hunting.")
end

shark = GameObject("shark")
shark:Set("Health", 100)
shark:Set("Hunger", 15)
shark.Mass = 50.0
shark.MaxSpeed = 4.5
shark.Size = 96.0
shark.BaseSprite = spr_shark_right
game:AddObject("obj_shark", shark)

shark.StepEvent = function(entity)
    if entity.Velocity.X > 0 then
        entity.BaseSprite = game:GetSprite("spr_SharkRight")
    else
        entity.BaseSprite = game:GetSprite("spr_SharkLeft")
    end
    
    local hunger = entity:Get("Hunger")
    if hunger > 0 then
        hunger = hunger - 0.02
        entity:Set("Hunger", hunger)
    else
        local health = entity:Get("Health")
        health = health - 0.1
        entity:Set("Health", health)
    end
end