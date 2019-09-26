baba = GameObject("baba")
baba.Mass = 20.0
baba.MinSpeed = 2.0
baba.MaxSpeed = 3.5
baba.Size = 48.0
baba.BaseSprite = spr_baba_right_1
game:AddObject("obj_baba", baba)

baba.StepEvent = function(entity)
    local dx = entity:Get("dx") or 0
    local dy = entity:Get("dy") or 0
    local moving = dx == 0 and dy == 0

    if dx < 0 and not moving then
        entity:Set("dx", entity:Get("dx") + 8)
        local vector = entity.Position
        entity.Position = Vector2(vector.X - 8, vector.Y)
    end
    if dx > 0 and not moving then
        entity:Set("dx", entity:Get("dx") - 8)
        local vector = entity.Position
        entity.Position = Vector2(vector.X + 8, vector.Y)
    end
    if dy < 0 and not moving then
        entity:Set("dy", entity:Get("dy") + 8)
        local vector = entity.Position
        entity.Position = Vector2(vector.X, vector.Y - 8)
    end
    if dy > 0 and not moving then
        entity:Set("dy", entity:Get("dy") - 8)
        local vector = entity.Position
        entity.Position = Vector2(vector.X, vector.Y + 8)
    end

    --[[
    local float = entity:Get("float") or 0
    float = float + 0.05
    entity:Set("float", float)
    local vector = entity.Position
    entity.Position = Vector2(vector.X, vector.Y + math.cos(float))
    ]]--
end

baba.KeyboardEvent = function(key, entity)
    if(key == 68) then
        entity.BaseSprite = spr_baba_right_1
        entity:Set("dx", 48)
    end
    if(key == 65) then
        entity.BaseSprite = spr_baba_left_1
        entity:Set("dx", -48)
    end
    if(key == 87) then
        entity.BaseSprite = spr_baba_up_1
        entity:Set("dy", -48)
    end
    if(key == 83) then
        entity.BaseSprite = spr_baba_down_1
        entity:Set("dy", 48)
    end
end