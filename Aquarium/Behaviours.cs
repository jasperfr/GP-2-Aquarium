﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Aquarium
{
    public static class Behaviours
    {
        public static Vector2 Seek(MovingEntity entity, Vector2 target)
        {
            Vector2 desiredVelocity = Vector2.Normalize(target - entity.Position) * entity.MaxSpeed;
            return desiredVelocity - entity.Velocity;
        }
        
        public static Vector2 Flee(MovingEntity entity, Vector2 target)
        {
            Vector2 desiredVelocity = Vector2.Normalize(entity.Position - target) * entity.MaxSpeed;
            return desiredVelocity - entity.Velocity;
        }
        
        public static Vector2 Arrive(MovingEntity entity, Vector2 target)
        {
            Vector2 toTarget = target - entity.Position;
            float dist = toTarget.Length();

            float rampSpeed = entity.MaxSpeed * dist / 50f;
            float clipSpeed = Math.Min(rampSpeed, entity.MaxSpeed);
            Vector2 desired = (clipSpeed / dist) * toTarget;
            return desired - entity.Velocity;
        }

        public static Vector2 Flock(MovingEntity entity, List<MovingEntity> flockWith)
        {
            List<MovingEntity> neighbors = flockWith.Where(tgt => tgt != entity
            && Vector2.Distance(entity.Position, tgt.Position) < 50).ToList();

            // prevent execution if list is empty
            if(neighbors.Count == 0) {
                return new Vector2(0, 0);
            }

            // separation
            Vector2 separationForce = new Vector2(0, 0);
            neighbors.ForEach(tgt => {
                Vector2 toAgent = entity.Position - tgt.Position;
                separationForce += Vector2.Normalize(toAgent / toAgent.Length());
            });
            
            // cohesion
            Vector2 cohesionForce = new Vector2(0, 0);
            Vector2 centerOfMass = new Vector2(0, 0);
            neighbors.ForEach(tgt => {
                centerOfMass += tgt.Position;
            });
            centerOfMass /= neighbors.Count;
            cohesionForce = Vector2.Normalize(centerOfMass - entity.Position) * entity.MaxSpeed - entity.Velocity;

            // alignment
            Vector2 alignmentForce = new Vector2(0, 0);
            Vector2 averageHeading = new Vector2();
            neighbors.ForEach(tgt => {
                averageHeading += tgt.Direction;
            });
            alignmentForce = (averageHeading / neighbors.Count) - entity.Direction;

            // calculate flocking force
            return (cohesionForce + alignmentForce + separationForce) / 3.0f;
        }
    }
}
