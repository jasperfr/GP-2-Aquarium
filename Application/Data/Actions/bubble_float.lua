bubble_float = function(entity)
    local sin = entity:GetLocal("sinoid")
    sin = sin + 0.01
    entity:SetLocal("sinoid", sin)

    local vector = entity.Position
    entity.Position = Vector2(vector.X + math.sin(sin), vector.Y)
end