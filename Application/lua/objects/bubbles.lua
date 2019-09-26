bubbles = GameObject()
bubbles:Set("vpseed", 3)
bubbles:Set("sinoid", 0.2)
bubbles.Size = 24.0
bubbles.BaseSprite = spr_bubbles
game:AddObject("obj_bubbles", bubbles)

bubbles.StepEvent = function(entity)
    local sin = entity:Get('sinoid')
    sin = sin + 0.01
    entity:Set('sinoid', sin)

    local vector = entity.Position
    if vector.Y < 0 then vector.Y = 640 end
    entity.Position = Vector2(vector.X + math.sin(sin), vector.Y - entity:Get('vspeed'))
end
