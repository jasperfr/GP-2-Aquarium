shark_move = function(entity)
    if entity.Velocity.X > 0 then
        entity.BaseSprite = game:GetSprite("spr_SharkRight")
    else
        entity.BaseSprite = game:GetSprite("spr_SharkLeft")
    end
end