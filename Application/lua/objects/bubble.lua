bubble = GameObject()
bubble.Locals:Add("vspeed", 3)
bubble.Locals:Add("sinoid", 0.2)
bubble.Size = 24.0
bubble.BaseSprite = spr_bubble
game:AddObject("obj_bubble", bubble)

bubble.StepEvent = function(entity)
    local sin = entity.Locals:Get("sinoid")
    sin = sin + 0.01
    entity.Locals:Add("sinoid", sin)

    local vector = entity.Position
    entity.Position = Vector2(vector.X + math.sin(sin), vector.Y - entity:GetLocal("vspeed"))
end