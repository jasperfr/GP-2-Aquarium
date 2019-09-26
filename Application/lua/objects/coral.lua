coral = GameObject()
coral.Size = 48.0
coral.BaseSprite = spr_coral
game:AddObject("obj_coral", coral)

coral.StepEvent = function(entity)
    local startPos = entity.StartPosition
    local xpos = entity:Get("xpos") or startPos.Y / 32
    xpos = xpos + 0.01
    entity:Set("xpos", xpos)
    entity.Position = Vector2(startPos.X + math.cos(xpos) * 16, startPos.Y)
end