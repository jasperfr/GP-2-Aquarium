rocks = GameObject()
rocks.Size = 48.0
rocks.BaseSprite = spr_rocks
game:AddObject("obj_rocks", rocks)

rocks.StepEvent = function(entity)
    local startPos = entity.StartPosition
    local xpos = entity:Get("xpos") or startPos.Y / 32
    xpos = xpos + 0.01
    entity:Set("xpos", xpos)
    entity.Position = Vector2(startPos.X + math.cos(xpos) * 16, startPos.Y)
end