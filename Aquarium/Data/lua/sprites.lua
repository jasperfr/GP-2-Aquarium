spr_bg = Sprite()
spr_bg:AddImage('Data/Sprites/background.png')
spr_bg.ImageSpeed = 0.0
game:AddSprite("background", spr_bg)

spr_fish_right = Sprite()
spr_fish_right:AddImage("Data/Sprites/fish_0_0.png")
spr_fish_right:AddImage("Data/Sprites/fish_0_1.png")
spr_fish_right:AddImage("Data/Sprites/fish_0_2.png")
spr_fish_right.ImageSpeed = 0.1
game:AddSprite("spr_FishRight", spr_fish_right)

spr_fish_left = Sprite()
spr_fish_left:AddImage("Data/Sprites/fish_2_0.png")
spr_fish_left:AddImage("Data/Sprites/fish_2_1.png")
spr_fish_left:AddImage("Data/Sprites/fish_2_2.png")
spr_fish_left.ImageSpeed = 0.1
game:AddSprite("spr_FishLeft", spr_fish_left)

spr_shark_left = Sprite()
spr_shark_left:AddImage("Data/Sprites/shark_2_0.png")
spr_shark_left:AddImage("Data/Sprites/shark_2_1.png")
spr_shark_left:AddImage("Data/Sprites/shark_2_2.png")
spr_shark_left.ImageSpeed = 0.1
game:AddSprite("spr_SharkLeft", spr_shark_left)

spr_shark_right = Sprite()
spr_shark_right:AddImage("Data/Sprites/shark_0.png")
spr_shark_right:AddImage("Data/Sprites/shark_1.png")
spr_shark_right:AddImage("Data/Sprites/shark_2.png")
spr_shark_right.ImageSpeed = 0.1
game:AddSprite("spr_SharkRight", spr_shark_right)

spr_bubble = Sprite()
spr_bubble:AddImage("Data/Sprites/bubble_0.png")
spr_bubble:AddImage("Data/Sprites/bubble_1.png")
spr_bubble:AddImage("Data/Sprites/bubble_2.png")
spr_bubble.ImageSpeed = 0.1
game:AddSprite("spr_Bubble", spr_bubble)

spr_bubbles = Sprite()
spr_bubbles:AddImage("Data/Sprites/bubbles_0.png")
spr_bubbles:AddImage("Data/Sprites/bubbles_1.png")
spr_bubbles:AddImage("Data/Sprites/bubbles_2.png")
spr_bubbles.ImageSpeed = 0.1
game:AddSprite("spr_Bubbles", spr_bubbles)
